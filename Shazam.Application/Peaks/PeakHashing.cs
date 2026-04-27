namespace Shazam.Application.Peaks
{
    public class PeakHashing
    {
        public uint CalculateHash(List<Fingerprint> fp)
        {
            foreach (var item in fp)
            {
                uint hashData = (uint)((item.Freq1 << 20) | (item.Freq2 << 10) | item.DeltaTime);
            }

            return 0;
        }
    }
}
