using NAudio.Wave;

namespace Shazam.Application.Audio
{
    public class LoadAudioFiles
    {
        // TODO: seperate processes
        public float[] LoadAudioSample()
        {
            // decode audio file
            // to keep simple at moment only will work on mp3 files
            using var reader = new Mp3FileReader("audio.mp3");
            using var pcmStream = WaveFormatConversionStream.CreatePcmStream(reader);
            WaveFileWriter.CreateWaveFile("output.wav", pcmStream);

            // normalize
            float max = 0;
            
            using var wavReader = new AudioFileReader("output.wav");
            var buffer = new float[wavReader.WaveFormat.SampleRate];

            int read;

            // find max peak
            do
            {
                read = wavReader.Read(buffer, 0, buffer.Length);

                for (int i = 0; i < read; i++)
                {
                    var abs = Math.Abs(buffer[i]);

                    if (abs > max)
                    {
                        max = abs;
                    }
                }
            }
            while (read > 0);

            if (max == 0 || max > 1.0f)
            {
                throw new InvalidOperationException("File can not be normalized");
            }
            // rewind and amplify
            wavReader.Position = 0;
            wavReader.Volume = 1.0f / max;

            // write to new wav file
            WaveFileWriter.CreateWaveFile("output-normalized.wav", wavReader);

            // convert to mono, only one audio channel, no sense of space and direction in audio needed
            const int Khz = 44100;
            // 44.1 khz, 16 bit, 1 channel
            var mono = new WaveFormat(Khz, 1);

            var normalizedWavReader = new WaveFileReader("output-normalized.wav");
            // converting input to 16 bit pcm
            var floatTo16Provider = new WaveFloatTo16Provider(normalizedWavReader);

            using var provider = new WaveFormatConversionProvider(mono, floatTo16Provider);
            // write new mono file (1 channel)
            WaveFileWriter.CreateWaveFile("output-mono.wav", provider);
        }
    }
}
