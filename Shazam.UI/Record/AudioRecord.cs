using NAudio.Wave;
using Shazam.Application.Audio.Spectogram;
using Shazam.Application.Peaks;
using System.Net.Http.Json;

namespace Shazam.UI.Record
{
    // instead of record audio file and generate fingerpring based on that we fingerprint on client and send hashes chunk by chunk
    public class AudioRecord
    {
        private WasapiLoopbackCapture _capture;
        private readonly List<byte> _accumulated = new();
        private readonly HttpClient _http = new();
        // sending chunks to generate fingerptint
        private const int ChunkDurationSeconds = 3;

        private readonly TaskCompletionSource<string?> _result = new();

        public void StartRecording(CancellationToken ct = default)
        {
            ct.Register(() =>
            {
                StopRecording();
                _result.TrySetResult(null);
            });

            _capture = new WasapiLoopbackCapture();
            Console.WriteLine($"Capture: {_capture.WaveFormat.SampleRate}Hz {_capture.WaveFormat.Channels}ch {_capture.WaveFormat.BitsPerSample}bit");
            int bytesPerSecond = _capture.WaveFormat.AverageBytesPerSecond;
            int chunkSize = bytesPerSecond * ChunkDurationSeconds;

            _capture.DataAvailable += async (s, e) =>
            {
                _accumulated.AddRange(e.Buffer[..e.BytesRecorded]);
                if (_accumulated.Count >= chunkSize)
                {
                    var samples = ConvertToSamples(_accumulated.ToArray(), _capture.WaveFormat);
                    _accumulated.Clear();

                    var hashes = GenerateHashes(samples);

                    if (hashes.Count == 0) return;

                    Console.WriteLine($"Sending {hashes.Count} hashes...");
                    var match = await SendHashesAsync(hashes);

                    if (match != null)
                    {
                        Console.WriteLine($"Found: {match}");
                        StopRecording();
                        _result.TrySetResult(match);
                    }
                    else
                    {
                        Console.WriteLine("No match yet, still listening...");
                    }
                }
            };

            _capture.RecordingStopped += (s, e) => _result.TrySetResult(null);

            _capture.StartRecording();
        }

        public Task<string?> WaitForResultAsync()
        {
            return _result.Task;
        }

        public void StopRecording()
        {
            _capture?.StopRecording();
        }

        private float[] ConvertToSamples(byte[] buffer, WaveFormat format)
        {
            var samples = new float[buffer.Length / 4];
            for (int i = 0; i < samples.Length; i++)
                samples[i] = BitConverter.ToSingle(buffer, i * 4);
            return samples;
        }

        private Dictionary<string, int> GenerateHashes(float[] samples)
        {
            var spectrogram = new STFT().ComputeSpectrogram(samples);
            var peaks = new PeakDetection().FindPeaks(spectrogram);
            var fingerprints = new PeakPairing().Pear(peaks);

            return fingerprints
                .GroupBy(f => f.Hash.ToString("X8"))
                .ToDictionary(g => g.Key, g => g.First().AnchorTime);
        }

        private async Task<string?> SendHashesAsync(Dictionary<string, int> hashes)
        {
            var response = await _http.PostAsJsonAsync("https://localhost:7072/api/recognize", hashes);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadAsStringAsync();
        }
    }
}
