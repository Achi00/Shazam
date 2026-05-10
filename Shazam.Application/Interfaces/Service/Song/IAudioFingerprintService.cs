namespace Shazam.Application.Interfaces.Service
{
    public interface IAudioFingerprintService
    {
        Task<Dictionary<string, int>> GenerateHashesAsync(string filePath);
    }
}
