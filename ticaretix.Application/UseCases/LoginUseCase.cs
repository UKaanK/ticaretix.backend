using System;
using System.Threading.Tasks;
using ticaretix.Application.Dtos;
using ticaretix.Application.Interfaces;
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
        public async Task<string> ExecuteAsync(KullaniciLoginDto loginDto, string deviceId)
        {
            if (loginDto == null)
            {
                throw new ArgumentNullException(nameof(loginDto), "LoginDto null olamaz.");
            }

            var user = await _userRepository.GetKullaniciByEmailAsync(loginDto.Email);
            if (user == null)
            {
                // Kullanıcı bulunamazsa giriş deneme sayısını artır
                await _redisService.IncrementDeviceLoginAttemptAsync(deviceId);
                throw new UnauthorizedAccessException("Geçersiz kullanıcı.");
            }

            string userId = user.KullaniciID.ToString();

            // Kullanıcının veya cihazın rate limit'e takılıp takılmadığını kontrol et
            if (await _redisService.IsUserRateLimitedAsync(userId) || await _redisService.IsDeviceRateLimitedAsync(deviceId))
            {
                throw new UnauthorizedAccessException("Çok fazla başarısız giriş denemesi. Lütfen daha sonra tekrar deneyin.");
            }

            // Kullanıcının eski token'ını sil
            await _redisService.RemoveUserToken(userId, deviceId);

            // Şifre doğrulaması
            if (!VerifyPassword(loginDto.Sifre, user.Sifre))
            {
                // Başarısız giriş denemesini artır
                await _redisService.IncrementUserLoginAttemptAsync(userId);
                await _redisService.IncrementDeviceLoginAttemptAsync(deviceId);

                throw new UnauthorizedAccessException("Şifre yanlış.");
            }

            // Kullanıcı başarılı giriş yaptıysa, giriş deneme sayısını sıfırla
            await _redisService.ResetUserLoginAttemptsAsync(userId);
            await _redisService.ResetDeviceLoginAttemptsAsync(deviceId);

            // Yeni token oluştur ve Redis'e kaydet
            var token = _jwtService.GenerateToken(user);
            await _redisService.SetUserToken(userId, deviceId, token);

            return token;
        }



        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            return enteredPassword == storedPassword;  // Burada hash'lenmiş şifre kontrolü yapılmalı
        }
    }
}
