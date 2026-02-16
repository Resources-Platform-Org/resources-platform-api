using Api.Contracts;
using Api.Controllers;
using Api.Dtos.Auth;
using Api.Dtos.Users;
using Api.Wrappers;
using AutoMapper;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route(ApiRoutes.Identity.Controller)]
    [Produces("application/json")]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="dto">User registration details</param>
        /// <returns>Created user information</returns>
        /// <response code="200">Returns the newly created user</response>
        /// <response code="400">If the item is null or validation fails</response>
        [HttpPost(ApiRoutes.Identity.Register)]
        [ProducesResponseType(typeof(ApiResponse<UserResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 400)]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto.Name, dto.Email, dto.Password);

            if (!result.Success)
            {
                return ErrorResponse(result.Message, 400);
            }

            var response = _mapper.Map<UserResponseDto>(result.user);
            return SuccessResponse(response, result.Message);
        }

        /// <summary>
        /// Optimizes user login
        /// </summary>
        /// <param name="dto">Login credentials</param>
        /// <returns>JWT Token and user info</returns>
        /// <response code="200">Returns the user with token</response>
        /// <response code="401">If credentials are invalid</response>
        [HttpPost(ApiRoutes.Identity.Login)]
        [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 401)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto.Email, dto.Password);
            if (!result.Success)
            {
                return ErrorResponse(result.Message, 401);
            }
            var response = _mapper.Map<AuthResponseDto>(result.user);
            response.Token = result.Token!;

            return SuccessResponse(response, "Login successful");
        }
    }
}
