using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticaretix.Core.Entities;

namespace ticaretix.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(KullaniciEntity kullanici);
    }
}
