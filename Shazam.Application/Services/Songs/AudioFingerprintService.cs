using Shazam.Application.Audio;
using Shazam.Application.Audio.Spectogram;
using Shazam.Application.Interfaces.Service;
using Shazam.Application.Peaks;

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

            var hashes = fingerPrints.GroupBy(f => f.Hash.ToString("X8")).ToDictionary(g => g.Key, g => g.First().AnchorTime);

            // test
            Console.WriteLine(hashes.FirstOrDefault());

            return Task.FromResult(hashes);
        }
    }
}
