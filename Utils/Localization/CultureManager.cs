using System.Globalization;

namespace OpenBMCLAPI_IN.Utils.Localization
{
    public static class CultureManager
    {
        private static readonly AsyncLocal<CultureInfo> _currentCulture = new AsyncLocal<CultureInfo>();

        public static CultureInfo Current
        {
            get => _currentCulture.Value ?? CultureInfo.DefaultThreadCurrentCulture ?? CultureInfo.InvariantCulture;
            set
            {
                _currentCulture.Value = value;
                Thread.CurrentThread.CurrentCulture = value;
                Thread.CurrentThread.CurrentUICulture = value;

                // 触发缓存更新
                if (!ResxCache.IsCultureCached(value))
                {
                    ResxCache.PreloadCulture(value);
                }
            }
        }

        public static void SwitchCulture(CultureInfo newCulture, Action action)
        {
            var original = Current;
            try
            {
                Current = newCulture;
                action();
            }
            finally
            {
                Current = original;
            }
        }
    }
}
