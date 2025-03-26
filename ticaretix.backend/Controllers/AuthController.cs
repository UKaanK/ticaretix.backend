using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ticaretix.Application.Dtos;
using ticaretix.Application.Interfaces;
using ticaretix.Application.Services;
using ticaretix.Application.UseCases;
using ticaretix.Core.Entities;
using ticaretix.Core.Interfaces;
using ticaretix.Core.Exceptions;
using Serilog;
using System.Threading.Tasks;

namespace ticaretix.backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LoginUseCase _loginUseCase;
        private readonly RefreshTokenUseCase _refreshTokenUseCase;
        private readonly IAuthService _authService;
        private readonly IRedisService _redisService;

        public AuthController(IAuthService authService, LoginUseCase loginUseCase, RefreshTokenUseCase refreshTokenUseCase, IRedisService redisService)
        {
            _authService = authService;
            _loginUseCase = loginUseCase;
            _refreshTokenUseCase = refreshTokenUseCase;
            _redisService = redisService;
        }

        [HttpGet("get-user-by-token/{token}")]
        public IActionResult GetUserByToken(string token)
        {
            Log.Information("Token doğrulama işlemi başlatıldı: Token={Token}", token);

            var userId = _redisService.GetUserIdByToken(token);

            if (string.IsNullOrEmpty(userId))
            {
                Log.Warning("Kullanıcı bulunamadı: Token={Token}", token);
                throw new ApiException(ErrorCodes.U101);
            }

            Log.Information("Kullanıcı başarıyla bulundu: UserId={UserId}", userId);
            return Ok(new { userId });
        }
        /// <summary>
        /// Login Methodu
        /// </summary>
        /// <param name="loginDto"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] KullaniciLoginDto loginDto, [FromHeader] string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                Log.Warning("Eksik alan: deviceId={DeviceId}", deviceId);
                throw new ApiException(ErrorCodes.V303);
            }

            Log.Information("Login işlemi başlatıldı: DeviceId={DeviceId}", deviceId);

            var token = await _loginUseCase.ExecuteAsync(loginDto, deviceId);
            if (token == default)
            {
                Log.Warning("Login başarısız: DeviceId={DeviceId}", deviceId);
                throw new ApiException(ErrorCodes.U101);
            }

            Log.Information("Login başarılı: AccessToken={AccessToken}", token.accessToken);
            return Ok(new { accessToken = token.accessToken, refreshToken = token.refreshToken });
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromHeader] string refreshToken, [FromHeader] string deviceId)
        {
            if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(deviceId))
            {
                throw new ApiException(ErrorCodes.U204);
            }

            var newAccessToken = await _refreshTokenUseCase.ExecuteAsync(refreshToken, deviceId);

            if (newAccessToken == null)
            {
                throw new ApiException(ErrorCodes.U204);
            }

            return Ok(new { accessToken = newAccessToken });
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromHeader] string token, [FromHeader] string deviceId)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(deviceId))
            {
                Log.Warning("Eksik token veya deviceId: Token={Token}, DeviceId={DeviceId}", token, deviceId);
                throw new ApiException(ErrorCodes.U204);
            }

            Log.Information("Logout işlemi başlatıldı: Token={Token}, DeviceId={DeviceId}", token, deviceId);

            await _redisService.RemoveUserToken(token);
            Log.Information("Logout başarılı: Token={Token}, DeviceId={DeviceId}", token, deviceId);
            return Ok(new { message = "Logout successful" });
        }


        [HttpGet("test-log")]
        public IActionResult TestLog()
        {
            Log.Information("Test log çalıştırıldı!");
            return Ok("Log başarıyla yazıldı!");
        }
    }
}
