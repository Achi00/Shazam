using Mapster;
using Microsoft.EntityFrameworkCore;
using Shazam.Application.DTOs.Song;
using Shazam.Application.Interfaces.Repository;
using Shazam.Domain.Entity;
using Shazam.Persistence.Context;

namespace Shazam.Infrastructure.Repositories
{
    public class SongRepository : ISongRepository
    {
        private readonly ShazamContext _context;

        public SongRepository(ShazamContext context)
        {
            _context = context;
        }
        public void AddSong(Song song)
        {
            _context.Songs.Add(song);
        }

        public async Task<List<SongResponse>> GetAllSongs(CancellationToken ct = default)
        {
            return await _context.Songs.AsNoTracking().ProjectToType<SongResponse>().ToListAsync(ct);
        }

        public async Task<Song?> GetForUpdateAsync(int id, CancellationToken ct = default)
        {
            return await _context.Songs.FirstOrDefaultAsync(s => s.Id == id, ct);
        }

        public async Task<Song?> GetSongById(int id, CancellationToken ct = default)
        {
            return await _context.Songs.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, ct);
        }

        public async Task<bool> ExistsByYoutubeUrlAsync(string url, CancellationToken ct = default)
        {
            return await _context.Songs.AsNoTracking().AnyAsync(s => s.YoutubeUrl == url, ct);
        }

        public void RemoveSong(Song song)
        {
            _context.Songs.Remove(song);
        }
    }
}
