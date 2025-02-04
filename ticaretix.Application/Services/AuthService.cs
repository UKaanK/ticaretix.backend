using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticaretix.Application.Interfaces;
using ticaretix.Core.Entities;
using ticaretix.Core.Interfaces;

namespace ticaretix.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IKullaniciRepository _kullaniciRepository;
        private readonly IJwtService _jwtService;

        public AuthService(IKullaniciRepository kullaniciRepository, IJwtService jwtService)
        {
            _kullaniciRepository = kullaniciRepository;
            _jwtService = jwtService;
        }

        public async Task<string?> LoginAsync(string email, string password)
        {
            var kullanici = await _kullaniciRepository.GetKullaniciByEmailAsync(email);
            if (kullanici == null || kullanici.Sifre != password)
                return null;

            return _jwtService.GenerateToken(kullanici);
        }

        public async Task<KullaniciEntity> RegisterAsync(KullaniciEntity kullanici)
        {
            var existingUser = await _kullaniciRepository.GetKullaniciByEmailAsync(kullanici.Email);
            if (existingUser != null) throw new Exception("Kullanıcı zaten var");

            return await _kullaniciRepository.AddKullaniciAsync(kullanici);
        }
    }
}
