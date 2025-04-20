using System.IO;
using System.Threading.Tasks;

namespace OpenBMCLAPI_IN.Core.Storage;

public interface IStorage
{
    Task UploadAsync(string path, Stream content);
    Task<bool> ExistsAsync(string path);
    Task<long> GetSizeAsync(string path);
    Task<string> GetSha1Async(string path);
    Task<SizeVerificationResult> VerifySizeAsync(string path, long expectedSize);
}

public enum SizeVerificationResult
{
    NotFound,
    SizeMismatch,
    Match
}

public static class StorageExtensions
{
    public static async Task CheckMeasureFilesAsync(this IStorage storage)
    {
        int[] sizes = { 10, 20, 30, 40, 50, 100, 200 };
        List<Task> tasks = new List<Task>();
        foreach (var size in sizes)
        {
            string path = $"measures/{size}";
            long expectedSize = size * 1024 * 1024;
            var result = await storage.VerifySizeAsync(path, expectedSize);
            if (result != SizeVerificationResult.Match)
            {
                using var stream = new MemoryStream(new byte[expectedSize]);
                tasks.Add(storage.UploadAsync(path, stream));
            }
        }
        await Task.WhenAll(tasks);
    }
}