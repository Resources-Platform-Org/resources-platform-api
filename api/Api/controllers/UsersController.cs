using Api.Dtos.Users;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // 1. GET /api/users/me
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            // Assuming the ID is stored in the NameIdentifier claim
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            var user = await _unitOfWork.Users.FindAsync(userId);
            if (user == null) return NotFound();

            var result = new UserResponseDto
            {
                UserID = user.UserID,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role.ToString()
            };

            return Ok(result);
        }

        // 2. GET /api/users/{id} - Admin only
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _unitOfWork.Users.FindAsync(id);
            if (user == null) return NotFound();

            var result = new UserResponseDto
            {
                UserID = user.UserID,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role.ToString()
            };

            return Ok(result);
        }

        // 3. GET /api/users - Admin only (Paged)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            // Basic pagination implementation
            // Note: IGenericRepository.GetAllAsync returns IEnumerable, so we are doing in-memory pagination here 
            // unless we update the repository to support IQueryable or database-side pagination.
            // For now, fetching all and taking page.
            
            var users = await _unitOfWork.Users.GetAllAsync(null);
            
            var totalRecords = users.Count();
            var pagedUsers = users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserResponseDto
                {
                    UserID = u.UserID,
                    Username = u.Username,
                    Email = u.Email,
                    Role = u.Role.ToString()
                });

            return Ok(new
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = pagedUsers
            });
        }

        // 4. POST /api/users - Create a new user
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            if (await _unitOfWork.Users.IsUsernameOrEmailTakenAsync(dto.Username, dto.Email))
            {
                return BadRequest("Username or Email is already taken.");
            }

            // For now, storing as plain text or simple placeholder as requested by context limitations.
            // I Will Do it in Feature Implementation.
            
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = dto.Password,
                Role = enRoles.User // Default role
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var result = new UserResponseDto
            {
                UserID = user.UserID,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role.ToString()
            };

            // Assuming GetById is accessible or we just return Created
            return CreatedAtAction(nameof(GetById), new { id = user.UserID }, result);
        }

        // 5. PUT /api/users/{id} - Admin only
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
        {
            var user = await _unitOfWork.Users.FindAsync(id);
            if (user == null) return NotFound();

            // Check if username changed and is taken
            if (user.Username != dto.Username && await _unitOfWork.Users.ExistsAsync(u => u.Username == dto.Username))
            {
                return BadRequest("Username is already taken.");
            }

            // Check if email changed and is taken
            if (user.Email != dto.Email && await _unitOfWork.Users.ExistsAsync(u => u.Email == dto.Email))
            {
                return BadRequest("Email is already taken.");
            }

            user.Username = dto.Username;
            user.Email = dto.Email;
            user.Role = dto.Role;

            await _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            var result = new UserResponseDto
            {
                UserID = user.UserID,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role.ToString()
            };

            return Ok(result);
        }

        // 6. DELETE /api/users/{id} - Admin only
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _unitOfWork.Users.FindAsync(id);
            if (user == null) return NotFound();

            await _unitOfWork.Users.DeleteAsync(u => u.UserID == id);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
