using Shazam.Application.DTOs.Song;
using Shazam.Application.Interfaces;
using Shazam.Application.Interfaces.Repository;
using Shazam.Application.Interfaces.Service.Song;

namespace Shazam.Application.Services.Song
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
        public Task<SongResponse> AddSongAsync(AddSongRequest dto, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<SongResponse> GetAllAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
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
