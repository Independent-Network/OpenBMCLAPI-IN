namespace OpenBMCLAPI_IN.Core.Storage
{
    public class StorageParameter
    {
        public string? baseUri {  get; set; }
        public string? path { get; set; }
        public string? userName { get; set; }
        public string? password { get; set; }
        public string? bucket { get; set; }
        public string? accessKey {  get; set; }
        public string? secretKey {  get; set; }
    }
}
