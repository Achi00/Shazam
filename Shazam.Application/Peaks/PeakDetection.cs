namespace Shazam.Application.Peaks
{
    public class PeakDetection
    {
        public List<(int time, int freq)> FindPeaks(float[,] spectrogram)
        {
            // TODO: tune it later
            int timeRadius = 5;
            int freqRadius = 10;
            float threshold = -60f;

            int frameCount = spectrogram.GetLength(0);
            int binCount = spectrogram.GetLength(1);

            var peaks = new List<(int, int)>();
            // goes front in music play
            for (int t = timeRadius; t < frameCount - timeRadius; t++)
            {
                // local max peak
                for (int f = freqRadius; f < binCount - freqRadius; f++)
                {
                    float current = spectrogram[t, f];

                    if (current < threshold)
                    {
                        continue;
                    }

                    bool isPeak = true;

                    // find neighbor frames
                    for (int dt = -timeRadius; dt <= timeRadius && isPeak; dt++)
                    {
                        for (int df = -freqRadius; df <= freqRadius; df++)
                        {
                            if (dt == 0 && df == 0)
                            {
                                continue;
                            }

                            if (spectrogram[t + dt, f + df] > current)
                            {
                                isPeak = false;
                                break;
                            }
                        }
                    }

                    if (isPeak)
                    {
                        peaks.Add((t, f));
                    }
                }
            }
            return peaks;
        }
    }
}
