using OpenBMCLAPI_IN.Utils;
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
            ConfigInstance = new Config();
            ConfigInstance.LoadConfig().Wait();
            var levelSwitch=new LoggingLevelSwitch();
            levelSwitch.MinimumLevel = Logging.GetLogLevel(ConfigInstance.Instance.Log.LogLevel);
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
                .CreateLogger();
            Log.Logger.Debug("Successfully got config file:\n{content}",ConfigInstance.GetYamlContent());
            Log.Logger.Information("Successfully got config file");
            Log.Logger.Debug("Logger initialized with properties:\npath: {path},\nrollOnFileSizeLimit: {sizeLimitRolling},\nfileSizeLimit: {sizeLimit} bytes,\nrollingInterval: {interval},\noutputTemplate: {optTemplate},\nretainedFileCountLimit: {maxFile}", 
                                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Logging.FormatDateTimePlaceholders(ConfigInstance.Instance.Log.FilePathFormat), Logging.FormatDateTimePlaceholders(ConfigInstance.Instance.Log.FileNameFormat) + ".log"),
                                ConfigInstance.Instance.Log.RollOnFileSizeLimit,
                                ConfigInstance.Instance.Log.MaxSizeOfSingleFile,
                                ConfigInstance.Instance.Log.RollingInterval,
                                ConfigInstance.Instance.Log.OutputFormat,
                                ConfigInstance.Instance.Log.MaxFileOfSingleLaunch);
            Log.Logger.Information("Logger initialization finished");
            var app = builder.Build();
            app.MapGet("/", () => "Hello World!");
            app.Run();
        }
    }
}
