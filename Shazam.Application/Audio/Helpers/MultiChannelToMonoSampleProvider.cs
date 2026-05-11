using NAudio.Wave;

namespace Shazam.Application.Audio.Helpers
{
    // used to transfer multi channel to single chanel audio, in redis storage hashes are generated from single channel audios
    public sealed class MultiChannelToMonoSampleProvider : ISampleProvider
    {
        private readonly ISampleProvider _source;
        private readonly int _channels;
        private float[] _sourceBuffer = [];

        public WaveFormat WaveFormat { get; }

        public MultiChannelToMonoSampleProvider(ISampleProvider source)
        {
            _source = source;
            _channels = source.WaveFormat.Channels;
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(source.WaveFormat.SampleRate, 1);
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int sourceSamples = count * _channels;
            if (_sourceBuffer.Length < sourceSamples)
            {
                _sourceBuffer = new float[sourceSamples];
            }

            int read = _source.Read(_sourceBuffer, 0, sourceSamples);
            int monoSamples = read / _channels;

            for (int i = 0; i < monoSamples; i++)
            {
                float sum = 0;
                for (int ch = 0; ch < _channels; ch++)
                {
                    sum += _sourceBuffer[i * _channels + ch];
                }
                buffer[offset + i] = sum / _channels;
            }

            return monoSamples;
        }
    }
}
