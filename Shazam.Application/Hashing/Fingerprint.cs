namespace Shazam.Application.Hashing
{
    public record Fingerprint(int Freq1, int Freq2, int DeltaTime, int AnchorTime)
    {
        public uint Hash => ((uint)Freq1 << 20) | ((uint)Freq2 << 10) | (uint)DeltaTime;
    }
}
