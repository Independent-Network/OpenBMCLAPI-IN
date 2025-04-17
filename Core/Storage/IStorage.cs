using OpenBMCLAPI_IN.Utils;
using Serilog;
using Serilog.Localization;
using System.Threading.Channels;
using WebDav;

namespace OpenBMCLAPI_IN.Core.Storage
{
    public interface IStorage
    {
        public static readonly List<int> MEASURE_SIZES = [10, 20, 30, 40, 50, 100, 200];
        //接口
        public Task<bool> CheckMeasure(int size);
        //public Task<List<BmclApiFileInfo>> ListFiles();
        /// <summary>
        /// 流式上传文件
        /// </summary>
        /// <param name="stream">输入流</param>
        /// <param name="destinationPath">目标路径</param>
        /// <param name="contentType">内容类型</param>
        /// <param name="metadata">元数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>上传结果</returns>
        public Task UploadAsync(
            Stream stream,
            string destinationUri,
        CancellationToken cancellationToken = default);
        //public Task<BmclApiFileInfo> GetFile();
        //接口默认方法
        public async Task CheckMeasures()
        {
            do
            {
                try
                {
                    foreach (var i in MEASURE_SIZES)
                    {

                        if (!CheckMeasure(i).GetAwaiter().GetResult())
                        {
                            Log.Logger.InformationL("lost_measure", i + "MB");
                            try
                            {
                                await WriteMeasure(i);
                            }
                            catch { }
                        }

                    }
                    Log.Logger.InformationL("created_measure");
                }
                catch { }
                await Task.Delay(Program.ConfigInstance.Instance.General.InspectMeasureInterval);
            } while (Program.ConfigInstance.Instance.General.ContinuouslyInspectMeasure);
        }
        public async Task WriteMeasure(int size)
        {
            string path = "/dav/measures/" + size;
            size = size * 1024 * 1024;

            // 直接使用数组，避免MemoryStream开销
            byte[] buffer = new byte[size];
            Array.Fill(buffer, (byte)41);

            using (var ms = new MemoryStream(buffer)) // 这里不复制数组
            {
                await UploadAsync(ms, path);
            }

            // 显式释放大数组
            buffer = null;
        }
    }

}
