namespace Shazam.Domain.Entity
{
    public static class AudioTime
    {
        public static double FrameToMs(
            int frame,
            int sampleRate = 16000,
            int hopSize = 512)
        {
            return frame * hopSize * 1000.0 / sampleRate;
        }
        public static double FrameToSec(
            int frame,
            int sampleRate = 16000,
            int hopSize = 512)
        {
            return (frame * hopSize * 1000.0 / sampleRate) / 1000;
        }
    }
}
