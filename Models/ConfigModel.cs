using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;

namespace OpenBMCLAPI_IN.Models
{
    public class ConfigModel
    {
        public ConfigGeneralModel? General { get; set; } = new();
        public ConfigWebModel? Web { get; set; } = new();
        public ConfigCertModel? Cert { get; set; } = new();
        public ConfigLogModel? Log { get; set; } = new();
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
        public bool Proxy { get; set; } = false;
    }
    public class ConfigCertModel
    {
        public string? SaveDirectory { get; set; } = ".cert";
        public string? Key { get; set; }
        public string? Cert { get; set; }
    }
    public class ConfigLogModel
    {
        public LogEventLevel LogLevel { get; set; } = LogEventLevel.Verbose;
        public string FilePathFormat { get; set; } = "logs/logs_{{yyyy_MM_dd}}";
        public string FileNameFormat { get; set; } = "log_{{yyyyMMdd_HHmmss}}";
        public string OutputFormat { get; set; } = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level:u3}] | {SourceContext} {Message:lj}{NewLine}{Exception}";
        public bool RollOnFileSizeLimit { get; set; } = true;
        public int MaxSizeOfSingleFile { get; set; } = 1024 * 1024 * 128; //128MiB
        public RollingInterval RollingInterval { get; set; } = RollingInterval.Infinite;
        public int MaxFileOfSingleLaunch { get; set; } = 10;
        public Dictionary<ConsoleThemeStyle, string> ConsoleTheme { get; set; } = new Dictionary<ConsoleThemeStyle, string>
        {
            // 基础文本（浅灰 #E0E0E0）→ 中性背景友好
            [ConsoleThemeStyle.Text] = "\x1b[38;5;254m",

            // 次要文本（中灰 #A0A0A0）→ 明确次级关系
            [ConsoleThemeStyle.SecondaryText] = "\x1b[38;5;247m",

            // 三级文本（深灰蓝 #5F9EA0）→ 保留结构层次
            [ConsoleThemeStyle.TertiaryText] = "\x1b[38;5;73m",

            // 无效值（橙红 #FF4500）→ 突出异常
            [ConsoleThemeStyle.Invalid] = "\x1b[38;5;202m",

            // null值（冰蓝 #70DBDB）→ 冷色高亮
            [ConsoleThemeStyle.Null] = "\x1b[38;5;80m",

            // 属性名（紫藤 #9F8CFF）→ 中饱和冷色
            [ConsoleThemeStyle.Name] = "\x1b[38;5;141m",

            // 字符串值（浅珊瑚 #FFA07A）→ 暖色但低攻击性
            [ConsoleThemeStyle.String] = "\x1b[38;5;216m",

            // 数字值（春绿 #00FF7F）→ 高对比冷色
            [ConsoleThemeStyle.Number] = "\x1b[38;5;48m",

            // 布尔值（薰衣草 #BA55D3）→ 独特紫色系
            [ConsoleThemeStyle.Boolean] = "\x1b[38;5;134m",

            // 标量值（孔雀蓝 #33A1C9）→ 稳定视觉锚点
            [ConsoleThemeStyle.Scalar] = "\x1b[38;5;74m",

            /**************** 高对比度日志级别 ****************/
            // Verbose（钢蓝 #4682B4）→ 最低调
            [ConsoleThemeStyle.LevelVerbose] = "\x1b[38;5;25m",

            // Debug（宝石蓝 #4169E1）→ 清晰技术感
            [ConsoleThemeStyle.LevelDebug] = "\x1b[38;5;27m",

            // Information（森林绿 #228B22）→ 自然积极
            [ConsoleThemeStyle.LevelInformation] = "\x1b[38;5;28m",

            // Warning（金黄 #FFD700）→ 温暖警示
            [ConsoleThemeStyle.LevelWarning] = "\x1b[38;5;220m",

            // Error（深红 #B22222）→ 强烈对比
            [ConsoleThemeStyle.LevelError] = "\x1b[38;5;124m",

            // Fatal（白+深红背景）→ 最高紧急度
            [ConsoleThemeStyle.LevelFatal] = "\x1b[97m\x1b[48;5;88m"
        };
    }
}
