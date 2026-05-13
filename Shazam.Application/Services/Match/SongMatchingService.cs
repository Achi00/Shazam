using Mapster;
using Shazam.Application.DTOs.Song;
using Shazam.Application.Interfaces.Repository;
using Shazam.Application.Interfaces.Service.Match;
using Shazam.Domain.Entity;
using System.Text.RegularExpressions;

namespace Shazam.Application.Services.Match
{
    public class SongMatchingService : ISongMatchingService
    {
        private readonly IFingerprintRepository _fingerprintRepository;
        private readonly ISongRepository _songRepository;
        // min consistent hits to count as matching
        private const int ConfidenceThreshold = 5;

        public SongMatchingService(IFingerprintRepository fingerprintRepository, ISongRepository songRepository)
        {
            _fingerprintRepository = fingerprintRepository;
            _songRepository = songRepository;
        }
        public async Task<SongResponse?> FindMatchAsync(Dictionary<string, int> queryHashes, CancellationToken ct = default)
        {
            var votes = new Dictionary<(int SongId, int TimeDelta), int>();

            var hitCount = 0;

            foreach (var (hash, queryOffset) in queryHashes)
            {
                var candidates = await _fingerprintRepository.GetCandidatesAsync(hash);
                if (candidates.Count > 0) hitCount++;

                foreach (var entry in candidates)
                {
                    // timeDelta = how far into the original song this snippet sits
                    var deltaTime = entry.TimeOffsetFrame - queryOffset;
                    var key = (entry.SongId, deltaTime);

                    votes.TryGetValue(key, out var count);
                    // increment vote count on hash key if matched
                    votes[key] = count + 1;
                }
            }
            Console.WriteLine($"Hashes with Redis hits: {hitCount} out of {queryHashes.Count}");

            if (votes.Count == 0)
            {
                return null;
            }


            // get single audio with most vote cound
            var winner = votes.MaxBy(v => v.Value);

            if (winner.Value < ConfidenceThreshold)
            {
                // matched audio hashes are not enought to be confident about winner
                return null;
            }

            var matchFrame = Math.Abs(winner.Key.TimeDelta);

            // fetch winner song
            var song = await _songRepository.GetSongById(winner.Key.SongId, ct);

            var matchSec = AudioTime.FrameToSec(matchFrame);

            var response = song.Adapt<SongResponse>();

            if (!string.IsNullOrWhiteSpace(response.YoutubeUrl))
            {
                response.YoutubeUrl += $"&t={matchSec}s";
            }

            response.MatchPositionMs = matchSec;

            return response;
        }
    }
}
