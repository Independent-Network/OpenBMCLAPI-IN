using OpenBMCLAPI_IN.Utils;
using OpenBMCLAPI_IN.Utils.Localization;
using Serilog;
using Serilog.Core;
namespace OpenBMCLAPI_IN
{
    public class Program
    {
        public static Config ConfigInstance { get; set; }
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //读取配置
            ConfigInstance = new Config();
            ConfigInstance.LoadConfig().Wait();
            //初始本地化
            CultureManager.Current = new System.Globalization.CultureInfo(ConfigInstance.Instance.General.Locale);
            //配置日志
            var levelSwitch=new LoggingLevelSwitch();
            levelSwitch.MinimumLevel = ConfigInstance.Instance.Log.LogLevel;
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .WriteTo.File(
                path: Path.Combine(AppDomain.CurrentDomain.BaseDirectory,Logging.FormatDateTimePlaceholders(ConfigInstance.Instance.Log.FilePathFormat), Logging.FormatDateTimePlaceholders(ConfigInstance.Instance.Log.FileNameFormat)+".log"),
                rollOnFileSizeLimit: ConfigInstance.Instance.Log.RollOnFileSizeLimit,
                fileSizeLimitBytes: ConfigInstance.Instance.Log.MaxSizeOfSingleFile,
                rollingInterval: ConfigInstance.Instance.Log.RollingInterval,
                outputTemplate: ConfigInstance.Instance.Log.OutputFormat,
                retainedFileCountLimit: ConfigInstance.Instance.Log.MaxFileOfSingleLaunch
                )
                .WriteTo.Console()       
                .CreateLogger();
            Log.Logger.DebugL("got_config_file",ConfigInstance.GetYamlContent());
            Log.Logger.InformationL("got_config_file");
            Log.Logger.DebugL("logger_initialized", 
                                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Logging.FormatDateTimePlaceholders(ConfigInstance.Instance.Log.FilePathFormat), Logging.FormatDateTimePlaceholders(ConfigInstance.Instance.Log.FileNameFormat) + ".log"),
                                ConfigInstance.Instance.Log.RollOnFileSizeLimit,
                                ConfigInstance.Instance.Log.MaxSizeOfSingleFile,
                                ConfigInstance.Instance.Log.RollingInterval,
                                ConfigInstance.Instance.Log.OutputFormat,
                                ConfigInstance.Instance.Log.MaxFileOfSingleLaunch);
            Log.Logger.InformationL("logger_initialized");
            var app = builder.Build();
            app.MapGet("/", () => "Hello World!");
            app.Run();
        }
    }
}
