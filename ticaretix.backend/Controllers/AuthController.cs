using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ticaretix.Application.Dtos;
using ticaretix.Application.Interfaces;
using ticaretix.Application.Services;
using ticaretix.Application.UseCases;
using ticaretix.Core.Entities;
using ticaretix.Core.Interfaces;

namespace ticaretix.backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LoginUseCase _loginUseCase;

        private readonly IAuthService _authService;

        private readonly IRedisService _redisService;

        public AuthController(IAuthService authService,LoginUseCase loginUseCase,IRedisService redisService)
        {
            _authService = authService;
            _loginUseCase = loginUseCase;
            _redisService = redisService;

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] KullaniciLoginDto loginDto)
        {
            try
            {
                var token = await _loginUseCase.ExecuteAsync(loginDto);
                return Ok(new {token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid credentials");
            }
        }
        [HttpPost("logout")]
        public IActionResult Logout([FromBody] KullanıcıLogoutDto logoutDto)
        {


            _redisService.RemoveUserToken(logoutDto.UserId); // Doğru çağrı

            return Ok(new { message = "Logout successful" });
        }

        [HttpGet("test-token/{userId}")]
        public IActionResult TestToken(string userId)
        {
            var token = _redisService.GetUserToken(userId);

            if (string.IsNullOrEmpty(token))
            {
                return NotFound("Token not found.");
            }

            return Ok(new { token });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] KullaniciEntity newUser)
        {
            try
            {
                var registeredUser = await _authService.RegisterAsync(newUser);
                return Ok(registeredUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
