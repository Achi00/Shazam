using NAudio.Wave;

namespace Shazam.Application.Audio.Helpers
{
    public sealed class RawSampleProvider : ISampleProvider
    {
        private readonly float[] _samples;
        private int _position;
        public WaveFormat WaveFormat { get; }

        public RawSampleProvider(float[] samples, WaveFormat format)
        {
            _samples = samples;
            WaveFormat = format;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int available = Math.Min(count, _samples.Length - _position);
            _samples.AsSpan(_position, available).CopyTo(buffer.AsSpan(offset));
            _position += available;
            return available;
        }
    }
}
