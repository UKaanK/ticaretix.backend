using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ticaretix.Core.Interfaces
{
    public interface IRedisService
    {
        void SetUserToken(string userId,string deviceId, string token);
        string GetUserToken(string userId,string deviceId);
        void RemoveUserToken(string userId,string deviceId);
        string GetUserIdByToken(string token);
        void RemoveAllUserTokens(string userId);
        Task<bool> IsRateLimitedAsync(string key);
        Task SetRateLimitAsync(string key, TimeSpan expirationTime);
       , Task ResetLoginAttemptsAsync(string key);

    }
}
