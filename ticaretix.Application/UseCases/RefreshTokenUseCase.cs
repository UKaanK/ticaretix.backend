using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ticaretix.Application.Interfaces;
using ticaretix.Core.Interfaces;

namespace ticaretix.Application.UseCases
{
    public class RefreshTokenUseCase
    {
        private readonly IJwtService _jwtService;
        private readonly IRedisService _redisService;

        public RefreshTokenUseCase(IJwtService jwtService, IRedisService redisService)
        {
            _jwtService = jwtService;
            _redisService = redisService;
        }

        public async Task<string> ExecuteAsync(string refreshToken, string deviceId)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken), "Refresh token boş olamaz.");
            }

            // Refresh Token'ı doğrula ve içinden kullanıcı bilgilerini al
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(refreshToken);

            var userId = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var email = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var role = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var pwd = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Hash)?.Value;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(email))
            {
                throw new UnauthorizedAccessException("Geçersiz refresh token.");
            }

            // Redis'ten refresh token doğrulaması yap
            var storedRefreshToken = await _redisService.GetRefreshToken(userId, deviceId);
            if (storedRefreshToken != refreshToken)
            {
                throw new UnauthorizedAccessException("Geçersiz refresh token.");
            }

            // Yeni access token oluştur
            var newAccessToken = _jwtService.GenerateToken(new ticaretix.Core.Entities.KullaniciEntity
            {
                KullaniciID = int.Parse(userId),
                Email = email,
                Role = role,
                Sifre=pwd
            });

            // Yeni access token'ı Redis'e kaydet
            await _redisService.SetUserToken(userId, deviceId, newAccessToken);

            return newAccessToken;
        }
    }
}
