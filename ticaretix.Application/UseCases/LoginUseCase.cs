using System;
using System.Threading.Tasks;
using ticaretix.Application.Dtos;
using ticaretix.Application.Interfaces;
using ticaretix.Core.Exceptions;
using ticaretix.Core.Interfaces;

namespace ticaretix.Application.UseCases
{
    public class LoginUseCase
    {
        private readonly IKullaniciRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IRedisService _redisService;

        public LoginUseCase(IKullaniciRepository userRepository, IJwtService jwtService, IRedisService redisService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _redisService = redisService;
        }
        public async Task<(string accessToken, string refreshToken)> ExecuteAsync(KullaniciLoginDto loginDto, string deviceId)
        {
            if (loginDto == null)
            {
                throw new ArgumentNullException(nameof(loginDto), "LoginDto null olamaz.");
            }

            var user = await _userRepository.GetKullaniciByEmailAsync(loginDto.Email);
            if (user == null)
            {
                await _redisService.IncrementDeviceLoginAttemptAsync(deviceId);
                throw new UnauthorizedAccessException("Geçersiz kullanıcı.");
            }

            string userId = user.KullaniciID.ToString();

            // Kullanıcı veya cihaz rate limit'e takıldı mı kontrol et
            if (await _redisService.IsUserRateLimitedAsync(userId) || await _redisService.IsDeviceRateLimitedAsync(deviceId))
            {
                throw new UnauthorizedAccessException("Çok fazla başarısız giriş denemesi. Lütfen daha sonra tekrar deneyin.");
            }

            // **Eski token'ı al ve sil**
            string oldToken = await _redisService.GetUserToken(userId, deviceId);
            if (!string.IsNullOrEmpty(oldToken))
            {
                await _redisService.RemoveUserToken(oldToken);
            }

            // Şifre doğrulama
            if (!VerifyPassword(loginDto.Sifre, user.Sifre))
            {
                await _redisService.IncrementUserLoginAttemptAsync(userId);
                await _redisService.IncrementDeviceLoginAttemptAsync(deviceId);
                throw new ApiException(ErrorCodes.S502);
            }

            // Başarılı giriş -> deneme sayacı sıfırla
            await _redisService.ResetUserLoginAttemptsAsync(userId);
            await _redisService.ResetDeviceLoginAttemptsAsync(deviceId);

            // Yeni access token ve refresh token oluştur
            var accessToken = _jwtService.GenerateToken(user);  // Access token oluşturuluyor
            var refreshToken = _jwtService.GenerateRefreshToken(user);  // Refresh token oluşturuluyor

            // Refresh token'ı Redis'e kaydet
            await _redisService.SetRefreshToken(userId, deviceId, refreshToken);

            // Access token'ı Redis'e kaydet
            await _redisService.SetUserToken(userId, deviceId, accessToken);

            // Refresh token ve access token'ı geri döndür
            return (accessToken, refreshToken);
        }

        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            return enteredPassword == storedPassword;  // Burada hash'lenmiş şifre kontrolü yapılmalı
        }
    }
}
