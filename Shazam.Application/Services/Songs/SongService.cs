using Mapster;
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
        private readonly ProcessYoutubeService _youtubeService;

        public SongService(ISongRepository songRepository, IUnitOfWork unitOfWork, ProcessYoutubeService youtubeService)
        {
            _songRepository = songRepository;
            _unitOfWork = unitOfWork;
            _youtubeService = youtubeService;
        }
        //TODO: first store metadata only in database, next fingerprint
        public async Task<SongResponse> AddSongAsync(string url, CancellationToken ct = default)
        {
            // get add data from youtube url
            var (song, streamInfo) = await _youtubeService.GetMetaDataAsync(url, ct);
            //var song = dto.Adapt<Song>();

            // TODO: fix path later
            string fileName = $"/audio/{Guid.NewGuid()}.{streamInfo.Container}";

            /// TODO: remove audio later, clean up!!!
            // download audio
            await _youtubeService.DownloadStreamAsync(streamInfo, fileName);

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

        public async Task<SongResponse> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var song = await _songRepository.GetSongById(id, ct);

            if (song == null)
            {
                throw new NotFoundException($"Song with id: {id} was not found");
            }

            return song.Adapt<SongResponse>();
        }

        public async Task RemoveSongAsync(int id, CancellationToken ct = default)
        {
            var song = await _songRepository.GetForUpdateAsync(id, ct);

            if (song == null)
            {
                throw new NotFoundException($"Song with id: {id} was not found");
            }

            _songRepository.RemoveSong(song);

            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task UpdateSongAsync(int id, UpdateSongRequest dto, CancellationToken ct = default)
        {
            var song = await _songRepository.GetForUpdateAsync(id, ct);

            if (song == null)
            {
                throw new NotFoundException($"Song with id: {id} was not found");
            }

            dto.Adapt(song);

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
