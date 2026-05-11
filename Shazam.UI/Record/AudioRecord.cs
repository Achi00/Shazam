using NAudio.Wave;

namespace Shazam.UI.Record
{
    public class AudioRecord
    {
        private WasapiLoopbackCapture capture;
        private WaveFileWriter writer;
        private readonly ManualResetEventSlim _stopped = new(false);

        public void StartRecording(string outputFilePath)
        {
            _stopped.Reset();

            // capture from output device/speaker not mic
            capture = new WasapiLoopbackCapture();

            writer = new WaveFileWriter(outputFilePath, capture.WaveFormat);

            capture.DataAvailable += (s, e) =>
            {
                writer.Write(e.Buffer, 0, e.BytesRecorded);
            };

            capture.RecordingStopped += (s, e) =>
            {
                writer?.Dispose();
                writer = null;
                capture.Dispose();
                _stopped.Set();
            };

            capture.StartRecording();
        }

        public void StopRecording()
        {
            capture?.StopRecording();
            // block until capture.RecordingStopped fires and writer is flushed
            _stopped.Wait();
        }
    }
}
