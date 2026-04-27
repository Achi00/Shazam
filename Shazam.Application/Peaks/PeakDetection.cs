namespace Shazam.Application.Peaks
{
    public class PeakDetection
    {
        public List<(int time, int freq)> FindPeaks(List<float[]> spectrogram)
        {
            int timeRadius = 5;
            int freqRadius = 10;
            float threshold = -5f;

            var peaks = new List<(int, int)>();
            // goes front in music play
            for (int t = timeRadius; t < spectrogram.Count - timeRadius; t++)
            {
                var frame = spectrogram[t];
                // local max peak
                for (int f = freqRadius; f < frame.Length; f++)
                {
                    var current = frame[f];

                    if (current < threshold)
                    {
                        continue;
                    }

                    bool isPeak = true;

                    // find neighbor frames
                    for (int dt = -timeRadius; dt <= timeRadius && isPeak; dt++)
                    {
                        var neighborFrame = spectrogram[t + dt];

                        for (int df = -freqRadius; df <= freqRadius; df++)
                        {
                            if (dt == 0 && df == 0)
                            {
                                continue;
                            }

                            if (neighborFrame[f + df] > current)
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
