using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Currency.Exchange;

public class DistributedCacheStore : ICacheStore
{
    private readonly IDistributedCache _distributedCache;

    public DistributedCacheStore(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }
    
    public void Add<T>(string key, T item, TimeSpan expiration)
    {
        var cache = JsonSerializer.SerializeToUtf8Bytes(item);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };
        
        _distributedCache.SetAsync(key, cache, options);
    }

    public T? Get<T>(string key)
    {
        var cachedObject = _distributedCache.Get(key);
        
        if (cachedObject != null)
        {
            var json = Encoding.UTF8.GetString(cachedObject);
            
            var cachedResult = JsonSerializer.Deserialize<T>(json);
         
            if (cachedResult != null)
            {
                return cachedResult;
            }
        }

        return default;
    }
}