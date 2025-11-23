using Api.Dtos.Users;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.controllers
{
    [ApiController]
    [Route("Auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtService _jwtService;

        public AuthController(IUnitOfWork unitOfWork, JwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Api.Dtos.Users.LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { message = "Username and password are required." });

            var user = await _unitOfWork.Users.GetAsync(u => u.Username == request.Username);
            
            if (user == null || user.PasswordHash != request.Password)
                return Unauthorized(new { message = "Invalid username or password." });

            var token = _jwtService.GenerateToken(user);
            return Ok(new LoginResponse
            {
                Token = token,
                Username = user.Username,
                Role = user.Role.ToString()
            });
        }
    }
}
