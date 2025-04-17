namespace OpenBMCLAPI_IN.Core.Storage
{
    public class BmclApiFileInfo
    {
        public required string hash { get; set; }
        public required string path { get; set; }
        public required int size { get; set; }
        public required float mtime { get; set; }
    }
}
