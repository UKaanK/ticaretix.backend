using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ticaretix.Application.Interfaces;
using ticaretix.Core.Entities;

namespace ticaretix.Application.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateToken(KullaniciEntity kullanici)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, kullanici.KullaniciID.ToString()),
            new Claim(ClaimTypes.Name, kullanici.KullaniciAdi),
            new Claim(ClaimTypes.Email, kullanici.Email),
            new Claim(ClaimTypes.Role, kullanici.Role),
            new Claim(ClaimTypes.Hash,kullanici.Sifre)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken(KullaniciEntity kullanici)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, kullanici.KullaniciID.ToString()),
            new Claim(ClaimTypes.Name, kullanici.KullaniciAdi),
                        new Claim(ClaimTypes.Email, kullanici.Email),
                                    new Claim(ClaimTypes.Hash,kullanici.Sifre),
                                                new Claim(ClaimTypes.Role, kullanici.Role),



        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var refreshToken = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: DateTime.UtcNow.AddDays(7), // Refresh token'ın süresi daha uzun (örneğin 7 gün)
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(refreshToken);
        }
    }



}
