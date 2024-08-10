using Newtonsoft.Json;
using StackExchange.Redis;

namespace src.Services;

public class RedisService
{
    private readonly IConnectionMultiplexer _redis;

    public RedisService(IConnectionMultiplexer connectionMultiplexer)
    {
        _redis = connectionMultiplexer;
    }

    public async Task SetCacheValueAsync(string key, string value, TimeSpan? expiration = null)
    {
        var db = _redis.GetDatabase();
        await db.StringSetAsync(key, value, expiration);
    }

    public async Task<T?> GetCacheValueAsync<T>(string key)
    {
        var db = _redis.GetDatabase();
        var jsonString = await db.StringGetAsync(key);

        if (string.IsNullOrEmpty(jsonString))
            return default;

        var value = JsonConvert.DeserializeObject<T>(jsonString);
        return value;
    }
}
