using Shazam.Application.Audio;
using Shazam.Application.Spectogram;

var loader = new LoadAudioFiles();
float[] samples = loader.ProcessAudioSample("D:\\Csharp\\Shazam\\Shazam.Application\\Audio\\audio.mp3");

var stft = new STFT();
float[,] spectrogram = stft.ComputeSpectrogram(samples);

int frameCount = spectrogram.GetLength(0);
int binCount = spectrogram.GetLength(1);

Console.WriteLine($"Samples: {samples.Length}");
Console.WriteLine($"Duration: {samples.Length / 16000f:F2}s");
Console.WriteLine($"Frames: {frameCount}");
Console.WriteLine($"Bins per frame: {binCount}");

// check values are non-zero and reasonable
float max = 0, min = float.MaxValue;
for (int f = 0; f < frameCount; f++)
{
    for (int b = 0; b < binCount; b++)
    {
        max = MathF.Max(max, spectrogram[f, b]);
        min = MathF.Min(min, spectrogram[f, b]);
    }
}

Console.WriteLine($"Magnitude range: {min:F4} - {max:F4}");