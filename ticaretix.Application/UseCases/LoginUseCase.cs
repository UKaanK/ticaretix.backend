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

        public async Task<string> ExecuteAsync(KullaniciLoginDto loginDto)
        {
            var user = await _userRepository.GetKullaniciByEmailAsync(loginDto.Email);

            if (user == null || !VerifyPassword(loginDto.Sifre, user.Sifre))
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var token = _jwtService.GenerateToken(user);

            // Redis'e token kaydetme
            _redisService.SetUserToken(user.KullaniciID.ToString(), token);

            return token;
        }

        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            // Password doğrulama işlemi (hashing vs)
            return enteredPassword == storedPassword;
        }
    }
}
