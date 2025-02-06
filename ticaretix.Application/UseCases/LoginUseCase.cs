using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            var rateLimitKey = $"login_attempts:{loginDto.Email}:{deviceId}";

            // 1. Rate Limit kontrolü
            if (await _redisService.IsRateLimitedAsync(rateLimitKey))
            {
                throw new UnauthorizedAccessException("Too many login attempts. Please try again later.");
            }

            // Kullanıcıyı al
            var user = await _userRepository.GetKullaniciByEmailAsync(loginDto.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Geçersiz kullanıcı.");
            }

            // Şifre doğrulama
            if (!VerifyPassword(loginDto.Sifre, user.Sifre))
            {
                // Başarısız giriş denemesi sonrası deneme sayısını arttır
                await _redisService.SetRateLimitAsync(rateLimitKey, TimeSpan.FromMinutes(1)); // 1 dakika süreyle
                throw new UnauthorizedAccessException("Şifre yanlış.");
            }

            // Rate limit'i sıfırla
            await _redisService.ResetLoginAttemptsAsync(rateLimitKey);

            // JWT token oluştur
            var token = _jwtService.GenerateToken(user);

            // Token'ı Redis'e kaydet
            _redisService.SetUserToken(user.KullaniciID.ToString(), deviceId, token);

            return token;
        }

        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            return enteredPassword == storedPassword;  // Burada şifre doğrulama işlemi yapılmalı (hashing vs.)
        }
    }
}
