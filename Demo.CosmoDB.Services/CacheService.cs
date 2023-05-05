using Demo.CosmoDB.Services.Abstraction;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Demo.CosmoDB.Services
{
    public class CacheService<T> : ICacheService<T>
    {
        private readonly IDistributedCache _cache;
        private readonly string _regionName;
        private readonly TimeSpan _absoluteExpiration;
        public CacheService(TimeSpan absoluteExpiration, IDistributedCache distributedCache)
        {
            _cache = distributedCache;
            _regionName = typeof(T).FullName;
            _absoluteExpiration = absoluteExpiration;
        }

        public T GetOrAdd(string key, Func<T> retriever)
        {
            T item;
            var serializedItem = _cache.GetString(GenerateRegionedKey(key, _regionName));
            if (string.IsNullOrWhiteSpace(serializedItem))
            {
                item = retriever();
                if (item != null)
                {
                    _cache.SetString(GenerateRegionedKey(key, _regionName), JsonConvert.SerializeObject(item, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }), GenerateCacheItemPolicy());
                }
            }
            else
            {
                item = JsonConvert.DeserializeObject<T>(serializedItem);
            }
            return item;
        }
        public async Task<T> GetOrAddAsync(string key, Func<Task<T>> retriever)
        {
            T item;
            var serializedItem = await _cache.GetStringAsync(GenerateRegionedKey(key, _regionName));
            if (string.IsNullOrWhiteSpace(serializedItem))
            {
                item = await retriever();
                if (item != null)
                {
                    await _cache.SetStringAsync(GenerateRegionedKey(key, _regionName), JsonConvert.SerializeObject(item, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }), GenerateCacheItemPolicy());
                }

            }
            else
            {
                item = JsonConvert.DeserializeObject<T>(serializedItem);
            }
            return item;
        }

        public async Task RemoveAsync(string key, Func<Task> retriever)
        {
            _cache.Remove(GenerateRegionedKey(key, _regionName));
            await retriever();
        }
        private string GenerateRegionedKey(string key, string region)
        {
            return $"{region}_{key}";
        }

        private DistributedCacheEntryOptions GenerateCacheItemPolicy()
        {
            return new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddTicks(_absoluteExpiration.Ticks)
            };
        }
    }
}
