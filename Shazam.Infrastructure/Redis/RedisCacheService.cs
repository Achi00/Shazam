using Shazam.Application.Interfaces.Caching;

namespace Shazam.Infrastructure.Redis
{
    public class RedisCacheService : ICacheService
    {

        public Task<T?> GetAsync<T>(string key)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            throw new NotImplementedException();
        }
    }
}
