using NAudio.Dsp;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace Shazam.Application.Audio
{
    public class STFT
    {
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
                for (int i = 0; i < fftSize; i++)
                {
                    float real = buffer[i].X;
                    float imaginary = buffer[i].Y;
                    spectrogram[frame, i] = MathF.Sqrt(real * real + imaginary * imaginary);
                }
            }

            return spectrogram;
        }

        private float[] ComputeHannWindow(int fftSize)
        {
            float[] window = new float[fftSize];
            for (int n = 0; n < fftSize; n++)
            {
                window[n] = 0.5 * (1 - MathF.Cos(2 * MathF.PI * n / (fftSize - 1)));
            }
            return window;
        }

        private void ProcessSTFT(double[] signal, int windowSize, int hopSize)
        {

        }
    }
}
