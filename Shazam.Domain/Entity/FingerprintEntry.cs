namespace Shazam.Domain.Entity
{
    public sealed class FingerprintEntry
    {
        public int SongId { get; set; }
        // where in the song this hash appares
        public int TimeOffsetFrame { get; set; }
    }
}
