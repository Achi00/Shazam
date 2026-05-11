using NAudio.Wave;

namespace Shazam.UI.Record
{
    public static class AudioRecord
    {
        public static void RecordAudio()
        {
            var waveIn = new WaveInEvent();
            waveIn.WaveFormat = new WaveFormat(44100, 1);

            var writer = new WaveFileWriter("recording.wav", waveIn.WaveFormat);

            // incoming data
            waveIn.DataAvailable += (s, e) =>
            {
                writer.Write(e.Buffer, 0, e.BytesRecorded);
            };

            // stop recording
            waveIn.RecordingStopped += (s, e) =>
            {
                waveIn.Dispose();
            };

            waveIn.StartRecording();
            Console.WriteLine("• Recording audio");
            Console.WriteLine("Press any button to stop");
            Console.ReadLine();
            waveIn.StopRecording();
            waveIn.Dispose();
        }
    }
}
