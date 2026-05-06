using Shazam.Domain.Entity;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;

namespace Shazam.Application.Services
{
    // is part of data collecting pipeline
    public class ProcessYoutube
    {
        private static readonly YoutubeClient _youtubeClient = new YoutubeClient();

        public async Task DownloadStreamAsync(IStreamInfo streamInfo, string filePath)
        {
            // get dir name
            var directory = Path.GetDirectoryName(filePath);
            // create dir if it does not exists
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            Console.WriteLine($"Downloading audio file at: {directory}");

            await _youtubeClient.Videos.Streams.DownloadAsync(streamInfo, filePath);
        }

        public async Task<(Song song, IStreamInfo audioStream)> GetMetaDataAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("Youtube video url is required");
            }

            Console.WriteLine("Collectiong songs metadata");
            // get video metadata
            var video = await _youtubeClient.Videos.GetAsync(url);

            // get streams
            var streamManifest = await _youtubeClient.Videos.Streams.GetManifestAsync(url);
            var audioStream = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

            var song = new Song();

            song.Title = video.Title;
            song.Author = video.Author.ToString();
            song.Duration = video.Duration;
            song.ThumbnailUrl = video.Thumbnails.TryGetWithHighestResolution().Url;

            return (song, audioStream);
        }
    }
}
