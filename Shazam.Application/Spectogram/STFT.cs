using NAudio.Dsp;

namespace Shazam.Application.Spectogram
{
    public class STFT
    {
        // as default 1 frame on every ~30 ms
        // each frame has 512 frequency between 0-8000 Hz
        // each bin 8000 / 512 = 15.6 Hz
        public float[,] ComputeSpectrogram(float[] samples, int fftSize = 1024, int hopSize = 512)
        {
            int frameCount = (samples.Length - fftSize) / hopSize + 1;
            // only positive frequencies
            float[,] spectrogram = new float[frameCount, fftSize / 2]; 

            var hannWindow = ComputeHannWindow(fftSize);

            for (int frame = 0; frame < frameCount; frame++)
            {
                int offset = frame * hopSize;
                float[] windowed = new float[fftSize];

                // apply Hann window
                for (int i = 0; i < fftSize; i++)
                {
                    windowed[i] = samples[offset + i] * hannWindow[i];
                }

                // put windowed float[] into NAudio Complex[] to generate FFT
                var buffer = new Complex[fftSize];

                for (int i = 0; i < fftSize; i++)
                {
                    // real signal
                    buffer[i].X = windowed[i];
                    // imaginary input
                    buffer[i].Y = 0f;
                }
                // FFT
                // m = log2(fftSize)
                int m = (int)Math.Log2(fftSize);
                FastFourierTransform.FFT(true, m, buffer);

                // extract magnitudes for positive frequencies only
                for (int i = 0; i < fftSize / 2; i++)
                {
                    float real = buffer[i].X;
                    float imaginary = buffer[i].Y;
                    // calulate magnitude and convert indo db
                    float magnitude = MathF.Sqrt(real * real + imaginary * imaginary);
                    // avoid -infinity
                    spectrogram[frame, i] = 20f * MathF.Log10(magnitude + 1e-10f);
                }
            }

            return spectrogram;
        }

        private float[] ComputeHannWindow(int fftSize)
        {
            float[] window = new float[fftSize];
            for (int n = 0; n < fftSize; n++)
            {
                window[n] = 0.5f * (1 - MathF.Cos(2 * MathF.PI * n / (fftSize - 1)));
            }
            return window;
        }
    }
}
