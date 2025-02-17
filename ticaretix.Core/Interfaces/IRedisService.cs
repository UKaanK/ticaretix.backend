using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ticaretix.Core.Interfaces
{
    public interface IRedisService
    {
        Task SetUserToken(string userId,string deviceId, string token);
        Task<string> GetUserToken(string userId,string deviceId);
        Task<string> GetUserIdByRefreshTokenAsync(string refreshToken, string deviceId);

        Task RemoveUserToken(string token);
        string GetUserIdByToken(string token);
        void RemoveAllUserTokens(string userId);
        Task<bool> IsRateLimitedAsync(string key);
        Task SetRateLimitAsync(string key, TimeSpan expirationTime);
        Task ResetLoginAttemptsAsync(string key);
        Task<bool> IsUserRateLimitedAsync(string userId);
        Task<bool> IsDeviceRateLimitedAsync(string deviceId);
        Task IncrementUserLoginAttemptAsync(string userId);
        Task ResetDeviceLoginAttemptsAsync(string deviceId);
        Task IncrementDeviceLoginAttemptAsync(string deviceId);
        Task ResetUserLoginAttemptsAsync(string userId);
        Task<string> GetRefreshToken(string userId,string deviceId);
        Task SetRefreshToken(string userId, string deviceId,string refreshToken);
        //Cache İşlemleri
        Task SetAsync<T>(string key, T value);
        Task<T> GetAsync<T>(string key);
        Task RemoveAsync(string key);

    }
}
