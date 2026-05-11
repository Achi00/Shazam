using Shazam.UI.Record;

var recordAudio = new AudioRecord();

Console.WriteLine("Press any button to start audio recording...");
Console.ReadLine();

//recordAudio.StartRecording("newAudio.wav");

Console.WriteLine("• Recording...");

Console.WriteLine("Press Ctrl + C to stop audio recording");
using var cts = new CancellationTokenSource();

Console.CancelKeyPress += (s, e) =>
{
    e.Cancel = true;
    cts.Cancel();
    recordAudio.StopRecording();
    Console.WriteLine("Stopped.");
};

recordAudio.StartRecording(cts.Token);

// block main thread untill match is found
await recordAudio.WaitForResultAsync();

Console.ReadKey();
