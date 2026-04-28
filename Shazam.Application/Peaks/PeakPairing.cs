using Shazam.Application.Hashing;

namespace Shazam.Application.Peaks
{
    public class PeakPairing
    {
        public List<Fingerprint> Pear(List<(int time, int freq)> peaks)
        {
            var fingerprints = new List<Fingerprint>();

            // 1 frame ~ 32ms
            // max delta 1.6sec

            // pairs per anchor
            int pairsPerAnchor = 5;
            // min/max distance (frames)
            int maxDelta = 50;

            var ordered = peaks.OrderBy(p => p.time).ToList();

            for (int i = 0; i < ordered.Count; i++)
            {
                var anchor = ordered[i];

                int pairs = 0;

                for (int j = i + 1; j < ordered.Count && pairs < pairsPerAnchor; j++)
                {
                    var target = ordered[j];
                    // calculate time difference between two
                    var delta = target.time - anchor.time;

                    if (delta <= 0)
                    {
                        continue;
                    }
                    if (delta > maxDelta)
                    {
                        break;
                    }

                    fingerprints.Add(new Fingerprint(anchor.freq, target.freq, delta, anchor.time));

                    pairs++;
                }
            }

            return fingerprints;
        }
    }
}
