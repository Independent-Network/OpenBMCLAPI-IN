﻿namespace OpenBMCLAPI_IN.Models
{
    public class ConfigModel
    {
        public ConfigGeneralModel? General { get; set; }
        public ConfigWebModel? Web { get; set; }
    }
    public class ConfigGeneralModel
    {
        public string Locale { get; set; } = "en-US";
        public bool Debug { get; set; } = false;
        public bool AccessLog { get; set; } = false;
        public string? Hostname { get; set; }
        public string BaseUrl { get; set; } = "https://openbmclapi.bangbang93.com";
        public string BdUrl { get; set; } = "https://bd.bangbang93.com";
        public bool StorageMeasure { get; set; } = false;
        public int ClusterUpFailedTimes { get; set; } = 90;
        public TimeSpan ClusterUpFailedInterval { get; set; } = new TimeSpan(24, 0, 0);
        public bool ConcurrencyEnableCluster { get; set; } = false;
    }
    public class ConfigWebModel
    {
        public int Port { get; set; } = 4001;
        public int PublicPort { get; set; } = 4001;
        public bool Proxy {  get; set; } = false;
    }
    public class ConfigCertModel
    {
        public string? SaveDirectory { get; set; } = ".cert";
        public string? Key {  get; set; }
        public string? Cert { get; set; }
    }
}
