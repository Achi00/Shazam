//using Shazam.Application.Audio;
//using Shazam.Application.Peaks;
//using Shazam.Application.Spectogram;


//// original song
//var loader = new LoadAudioFiles();
////float[] samples = loader.ProcessAudioSample("D:\\Csharp\\Shazam\\Shazam.Application\\Audio\\audio.mp3");
//// cropped version of same song, ~15 seconds
//float[] samplesCrop = loader.ProcessAudioSample("D:\\Csharp\\Shazam\\Shazam.Application\\Audio\\audio-crop.mp3");

//var stft = new STFT();
////float[,] originalSpectrogram = stft.ComputeSpectrogram(samples);
//float[,] cutAudioSpectrogram = stft.ComputeSpectrogram(samplesCrop);

//var pd = new PeakDetection();
////var originalPeaks = pd.FindPeaks(originalSpectrogram);
//// same audio, cut version
//var cutPeaks = pd.FindPeaks(cutAudioSpectrogram);

////Console.WriteLine($"peaks count: {originalPeaks.Count}");
//var peakPearing = new PeakPairing();


////var originalFingerPrints = peakPearing.Pear(originalPeaks);
//var cutFingerPrints = peakPearing.Pear(cutPeaks);

////Console.WriteLine($"fingerPrints count: {originalFingerPrints.Count}");
//Console.WriteLine($"cut audio fingerPrints count: {cutFingerPrints.Count}");

//// songs table for testing
//Dictionary<int, string> songs = new();

//// build indexes
//Dictionary<uint, List<(int songId, int anchorTime)>> index = new();

//// index all songs in folder
//string[] files = Directory.GetFiles("D:\\Csharp\\Shazam\\Shazam.Application\\Audio\\OriginalFiles", "*.mp3");


//foreach (var file in files)
//{
//    int songId = songs.Count;
//    songs[songId] = Path.GetFileNameWithoutExtension(file);

//    Console.WriteLine($"Indexing: {songs[songId]}...");

//    float[] samples = loader.ProcessAudioSample(file);
//    float[,] spectrogram = stft.ComputeSpectrogram(samples);
//    var peaks = pd.FindPeaks(spectrogram);
//    var fingerprints = peakPearing.Pear(peaks);

//    foreach (var fp in fingerprints)
//    {
//        if (!index.TryGetValue(fp.Hash, out var list))
//        {
//            list = new();
//            index[fp.Hash] = list;
//        }
//        list.Add((songId, fp.AnchorTime));
//    }

//    Console.WriteLine($"peaks={peaks.Count} fingerprints={fingerprints.Count}");
//}

//Console.WriteLine($"index count: {index.Count}");

//// cropped audio file, testing sound matching
//// votes: (songId, time offset) -> count
//var votes = new Dictionary<(int songId, int offset), int>();

//foreach (var fp in cutFingerPrints)
//{
//    if (!index.TryGetValue(fp.Hash, out var matches))
//    {
//        continue;
//    }

//    foreach (var (matchedSongId, storedAnchorTime) in matches)
//    {
//        var offset = storedAnchorTime - fp.AnchorTime;
//        var key = (matchedSongId, offset);

//        if (!votes.ContainsKey(key))
//        {
//            votes[key] = 0;
//        }

//        votes[key]++;
//    }
//}

//// find winner
//var best = votes.MaxBy(v => v.Value);

//Console.WriteLine($"Best match: song={songs[best.Key.songId]} votes={best.Value} offset={best.Key.offset}");
//Console.WriteLine($"Total unique (song, offset) pairs: {votes.Count}");

/* Data collecing pipelins */

// ========================= get data from youtube url =========================

using Shazam.Application.Audio;
using Shazam.Application.Peaks;
using Shazam.Application.Services;
using Shazam.Application.Spectogram;

var youtubeService = new ProcessYoutube();

var (song, streamInfo) = await youtubeService.GetMetaDataAsync("https://www.youtube.com/watch?v=zBUx6zTxr98");

Console.WriteLine(song.Title);
Console.WriteLine(song.ThumbnailUrl);
Console.WriteLine(song.Author);
Console.WriteLine(song.Duration);

// testing, random file names
string fileName = $"/audio/{Guid.NewGuid()}.{streamInfo.Container}";

// download audio
await youtubeService.DownloadStreamAsync(streamInfo, fileName);

// load downloaded audio file
var loader = new LoadAudioFiles();
float[] samples = loader.ProcessAudioSample($"D:{fileName}");

// generate spectpgram
var stft = new STFT();
float[,] spectogram = stft.ComputeSpectrogram(samples);

// detect audio peaks
var pd = new PeakDetection();
var peaks = pd.FindPeaks(spectogram);

Console.WriteLine($"Peaks count: {peaks.Count}");

// connect peaks, generate audio fingerprint
var peakPearing = new PeakPairing();
var fingerPrints = peakPearing.Pear(peaks);

Console.WriteLine($"Fingerprints count: {fingerPrints.Count}");