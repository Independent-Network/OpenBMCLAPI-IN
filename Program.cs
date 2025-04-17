using OpenBMCLAPI_IN.Utils;
using Serilog.Localization;
using Serilog;
using Serilog.Core;
using Serilog.Extensions.Hosting;
using OpenBMCLAPI_IN.Resources;
using Serilog.Sinks.SystemConsole.Themes;
using OpenBMCLAPI_IN.Core.Storage;
using System.Net;
using WebDav;
namespace OpenBMCLAPI_IN
{
    public class Program
    {
        public static Config ConfigInstance { get; set; }
        public static void Main(string[] args)
        {            
            //配置控制台
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            //读取配置
            ConfigInstance = new Config();
            ConfigInstance.LoadConfig().Wait();
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
                .WithLocalization(typeof(LogResource), ConfigInstance.Instance.General.Locale)
                .WriteTo.Console(
                    theme: new AnsiConsoleTheme(ConfigInstance.Instance.Log.ConsoleTheme),
                    outputTemplate: ConfigInstance.Instance.Log.OutputFormat
                )       
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
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog(Log.Logger);
            var app = builder.Build();
            
            app.MapGet("/", () => "Hello World!");
            app.MapGet("/check_measure", () =>
            {
                IStorage webdav = new WebDavStorage("test", "test", "https://server.ind-network.top:5244/");
                _ = webdav.CheckMeasures();
            });
            app.Run();
        }
    }
}
