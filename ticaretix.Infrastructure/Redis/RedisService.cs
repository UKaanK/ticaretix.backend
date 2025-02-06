using StackExchange.Redis;
using ticaretix.Core.Interfaces;

namespace ticaretix.Infrastructure.Redis
{
    public class RedisService : IRedisService
    {
        private readonly IDatabase _database;
        ConnectionMultiplexer _multiplexer;

        public RedisService(IConnectionMultiplexer connectionMultiplexer)
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");

            _database=redis.GetDatabase();

        }
        public void SetUserToken(string userId, string deviceId, string token)
        {
            // 1️⃣ Önce eski tokenı bul
            string oldToken = _database.StringGet($"user_token:{userId}:{deviceId}");

            if (!string.IsNullOrEmpty(oldToken))
            {
                // 2️⃣ Eğer eski token varsa, onu sil
                _database.KeyDelete($"token_user:{oldToken}");
            }

            // 3️⃣ Yeni tokenı kaydet
            _database.StringSet($"user_token:{userId}:{deviceId}", token, TimeSpan.FromMinutes(30));
            _database.StringSet($"token_user:{token}", $"{userId}:{deviceId}", TimeSpan.FromMinutes(30));
        }

        public async Task<bool> IsRateLimitedAsync(string key)
        {
            var attempts = await _database.StringGetAsync(key);
            return attempts != RedisValue.Null && int.Parse(attempts) >= 5;  // 5 deneme limitini belirliyoruz
        }

        public async Task SetRateLimitAsync(string key, TimeSpan expirationTime)
        {
            var currentAttempts = await _database.StringGetAsync(key);
            int attempts = string.IsNullOrEmpty(currentAttempts) ? 0 : int.Parse(currentAttempts);

            // Eğer limit aşılmadıysa, arttır
            if (attempts < 5)
            {
                await _database.StringIncrementAsync(key);
            }
            else
            {
                // Limit aşılmışsa, rate-limiting işlemi uygulanır.
                await _database.StringSetAsync(key, (attempts + 1).ToString(), expirationTime);
            }
        }

        public async Task ResetLoginAttemptsAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }

        public string GetUserToken(string userId, string deviceId)
        {
            return _database.StringGet($"user_token:{userId}:{deviceId}");
        }

        public void RemoveUserToken(string userId, string deviceId)
        {
            _database.KeyDelete($"user_token:{userId}:{deviceId}");
        }

        public void RemoveAllUserTokens(string userId)
        {
            var server = ConnectionMultiplexer.Connect("localhost:6379").GetServer("localhost", 6379);
            var keys = server.Keys(pattern: $"user_token:{userId}:*").ToArray();
            _database.KeyDelete(keys);
        }

        public string GetUserIdByToken(string token)
        {
            return _database.StringGet($"token_user:{token}");
        }
    
}
}
