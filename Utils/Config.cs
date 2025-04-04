using OpenBMCLAPI_IN.Models;
using System.Diagnostics;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
namespace OpenBMCLAPI_IN.Utils
{
    public class Config
    {
        private const string ConfigFile = "config.yml";
        public ConfigModel Instance { get; set; }
        private IDeserializer deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        private ISerializer serializer = new SerializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        public async Task LoadConfig()
        {
            Instance = new();
            string configFile = GetAbsolutePathForConfigFile();
#if DEBUG
            if (File.Exists(ConfigFile))
            {
                File.Delete(ConfigFile);
            }
#endif
            //读取文件


            if (!File.Exists(configFile))
            {
                var fs=File.Create(configFile);
                fs.Close();

            }
            if (string.IsNullOrWhiteSpace(await File.ReadAllTextAsync(configFile)))
            {
                //初始化写入
                var yamlinit = serializer.Serialize(Instance);
                await File.WriteAllTextAsync(configFile, yamlinit, System.Text.Encoding.UTF8);
            }
            var config = await File.ReadAllTextAsync(configFile);
            Instance = deserializer.Deserialize<ConfigModel>(config);
            //直接保存，补全默认值
            //创建序列化器
            var yaml=serializer.Serialize(Instance);
            await File.WriteAllTextAsync(configFile, yaml,System.Text.Encoding.UTF8);

        }
        private string GetAbsolutePathForConfigFile()
        {
            return(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFile));
        }
        public string GetYamlContent()
        {
            return serializer.Serialize(Instance);
        }
    }

}
