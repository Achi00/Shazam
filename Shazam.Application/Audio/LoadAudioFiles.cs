using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Shazam.Application.Audio
{
    public class LoadAudioFiles
    {
        public float[] ProcessAudioSample(string path)
        {
            // mp3, wav
            using var reader = new AudioFileReader(path);

            ISampleProvider provider = reader;

            // convert to mono, only one audio channel, no sense of space and direction in audio needed
            if (reader.WaveFormat.Channels == 2)
            {
                provider = new StereoToMonoSampleProvider(provider)
                {
                    LeftVolume = 0.5f,
                    RightVolume = 0.5f
                };
            }

            // // resample, keep all in consistant Hz
            int targetSampleRate = 16000;
            if (provider.WaveFormat.SampleRate != targetSampleRate)
            {
                provider = new WdlResamplingSampleProvider(provider, targetSampleRate);
            }

            // read all samples into memory
            // find max peak
            // TESTING
            // TODO: change list float, avoid list growth memory allocation and GC pressure
            // instead of list, chunk processing? read - process - discard????
            List<float> samples = new List<float>();
            float[] buffer = new float[4096];

            int read;
            float max = 0;

            while ((read = provider.Read(buffer, 0, buffer.Length)) > 0)
            {
                for (int i = 0; i < read; i++)
                {
                    float sample = buffer[i];
                    samples.Add(sample);

                    float abs = Math.Abs(sample);
                    if (abs > max)
                    {
                        max = abs;
                    }
                }
            }

            // normalize
            if (max > 0)
            {
                float scale = 1.0f / max;

                for (int i = 0; i < samples.Count; i++)
                {
                    samples[i] *= scale;
                }
            }

            return samples.ToArray();

        }
    }
}
