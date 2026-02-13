using Api.Contracts;
using Api.Controllers;
using Api.Dtos.Auth;
using Api.Dtos.Users;
using AutoMapper;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route(ApiRoutes.Identity.Controller)]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
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

        [HttpPost(ApiRoutes.Identity.Login)]
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
