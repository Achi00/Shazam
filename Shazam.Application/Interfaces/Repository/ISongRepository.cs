using Shazam.Application.DTOs.Song;
using Shazam.Domain.Entity;

namespace Shazam.Application.Interfaces.Repository
{
    public interface ISongRepository
    {
        Task<Song> GetSongById(int id, CancellationToken ct = default);
        Task<List<Song>> GetAllSongs(CancellationToken ct = default);
        void AddSong(Song song, CancellationToken ct = default);
        void RemoveSong(int id, CancellationToken ct = default);
        Task<Song?> GetForUpdateAsync(int id, CancellationToken ct = default);
    }
}
