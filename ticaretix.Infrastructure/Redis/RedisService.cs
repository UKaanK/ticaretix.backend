using StackExchange.Redis;
using ticaretix.Core.Interfaces;

namespace ticaretix.Infrastructure.Redis
{
    public class RedisService : IRedisService
    {
        private readonly IDatabase _database;

        public RedisService()
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            _database = redis.GetDatabase();
        }

        public void SetUserToken(string userId, string token)
        {
            _database.StringSet($"user_token:{userId}", token, TimeSpan.FromMinutes(30)); // Token 30 dk geçerli
        }

        public string GetUserToken(string userId)
        {
            return _database.StringGet($"user_token:{userId}");
        }

        public void RemoveUserToken(string userId)
        {
            _database.KeyDelete($"user_token:{userId}");
        }
    }
}
