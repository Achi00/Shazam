using Mapster;
using Shazam.Application.DTOs.Song;
using Shazam.Application.Exceptions;
using Shazam.Application.Interfaces;
using Shazam.Application.Interfaces.Repository;
using Shazam.Application.Interfaces.Service;
using Shazam.Application.Interfaces.Service.Song;

namespace Shazam.Application.Services.Songs
{
    public class SongService : ISongService
    {
        private readonly ISongRepository _songRepository;
        private readonly IAudioFingerprintService _audioFingerprintService;
        private readonly IFingerprintRepository _fingerprintRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ProcessYoutubeService _youtubeService;

        public SongService(ISongRepository songRepository, IAudioFingerprintService audioFingerprintService, IFingerprintRepository fingerprintRepository, IUnitOfWork unitOfWork, ProcessYoutubeService youtubeService)
        {
            _songRepository = songRepository;
            _audioFingerprintService = audioFingerprintService;
            _unitOfWork = unitOfWork;
            _youtubeService = youtubeService;
            _fingerprintRepository = fingerprintRepository;
        }
        // TODO: first store metadata only in database, next fingerprint
        // TODO: add rollback logic
        public async Task<SongResponse> AddSongAsync(string url, CancellationToken ct = default)
        {
            string? fileName = null;
            try
            {
                //TODO: add sql transaction and rollbacks on fail
                // get add data from youtube url
                var (song, streamInfo) = await _youtubeService.GetMetaDataAsync(url, ct);

                // duplicate check
                var exists = await _songRepository.ExistsByYoutubeUrlAsync(song.YoutubeUrl, ct);

                if (exists)
                {
                    throw new AlreadyExistsException($"Song with Url: {song.YoutubeUrl} already added in database");
                }

                // download file
                // TODO: fix path later
                fileName = $"/audio/{Guid.NewGuid()}.{streamInfo.Container}";

                /// TODO: remove audio later, clean up!!!
                await _youtubeService.DownloadStreamAsync(streamInfo, fileName);

                // generate fingerprint hashes

                var hashes = await _audioFingerprintService.GenerateHashesAsync(fileName);

                // start sql transaction
                await _unitOfWork.BeginTransactionAsync(ct);

                _songRepository.AddSong(song);
                await _unitOfWork.SaveChangesAsync(ct);

                // sotore in redis
                await _fingerprintRepository.StoreHashesAsync(song.Id, hashes);

                // commit sql
                await _unitOfWork.CommitTransactionAsync(ct);

                return song.Adapt<SongResponse>();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(ct);

                // clean up file
                if (fileName != null && File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                throw;
            }
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
