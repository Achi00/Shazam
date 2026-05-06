using Shazam.Application.DTOs.Song;

namespace Shazam.Application.Interfaces.Service.Song
{
    public interface ISongService
    {
        Task<List<SongResponse>> GetAllAsync(CancellationToken ct = default);
        Task<IEnumerable<SongResponse>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<SongResponse> AddSongAsync(AddSongRequest dto, CancellationToken ct = default);
        Task RemoveSongAsync(int id, CancellationToken ct = default);
        Task UpdateSongAsync(int id, CancellationToken ct = default);
    }
}
