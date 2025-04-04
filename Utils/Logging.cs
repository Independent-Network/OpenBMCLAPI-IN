using Serilog.Core;
using Serilog.Events;
using System.Text.RegularExpressions;

namespace OpenBMCLAPI_IN.Utils
{
    public class Logging
    {
        /// <summary>
        /// 替换字符串中的 {{...}} 占位符（排除 {{Counter}}），用当前时间格式化。
        /// 例如："log_{{yyyyMMdd}}_{{HHmmss}}_{{Counter}}.log" -> "log_20231010_143000_{{Counter}}.log"
        /// </summary>
        public static string FormatDateTimePlaceholders(string input)
        {
            // 正则匹配所有 {{...}}，但排除 {{Counter}}
            var regex = new Regex(@"\{\{([^}]+)\}\}");

            // 替换匹配项
            string result = regex.Replace(input, match =>
            {
                string format = match.Groups[1].Value; // 提取 yyyyMMdd 等格式
                try
                {
                    return DateTime.Now.ToString(format);
                }
                catch (FormatException)
                {
                    // 如果格式无效，保留原占位符
                    return match.Value;
                }
            });

            return result;
        }
    }
}
