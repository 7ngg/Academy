using AuthService.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace AuthService.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _cacheDb;

        public CacheService(IConfiguration configuration)
        {
            var redis = ConnectionMultiplexer.Connect(configuration.GetSection("redis").Value);
            _cacheDb = redis.GetDatabase();
        }

        public async Task<T> Get<T>(string key)
        {
            var value = await _cacheDb.StringGetAsync(key);
        
            if (!value.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<T>(value);
            }

            return default;
        }

        public async Task<bool> Set<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expires = expirationTime.DateTime.Subtract(DateTime.UtcNow);
            var data = JsonSerializer.Serialize(value);

            return await _cacheDb.StringSetAsync(key, data, expires);
        }
    }
}
