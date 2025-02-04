using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ticaretix.Core.Interfaces
{
    public interface IRedisService
    {
        void SetUserToken(string userId, string token);
        string GetUserToken(string userId);
        void RemoveUserToken(string userId);
    }
}
