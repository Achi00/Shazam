using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace Shazam.Application.Services
{
    // this is part of data collecting pipeline
    public class DownloadYoutubeAudio
    {
        public async Task DownloadAudio(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("Youtube video url is required");
            }
            using var youtubeClinet = new YoutubeClient();

            // get available streams
            var stream = await youtubeClinet.Videos.Streams.GetManifestAsync(url);

            // get audio only stream
            var streamInfo = stream.GetAudioOnlyStreams().GetWithHighestBitrate();

            // download stream
            await youtubeClinet.Videos.Streams.DownloadAsync(streamInfo, $"audio.{streamInfo.Container}");

        }
    }
}
