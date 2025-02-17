using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ticaretix.Application.Interfaces
{
    public interface IJwtDecoderService
    {
        Dictionary<string, string> DecodeToken(string token);
    }
}
