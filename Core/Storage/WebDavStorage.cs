using Serilog;
using Serilog.Localization;
using System.Buffers;
using System.IO;
using System.IO.Hashing;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WebDav;

namespace OpenBMCLAPI_IN.Core.Storage;

public class WebDavStorage : IStorage
{
    private readonly WebDavClient _client;
    private const int MaxRetries = 3;
    private const int TimeoutSeconds = 30;

    private readonly string _pathPrefix;

    public WebDavStorage(string baseUrl, string path, string username, string password)
    {
        _client = new WebDavClient(new WebDavClientParams
        {
            BaseAddress = new Uri(baseUrl),
            Credentials = new System.Net.NetworkCredential(username, password),
            Timeout = Timeout.InfiniteTimeSpan
        });
        _pathPrefix = path.TrimEnd('/') + '/';
    }
    public WebDavStorage(StorageParameter param)
    {
        _client = new WebDavClient(new WebDavClientParams
        {
            BaseAddress = new Uri(param.baseUri),
            Credentials = new System.Net.NetworkCredential(param.userName, param.password),
            Timeout = Timeout.InfiniteTimeSpan
        });
        _pathPrefix = param.path.TrimEnd('/') + '/';
    }

    private string GetFullPath(string path)
    {
        return _pathPrefix + path.TrimStart('/');
    }

    public async Task UploadAsync(string path, Stream content)
    {
        int retryCount = 0;
        while (true)
        {
            try
            {
                using var stream = new MemoryStream();
                if (content.CanSeek)
                    content.Position = 0;
                await content.CopyToAsync(stream);
                stream.Position = 0;

                var response = await _client.PutFile(GetFullPath(path), stream);
                if (!response.IsSuccessful)
                    throw new IOException("Upload failed with status: " + response.StatusCode);
                Log.Logger.InformationL("upload_success",GetFullPath(path));
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
                throw new IOException($"Upload failed after {retryCount} retries: {ex.Message}", ex);
            }
        }
    }

    public async Task<bool> ExistsAsync(string path)
    {
        var response = await _client.Propfind(GetFullPath(path));
        return response.IsSuccessful && response.Resources.First().ContentLength != null;
    }

    public async Task<long> GetSizeAsync(string path)
    {
        var response = await _client.Propfind(GetFullPath(path));
        return response.Resources.First().ContentLength ?? 0;
    }

    public async Task<string> GetSha1Async(string path)
    {
        // TODO: 实现SHA1校验逻辑
        using var stream = await _client.GetRawFile(GetFullPath(path));
        return await CalculateSha1Async(stream.Stream);
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

        using var sha1 = SHA1.Create();
        var buffer = ArrayPool<byte>.Shared.Rent(81920);
        try
        {
            int bytesRead;
            while ((bytesRead = await stream.ReadAsync(buffer)) > 0)
            {
                sha1.TransformBlock(buffer, 0, bytesRead, null, 0);
            }
            sha1.TransformFinalBlock(buffer, 0, 0);
            return BitConverter.ToString(sha1.Hash).Replace("-", "").ToLowerInvariant();
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
}