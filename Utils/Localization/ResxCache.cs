using System.Collections.Concurrent;
using System.Collections;
using System.Globalization;
using System.Resources;
using Serilog;
using System.Runtime.CompilerServices;
using OpenBMCLAPI_IN.Resources;

namespace OpenBMCLAPI_IN.Utils.Localization
{
    public static class ResxCache
    {

        private static readonly ResourceManager _resourceManager = new ResourceManager(typeof(LogResource));

        // 线程安全的三层缓存结构：Culture -> Key -> Value
        private static readonly ConcurrentDictionary<CultureInfo, ConcurrentDictionary<string, string>> _cache =
            new ConcurrentDictionary<CultureInfo, ConcurrentDictionary<string, string>>();

        // 已预加载文化标记
        private static readonly ConcurrentBag<CultureInfo> _loadedCultures = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCultureCached(CultureInfo culture)
        {
            return _loadedCultures.Contains(culture);
        }

        public static string GetString(string key, CultureInfo culture = null)
        {
            culture ??= CultureManager.Current;

            // 双重检查锁定模式
            if (!IsCultureCached(culture))
            {
                lock (_loadedCultures)
                {
                    if (!IsCultureCached(culture))
                    {
                        LoadCulture(culture);
                    }
                }
            }

            return _cache.GetOrAdd(culture, _ => new ConcurrentDictionary<string, string>())
                .GetOrAdd(key, k =>
                {
                    try
                    {
                        return _resourceManager.GetString(k, culture) ?? $"[[{k}]]";
                    }
                    catch (MissingManifestResourceException)
                    {
                        return $"[[{k}_MISSING]]";
                    }
                });
        }

        public static void PreloadCulture(CultureInfo culture)
        {
            if (!IsCultureCached(culture))
            {
                LoadCulture(culture);
            }
        }

        private static void LoadCulture(CultureInfo culture)
        {
            var cultureDict = new ConcurrentDictionary<string, string>();

            try
            {
                // 移除using语句，让ResourceManager自行管理资源集生命周期
                var resourceSet = _resourceManager.GetResourceSet(culture, createIfNotExists: true, tryParents: true);
                if (resourceSet != null)
                {
                    // 立即遍历资源集以确保加载完成
                    var enumerator = resourceSet.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        if (enumerator.Key is string key && enumerator.Value is string value)
                        {
                            cultureDict.TryAdd(key, value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed to load culture {Culture}: {Message}", culture.Name, ex.Message);
            }

            // 合并到现有缓存而不是覆盖
            var existing = _cache.GetOrAdd(culture, _ => new ConcurrentDictionary<string, string>());
            foreach (var pair in cultureDict)
            {
                existing.TryAdd(pair.Key, pair.Value);
            }

            _loadedCultures.Add(culture);
        }

        public static void ClearCultureCache(CultureInfo culture)
        {
            _cache.TryRemove(culture, out _);
            _loadedCultures.TryTake(out _); // 非精确清理，但足够实用
        }
    }
}
