using Serilog;
using Serilog.Localization;
using System.Net;
using System.Threading.Channels;
using WebDav;

namespace OpenBMCLAPI_IN.Core.Storage
{
    public class WebDavStorage : IStorage
    {
        private readonly IWebDavClient _client;
        private readonly string _baseUri;
        private readonly int _bufferSize;
        private readonly string _userName;
        private readonly string _password;

        public WebDavStorage(
            string userName,
            string password,
            string baseUri = "",
            int bufferSize = 81920 /* 80KB */
)
        {
            var clientParams = new WebDavClientParams
            {
                BaseAddress = new Uri(baseUri),
                Credentials = new NetworkCredential(userName, password)
            };
            _client = new WebDavClient(clientParams);
            _baseUri = baseUri.TrimEnd('/');
            _bufferSize = bufferSize;
            _userName = userName;
            _password = password;
        }
        public async Task<bool> CheckMeasure(int size)
        {
            string file = "/dav/measures/" + size;

            var propfindParams = new PropfindParameters
            {
                RequestType = PropfindRequestType.AllProperties
            };
            var folderResponse = await _client.Propfind("measures", propfindParams);
            if (!folderResponse.IsSuccessful)
            {
                await _client.Mkcol("measures");
            }
            var response = await _client.Propfind(file, propfindParams);

            if (response.IsSuccessful && response.Resources.Any() && response.Resources.First().ContentLength == size * 1024 * 1024)
            {
                Log.Logger.InformationL("measure_verification_passed", file);
                return true;
            }
            else if (response.IsSuccessful && response.Resources.Any())
            {
                Log.Logger.WarningL("measure_size_not_match", file, response.Resources.First().ContentLength, size * 1024 * 1024);
                await _client.Delete(file);
                return false;
            }
            else if (response.StatusCode == 404 || !response.Resources.Any())
            {
                //文件不存在，需要创建
                Log.Logger.WarningL("measure_not_found", file);
                return false;
            }
            else
            {
                Log.Logger.ErrorL("failed_to_reach_measure", file, response.StatusCode, response.ToString());
                throw new WebException("File cannot be reached");
            }
        }
        public async Task UploadAsync(
            Stream stream,
            string destinationUri,
        CancellationToken cancellationToken = default)
        {
            await _client.PutFile(destinationUri, stream);
            Log.Logger.InformationL("upload_success",destinationUri);
        }
    }
}
