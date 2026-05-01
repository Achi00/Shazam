namespace Shazam.Domain.Entity
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string YoutubeUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
    }
}
