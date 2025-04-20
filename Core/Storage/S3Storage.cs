using Amazon.S3;
using Amazon.S3.Model;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;
using Serilog.Localization;
using Amazon.Runtime;

namespace OpenBMCLAPI_IN.Core.Storage;

public class S3Storage : IStorage
{
    private readonly AmazonS3Client _client;
    private const int MaxRetries = 3;
    private readonly string _bucketName;
    private readonly string _pathPrefix;

    public S3Storage(string endpoint, string bucket, string path, string accessKey, string secretKey)
    {
        var config = new AmazonS3Config
        {
            ServiceURL = endpoint,
            Timeout = Timeout.InfiniteTimeSpan,
            SignatureVersion = "v4",
            ResponseChecksumValidation = ResponseChecksumValidation.WHEN_REQUIRED,
            RequestChecksumCalculation = RequestChecksumCalculation.WHEN_REQUIRED

        };
        var cred = new BasicAWSCredentials(accessKey, secretKey);
        _client = new AmazonS3Client(cred, config);
        _bucketName = bucket;
        _pathPrefix = path.TrimEnd('/') + '/';
    }

    public S3Storage(StorageParameter param)
    {
        var config = new AmazonS3Config
        {
            ServiceURL = param.baseUri,
            Timeout = Timeout.InfiniteTimeSpan,
            SignatureVersion = "v4",
            ResponseChecksumValidation = ResponseChecksumValidation.WHEN_REQUIRED,
            RequestChecksumCalculation = RequestChecksumCalculation.WHEN_REQUIRED
        };
        var cred = new BasicAWSCredentials(param.accessKey, param.secretKey);
        _client = new AmazonS3Client(cred, config);
        _bucketName = param.path.Split('/')[0];
        _pathPrefix = string.Join('/', param.path.Split('/')[1..]).TrimEnd('/') + '/';
    }

    private string GetFullPath(string path) => _pathPrefix + path.TrimStart('/');

    public async Task UploadAsync(string path, Stream content)
    {
        int retryCount = 0;
        while (true)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    if (content.CanSeek)
                        content.Position = 0;
                    await content.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    var request = new PutObjectRequest
                    {
                        BucketName = _bucketName,
                        Key = GetFullPath(path),
                        InputStream = memoryStream,
                        DisablePayloadSigning=true
                    };

                    await _client.PutObjectAsync(request);
                    Log.Logger.InformationL("upload_success", GetFullPath(path));
                }
                break;
            }
            catch (AmazonS3Exception s3Ex)
            {
                Log.Logger.Error(s3Ex, "S3 API错误: {StatusCode} {ErrorCode}", s3Ex.StatusCode, s3Ex.ErrorCode);
                throw new IOException($"S3操作失败: {s3Ex.Message}", s3Ex);
            }
            catch (IOException ex) when (retryCount < MaxRetries)
            {
                retryCount++;
                await Task.Delay(1000 * retryCount);
                content.Position = 0;
            }
            catch (IOException ex)
            {
                throw new IOException($"Upload failed after {retryCount} retries: {ex.Message}", ex);
            }
        }
    }

    public async Task<bool> ExistsAsync(string path)
    {
        try
        {
            var request = new GetObjectMetadataRequest
            {
                BucketName = _bucketName,
                Key = GetFullPath(path)
            };

            var response = await _client.GetObjectMetadataAsync(request);
            return true;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
    }

    public async Task<long> GetSizeAsync(string path)
    {
        var request = new GetObjectMetadataRequest
        {
            BucketName = _bucketName,
            Key = GetFullPath(path)
        };

        var response = await _client.GetObjectMetadataAsync(request);
        return response.ContentLength;
    }

    public async Task<string> GetSha1Async(string path)
    {
        using var response = await _client.GetObjectAsync(_bucketName, GetFullPath(path));
        try
        {
            return await CalculateSha1Async(response.ResponseStream);
        }
        finally
        {
            if (response.ResponseStream.CanSeek)
                response.ResponseStream.Position = 0;
            response.ResponseStream.Close();
        }
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