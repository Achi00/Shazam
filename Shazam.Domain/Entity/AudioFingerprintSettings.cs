namespace Shazam.Domain.Entity
{
    public sealed class AudioFingerprintSettings
    {
        public int SampleRate { get; init; } = 16000;

        public int HopSize { get; init; } = 512;

        public int FftSize { get; init; } = 1024;

        public double FrameDurationMs =>
            HopSize * 1000.0 / SampleRate;
    }
}
