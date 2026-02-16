using Microsoft.AspNetCore.Mvc;
using Api.Contracts;
using Microsoft.AspNetCore.Authorization;
using Core.Interfaces;
using AutoMapper;
using Infrastructure.Services;
using Core.Setting;
using System.Security.Claims;
using Api.Dtos.Users;
using Api.Dtos;

namespace Api.Controllers;

[ApiController]
[Route(ApiRoutes.Users.Controller)]
[Authorize]
[Produces("application/json")]
public class UserController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAuthService _authService;
    private readonly FileServices _fileService;
    private readonly FileSetting _fileSetting;

    public UserController(IUnitOfWork unitOfWork,
            IMapper mapper,
            IAuthService authService,
            FileServices fileService,
            FileSetting fileSetting)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _authService = authService;
        _fileService = fileService;
        _fileSetting = fileSetting;
    }

    /// <summary>
    /// Gets the current user's profile
    /// </summary>
    /// <returns>User profile information</returns>
    /// <response code="200">Returns the user profile</response>
    /// <response code="404">If user is not found</response>
    [HttpGet(ApiRoutes.Users.GetMe)]
    [ProducesResponseType(typeof(ApiResponse<UserProfileDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<string>), 404)]
    public async Task<IActionResult> GetProfile()
    {
        var userId = GetCurrentUserId();
        var user = await _unitOfWork.Users.FindAsync(userId);

        if (user == null)
            return ErrorResponse("User not found", 404);
        var response = _mapper.Map<UserProfileDto>(user);

        if (!string.IsNullOrEmpty(user.ProfilePicture))
            response.ProfilePictureUrl = $"{Request.Scheme}://{Request.Host}/uploads/users/{user.ProfilePicture}";

        return SuccessResponse(response);
    }
    
    /// <summary>
    /// Change the current user's password
    /// </summary>
    /// <param name="dto">Old and new passwords</param>
    /// <response code="200">If password changed successfully</response>
    /// <response code="400">If old password is incorrect or validation fails</response>
    [HttpPost(ApiRoutes.Users.ChangePassword)]
    [ProducesResponseType(typeof(ApiResponse<string>), 200)]
    [ProducesResponseType(typeof(ApiResponse<string>), 400)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userId = GetCurrentUserId();
        var result = await _authService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);

        if (!result.IsSuccess)
        {
            return ErrorResponse(result.Message, 400);
        }
        return SuccessResponse(result.Message);
    }

    /// <summary>
    /// Uploads a profile picture for the current user
    /// </summary>
    /// <param name="dto">Image file (jpg, jpeg, png)</param>
    /// <returns>New image URL</returns>
    /// <response code="200">Image uploaded successfully</response>
    /// <response code="400">Invalid file type or size</response>
    [HttpPost("image")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ApiResponse<object>), 200)]
    [ProducesResponseType(typeof(ApiResponse<string>), 400)]
    public async Task<IActionResult> UploadImage([FromForm] UploadeImageDto dto)
    {
        var allowedExt = new[] { ".jpg", ".jpeg", ".png"};
        var ext = Path.GetExtension(dto.File.FileName).ToLower();
        if (!allowedExt.Contains(ext))
            return ErrorResponse("Invalid file type. Only .jpg, .jpeg, and .png are allowed.", 400);

        if (dto.File.Length > 2 * 1024 * 1024)
            return ErrorResponse("File size exceeds the limit of 2MB.", 400);

        var userId = GetCurrentUserId();
        var user = await _unitOfWork.Users.FindAsync(userId);

        if (!string.IsNullOrEmpty(user!.ProfilePicture))
        {
            try
            {
                _fileService.DeleteFile(user.ProfilePicture, _fileSetting.UserImagesFolder);
            }
            catch (Exception ex)
            {
                return ErrorResponse($"Error deleting file: {ex.Message}", 500);
            }
        }

        var fileName = await _fileService.UploadFile(dto.File, _fileSetting.UserImagesFolder);

        user.ProfilePicture = fileName;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return SuccessResponse(new { imaUrl = GetFullImageUrl(fileName)},"Image uploaded successfully.");
    }

    /// <summary>
    /// Deletes the current user's profile picture
    /// </summary>
    /// <response code="200">Image deleted successfully</response>
    /// <response code="400">If no image exists</response>
    [HttpDelete("image")]
    [ProducesResponseType(typeof(ApiResponse<object>), 200)]
    [ProducesResponseType(typeof(ApiResponse<string>), 400)]
    public async Task<IActionResult> DeleteImage()
    {
        var userId = GetCurrentUserId();
        var user = await _unitOfWork.Users.FindAsync(userId);

        if (string.IsNullOrEmpty(user!.ProfilePicture))
            return ErrorResponse("No profile picture to delete.", 400);

        _fileService.DeleteFile(user.ProfilePicture, _fileSetting.UserImagesFolder);

        user.ProfilePicture = null;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return SuccessResponse<object>(null!,"Profile picture deleted successfully.");
    }

    /// <summary>
    /// Get paginated list of users (Admin only)
    /// </summary>
    /// <param name="query">Pagination parameters</param>
    /// <returns>Paged list of users</returns>
    [HttpGet(ApiRoutes.Users.GetPaged)]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PagedResponse<UserProfileDto>), 200)]
    public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query)
    {
        var result =
            await _unitOfWork.Users.GetPagedAsync(
                query.PageNumber , 
                query.PageSize ,
                null , 
                false
            );
        var response = _mapper.Map<IEnumerable<UserProfileDto>>(result.Items);

        // full userpicture url
        foreach (var item in response)
        {
            var originalEntity = result.Items.FirstOrDefault(u => u.Id == item.Id);
            item.ProfilePictureUrl = GetFullImageUrl(originalEntity?.ProfilePicture);
        }
        return PagedResponse(response, query.PageNumber,query.PageSize , result.TotalCount);
    }

    /// <summary>
    /// Changes a user's role (Admin only)
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="dto">New role</param>
    [HttpPut("{id}/role")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<string>), 200)]
    [ProducesResponseType(typeof(ApiResponse<string>), 400)]
    [ProducesResponseType(typeof(ApiResponse<string>), 404)]
    public async Task<IActionResult> ChangeRole(int id, [FromBody] ChangeRoleDto dto)
    {
        var currentUserId = GetCurrentUserId();
        if (currentUserId == id)
            return ErrorResponse("You cannot change your own role.", 400);

        var user = await _unitOfWork.Users.FindAsync(id);
        if (user == null)
            return ErrorResponse("User not found", 404);

        user.Role = dto.NewRole;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return SuccessResponse(
            _mapper.Map<UserProfileDto>(user)
            ,"User role updated successfully."
        );
    }

    [HttpDelete(ApiRoutes.Users.Delete)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var currentUserId = GetCurrentUserId();
        if (currentUserId == id)
            return ErrorResponse("You cannot delete your own account.", 400);
        
        var user = await _unitOfWork.Users.FindAsync(id);
        if (user == null)
            return ErrorResponse("User not found", 404);

        // delete profile picture before deleting user
        if (!string.IsNullOrEmpty(user.ProfilePicture))
        {
            _fileService.DeleteFile(user.ProfilePicture, _fileSetting.UserImagesFolder);
        }

        await _unitOfWork.Users.DeleteAsync(x => x.Id == id);
        return SuccessResponse<object>(null!,"User deleted successfully.");
    }

    // Helper method to construct full image URL
    private string? GetFullImageUrl(string? fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            return null;
        return $"{Request.Scheme}://{Request.Host}/{_fileSetting.UserImagesFolder}/{fileName}";
    }
    // Helper method to get current user ID from claims
    private int GetCurrentUserId()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (claim == null)
        {
            throw new Exception("User ID claim not found");
        }
        return int.Parse(claim.Value);
    }
}