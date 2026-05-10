using NAudio.Wave;

namespace Shazam.UI.Record
{
    public class AudioRecord
    {
        public void RecordAudio()
        {
            var waveIn = new WaveInEvent();
            waveIn.WaveFormat = new WaveFormat(44100, 1);

            var writer = new WaveFileWriter("recording.wav", waveIn.WaveFormat);
        }
    }
}
