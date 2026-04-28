using Shazam.Application.Hashing;

namespace Shazam.Application.Peaks
{
    public class PeakHashing
    {
        public void CalculateHash(List<Fingerprint> fp)
        {
            foreach (var item in fp)
            {
                uint hashData = (uint)((item.Freq1 << 20) | (item.Freq2 << 10) | item.DeltaTime);
            }
        }
    }
}
