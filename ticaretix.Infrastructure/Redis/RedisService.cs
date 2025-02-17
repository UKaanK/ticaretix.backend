using StackExchange.Redis;
using System.Text.Json;
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
        public async Task SetUserToken(string userId, string deviceId, string token)
        {
            // 1️⃣ Kullanıcı ve device'a ait eski token'ı al
            string oldToken = await _database.StringGetAsync($"user_token:{userId}:{deviceId}");

            if (!string.IsNullOrEmpty(oldToken))
            {
                // 2️⃣ Eğer eski token varsa, onu sil
                await _database.KeyDeleteAsync($"token_user:{oldToken}");
                await _database.KeyDeleteAsync($"user_token:{userId}:{deviceId}");
            }

            // 3️⃣ Yeni token'ı kaydet
            await _database.StringSetAsync($"user_token:{userId}:{deviceId}", token, TimeSpan.FromMinutes(30));
            await _database.StringSetAsync($"token_user:{token}", $"{userId}:{deviceId}", TimeSpan.FromMinutes(30));
        }

        public async Task IncrementUserLoginAttemptAsync(string userId)
        {
            await _database.StringIncrementAsync($"login_attempts:user:{userId}");
            await _database.KeyExpireAsync($"login_attempts:user:{userId}", TimeSpan.FromMinutes(5)); // 5 dk sonra sıfırlansın
        }

        public async Task<bool> IsUserRateLimitedAsync(string userId)
        {
            var attempts = await _database.StringGetAsync($"login_attempts:user:{userId}");
            return attempts != RedisValue.Null && int.Parse(attempts) >= 5;
        }
        public async Task IncrementDeviceLoginAttemptAsync(string deviceId)
        {
            await _database.StringIncrementAsync($"login_attempts:device:{deviceId}");
            await _database.KeyExpireAsync($"login_attempts:device:{deviceId}", TimeSpan.FromMinutes(5)); // 5 dk sonra sıfırlansın
        }
        public async Task ResetUserLoginAttemptsAsync(string userId)
        {
            await _database.KeyDeleteAsync($"login_attempts:user:{userId}");
        }

        public async Task ResetDeviceLoginAttemptsAsync(string deviceId)
        {
            await _database.KeyDeleteAsync($"login_attempts:device:{deviceId}");
        }

        public async Task<bool> IsRateLimitedAsync(string key)
        {
            var attempts = await _database.StringGetAsync(key);
            return attempts != RedisValue.Null && int.Parse(attempts) >= 5;  // 5 deneme limitini belirliyoruz
        }

        public async Task<bool> IsDeviceRateLimitedAsync(string deviceId)
        {
            var attempts = await _database.StringGetAsync($"login_attempts:device:{deviceId}");
            return attempts != RedisValue.Null && int.Parse(attempts) >= 5;
        }
        public async Task<string> GetUserIdByRefreshTokenAsync(string refreshToken, string deviceId)
        {
            // Redis'te refresh token ile kullanıcı id'sini arıyoruz
            var userId = await _database.StringGetAsync($"refresh_token:{refreshToken}:{deviceId}");

            if (userId.IsNullOrEmpty)
            {
                return null; // Eğer kullanıcı id'si bulunamazsa null döner
            }

            return userId;
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

        public async Task<string> GetUserToken(string userId, string deviceId)
        {
            return await _database.StringGetAsync($"user_token:{userId}:{deviceId}");
        }

        public async Task SetRefreshToken(string userId,string deviceId, string refreshToken)
        {
            // Refresh token'ı veritabanında sakla (örn. Redis'te)
            await _database.StringSetAsync($"refresh_token:{userId}:{deviceId}", refreshToken, TimeSpan.FromDays(7));  // 7 gün geçerli
        }

        public async Task<string> GetRefreshToken(string userId,string deviceId)
        {
            return await _database.StringGetAsync($"refresh_token:{userId}:{deviceId}");
        }

        public async Task RemoveUserToken(string token)
        {
            // 1️⃣ Token'e bağlı userId ve deviceId'yi al
            string userDeviceInfo = await _database.StringGetAsync($"token_user:{token}");

            if (!string.IsNullOrEmpty(userDeviceInfo))
            {
                var parts = userDeviceInfo.Split(':');
                string userId = parts[0];
                string deviceId = parts[1];

                // 2️⃣ Kullanıcı ve cihaz ID ile kaydedilen token'ı al ve sil
                await _database.KeyDeleteAsync($"user_token:{userId}:{deviceId}");
                await _database.KeyDeleteAsync($"token_user:{token}");

                // 3️⃣ Refresh token'ı da sil
                await _database.KeyDeleteAsync($"refresh_token:{userId}:{deviceId}");
            }
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

        public async Task SetAsync<T>(string key, T value)
        {
            // Nesneyi JSON formatına serileştirme
            var serializedValue = JsonSerializer.Serialize(value, new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // Escape karakterlerini düzenler
                WriteIndented = true
            });

            await _database.StringSetAsync(key, serializedValue);
        }

        public async Task<T> GetAsync<T>(string key)
        {

            var value = await _database.StringGetAsync(key);
            if (value.IsNullOrEmpty) return default;

            // JSON string'ini nesneye dönüştürme
            return JsonSerializer.Deserialize<T>(value);
        }

        public async Task RemoveAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }
    }
}
