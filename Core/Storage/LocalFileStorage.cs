using Serilog;
using Serilog.Localization;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OpenBMCLAPI_IN.Core.Storage;

public class LocalFileStorage : IStorage
{
    private const int MaxRetries = 3;
    private readonly string _rootPath;
    private readonly string _pathPrefix;

    public LocalFileStorage(string baseUri, string path)
    {
        _rootPath = baseUri;
        _pathPrefix = path;
        Directory.CreateDirectory(GetFullPath(""));
    }

    public LocalFileStorage(StorageParameter param)
    {
        _rootPath = new Uri(param.baseUri).LocalPath;
        _pathPrefix = param.path.TrimEnd('/') + '/';
        Directory.CreateDirectory(GetFullPath(""));
    }

    private string GetFullPath(string path) => Path.Combine(_rootPath, _pathPrefix, path);

    public async Task UploadAsync(string path, Stream content)
    {
        int retryCount = 0;
        var fullPath = GetFullPath(path);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

        while (true)
        {
            try
            {
                using var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 81920, FileOptions.Asynchronous);
                if (content.CanSeek)
                    content.Position = 0;
                await content.CopyToAsync(fileStream);
                Log.Logger.InformationL("upload_success", fullPath);
                break;
            }
            catch (Exception ex) when (retryCount < MaxRetries)
            {
                retryCount++;
                await Task.Delay(1000 * retryCount);
                content.Position = 0;
            }
            catch (Exception ex)
            {
                throw new IOException($"上传失败，重试 {retryCount} 次: {ex.Message}", ex);
            }
        }
    }

    public Task<bool> ExistsAsync(string path) =>
        Task.FromResult(File.Exists(GetFullPath(path)));

    public Task<long> GetSizeAsync(string path) =>
        Task.FromResult(new FileInfo(GetFullPath(path)).Length);

    public async Task<string> GetSha1Async(string path)
    {
        using var stream = File.OpenRead(GetFullPath(path));
        return await CalculateSha1Async(stream);
    }

    public async Task<SizeVerificationResult> VerifySizeAsync(string path, long expectedSize)
    {
        if (!await ExistsAsync(path))
            return SizeVerificationResult.NotFound;

        var actualSize = await GetSizeAsync(path);
        return actualSize == expectedSize
            ? SizeVerificationResult.Match
            : SizeVerificationResult.SizeMismatch;
    }

    private static async Task<string> CalculateSha1Async(Stream stream)
    {
        if (stream.CanSeek)
            stream.Position = 0;

        using var sha1 = System.Security.Cryptography.SHA1.Create();
        var buffer = new byte[81920];
        int bytesRead;
        while ((bytesRead = await stream.ReadAsync(buffer)) > 0)
        {
            sha1.TransformBlock(buffer, 0, bytesRead, null, 0);
        }
        sha1.TransformFinalBlock(buffer, 0, 0);
        return BitConverter.ToString(sha1.Hash).Replace("-", "").ToLowerInvariant();
    }
}