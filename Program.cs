using OpenBMCLAPI_IN.Utils;
namespace OpenBMCLAPI_IN
{
    public class Program
    {
        public static Config ConfigInstance { get; set; }
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();
            ConfigInstance = new Config();
            ConfigInstance.LoadConfig().Wait();
            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
