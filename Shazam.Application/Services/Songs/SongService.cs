using Mapster;
using Microsoft.EntityFrameworkCore;
using Shazam.Application.DTOs.Song;
using Shazam.Application.Exceptions;
using Shazam.Application.Interfaces;
using Shazam.Application.Interfaces.Repository;
using Shazam.Application.Interfaces.Service.Song;
using Shazam.Domain.Entity;

namespace Shazam.Application.Services.Songs
{
    public class SongService : ISongService
    {
        private readonly ISongRepository _songRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SongService(ISongRepository songRepository, IUnitOfWork unitOfWork)
        {
            _songRepository = songRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<SongResponse> AddSongAsync(AddSongRequest dto, CancellationToken ct = default)
        {
            var song = dto.Adapt<Song>();

            var exists = await _songRepository.ExistsByYoutubeUrlAsync(song.YoutubeUrl, ct);

            if (exists)
            {
                throw new AlreadyExistsException($"Song with Url: {song.YoutubeUrl} already added in database");
            }

            _songRepository.AddSong(song);

            await _unitOfWork.SaveChangesAsync(ct);

            return song.Adapt<SongResponse>();
        }

        public async Task<List<SongResponse>> GetAllAsync(CancellationToken ct = default)
        {
            return await _songRepository.GetAllSongs(ct);
        }

        public Task<IEnumerable<SongResponse>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task RemoveSongAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateSongAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
