using Shazam.Application.DTOs.Song;

namespace Shazam.Application.Interfaces.Service.Match
{
    public interface ISongMatchingService
    {
        Task<SongResponse?> FindMatchAsync(Dictionary<string, int> queryHashes, CancellationToken ct = default);
    }
}
