using System.Drawing;

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
                double[] windowed = new double[fftSize];

                // apply Hann window
                for (int i = 0; i < fftSize; i++)
                {
                    windowed[i] = samples[offset + i] * hannWindow[i];
                }

                // FFT
            }

            return spectrogram;
        }

        private double[] ComputeHannWindow(int fftSize)
        {
            double[] window = new double[fftSize];
            for (int n = 0; n < fftSize; n++)
            {
                window[n] = 0.5 * (1 - Math.Cos(2 * Math.PI * n / (fftSize - 1)));
            }
            return window;
        }

        private void ProcessSTFT(double[] signal, int windowSize, int hopSize)
        {

        }
    }
}
