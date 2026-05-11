using Shazam.UI.Record;

var recordAudio = new AudioRecord();

Console.WriteLine("Press any button to start audio recording...");
Console.ReadLine();

recordAudio.StartRecording("newAudio.wav");

Console.WriteLine("• Recording...");

Console.WriteLine("Press any button to stop audio recording");
Console.ReadLine();

recordAudio.StopRecording();
