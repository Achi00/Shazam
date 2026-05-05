using Shazam.Application.Interfaces.Repository;
using Shazam.Application.Interfaces.Services;
using Shazam.Domain.Entity;

namespace Shazam.Infrastructure.Repositories
{
    public sealed class RedisFingerprintRepository : IFingerprintRepository
    {
        private readonly ICacheService _cacheService;
        // redis data prefix
        private const string prefix = "fingerprint:";

        public RedisFingerprintRepository(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }
        public async Task<List<FingerprintEntry>> GetCandidatesAsync(string hash)
        {
            return await _cacheService.GetAsync<List<FingerprintEntry>>(prefix + hash) ?? [];
        }

        public async Task StoreHashesAsync(int songId, IReadOnlyDictionary<string, int> hashToOffset)
        {
            foreach (var (hash, offsetMs) in hashToOffset)
            {
                var key = prefix + hash;
                var existing = await _cacheService.GetAsync<List<FingerprintEntry>>(key);
                existing.Add(new FingerprintEntry { SongId = songId, TimeOffsetMs = offsetMs });

                await _cacheService.SetAsync(key, existing);
            }
        }
    }
}
