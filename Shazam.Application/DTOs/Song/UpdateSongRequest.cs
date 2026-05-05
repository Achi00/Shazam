namespace Shazam.Application.DTOs.Song
{
    public sealed class UpdateSongRequest
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string YoutubeUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public TimeSpan? Duration { get; set; }
    }
}
