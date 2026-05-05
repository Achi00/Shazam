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
        public void AddSong(Song song, CancellationToken ct = default)
        {
            _context.Songs.Add(song);
        }

        public async Task<List<Song>> GetAllSongs(CancellationToken ct = default)
        {
            return await _context.Songs.AsNoTracking().ToListAsync(ct);
        }

        public Task<Song?> GetForUpdateAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Song> GetSongById(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public void RemoveSong(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Song> UpdateSong(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
