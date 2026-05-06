using Shazam.Domain.Entity;

namespace Shazam.Application.Interfaces.Repository
{
    public interface IFingerprintRepository
    {
        Task StoreHashesAsync(int songId, IReadOnlyDictionary<string, int> hashToOffset);
        Task<List<FingerprintEntry>> GetCandidatesAsync(string hash);
    }
}
