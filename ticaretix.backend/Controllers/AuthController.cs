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

        [HttpGet("get-user-by-token/{token}")]
        public IActionResult GetUserByToken(string token)
        {
            var userId = _redisService.GetUserIdByToken(token);

            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("User not found for this token.");
            }

            return Ok(new { userId });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] KullaniciLoginDto loginDto, [FromHeader] string deviceId)
        {
            try
            {
                if (string.IsNullOrEmpty(deviceId))
                    return BadRequest("Device ID is required.");

                var token = await _loginUseCase.ExecuteAsync(loginDto, deviceId);
                return Ok(new { token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid credentials");
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout([FromBody] KullanıcıLogoutDto logoutDto, [FromHeader] string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
                return BadRequest("Device ID is required.");

            _redisService.RemoveUserToken(logoutDto.UserId, deviceId);
            return Ok(new { message = "Logout successful for this device" });
        }

        [HttpPost("logout-all")]
        public IActionResult LogoutAll([FromBody] KullanıcıLogoutDto logoutDto)
        {
            _redisService.RemoveAllUserTokens(logoutDto.UserId);
            return Ok(new { message = "Logout successful for all devices" });
        }


    }
