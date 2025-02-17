using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticaretix.Application.Interfaces;

namespace ticaretix.Application.Services
{
    public class JwtDecoderService : IJwtDecoderService
    {
        public Dictionary<string, string> DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            return jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
        }
    }
}
