using Shazam.Application.Audio;
using Shazam.Application.Interfaces.Service;
using Shazam.Application.Peaks;
using Shazam.Application.Spectogram;

namespace Shazam.Application.Services.Songs
{
    public class AudioFingerprintService : IAudioFingerprintService
    {
        public Task<Dictionary<string, int>> GenerateHashesAsync(string filePath)
        {
            var loader = new LoadAudioFiles();
            // TODO: fix path later, now for testing!!!
            float[] samples = loader.ProcessAudioSample($"D:{filePath}");

            // generate spectpgram
            float[,] spectrogram = new STFT().ComputeSpectrogram(samples);

            // detect audio peaks
            var peaks = new PeakDetection().FindPeaks(spectrogram);

            // connect peaks, generate audio fingerprint
            var fingerPrints = new PeakPairing().Pear(peaks);

            var hashes = fingerPrints.ToDictionary(f => f.Hash.ToString("X8"), f => f.AnchorTime);

            Console.WriteLine($"Fingerprints count: {fingerPrints.Count}");

            return Task.FromResult(hashes);
        }
    }
}
