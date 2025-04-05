using System;
using Serilog;
using Serilog.Events;
using ILogger = Serilog.ILogger;

namespace OpenBMCLAPI_IN.Utils.Localization
{
    public static class LoggerExtensions
    {
        private static string LocalizeTemplate(string key)
        {
            return ResxCache.GetString(key).Replace("\\n", Environment.NewLine);
        }

        #region Verbose (10 overloads)
        public static void VerboseL(this ILogger logger, string messageTemplate)
            => logger.Verbose(LocalizeTemplate("verbose." + messageTemplate));

        public static void VerboseL<T>(this ILogger logger, string messageTemplate, T propertyValue)
            => logger.Verbose(LocalizeTemplate("verbose." + messageTemplate), propertyValue);

        public static void VerboseL<T0, T1>(this ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
            => logger.Verbose(LocalizeTemplate("verbose." + messageTemplate), propertyValue0, propertyValue1);

        public static void VerboseL<T0, T1, T2>(this ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
            => logger.Verbose(LocalizeTemplate("verbose." + messageTemplate), propertyValue0, propertyValue1, propertyValue2);

        public static void VerboseL(this ILogger logger, string messageTemplate, params object[] propertyValues)
            => logger.Verbose(LocalizeTemplate("verbose." + messageTemplate), propertyValues);

        public static void VerboseL(this ILogger logger, Exception exception, string messageTemplate)
            => logger.Verbose(exception, LocalizeTemplate("verbose." + messageTemplate));

        public static void VerboseL<T>(this ILogger logger, Exception exception, string messageTemplate, T propertyValue)
            => logger.Verbose(exception, LocalizeTemplate("verbose." + messageTemplate), propertyValue);

        public static void VerboseL<T0, T1>(this ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
            => logger.Verbose(exception, LocalizeTemplate("verbose." + messageTemplate), propertyValue0, propertyValue1);

        public static void VerboseL<T0, T1, T2>(this ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
            => logger.Verbose(exception, LocalizeTemplate("verbose." + messageTemplate), propertyValue0, propertyValue1, propertyValue2);

        public static void VerboseL(this ILogger logger, Exception exception, string messageTemplate, params object[] propertyValues)
            => logger.Verbose(exception, LocalizeTemplate("verbose." + messageTemplate), propertyValues);
        #endregion

        #region Debug (10 overloads)
        public static void DebugL(this ILogger logger, string messageTemplate)
            => logger.Debug(LocalizeTemplate("debug." + messageTemplate));

        public static void DebugL<T>(this ILogger logger, string messageTemplate, T propertyValue)
            => logger.Debug(LocalizeTemplate("debug." + messageTemplate), propertyValue);

        public static void DebugL<T0, T1>(this ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
            => logger.Debug(LocalizeTemplate("debug." + messageTemplate), propertyValue0, propertyValue1);

        public static void DebugL<T0, T1, T2>(this ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
            => logger.Debug(LocalizeTemplate("debug." + messageTemplate), propertyValue0, propertyValue1, propertyValue2);

        public static void DebugL(this ILogger logger, string messageTemplate, params object[] propertyValues)
            => logger.Debug(LocalizeTemplate("debug." + messageTemplate), propertyValues);

        public static void DebugL(this ILogger logger, Exception exception, string messageTemplate)
            => logger.Debug(exception, LocalizeTemplate("debug." + messageTemplate));

        public static void DebugL<T>(this ILogger logger, Exception exception, string messageTemplate, T propertyValue)
            => logger.Debug(exception, LocalizeTemplate("debug." + messageTemplate), propertyValue);

        public static void DebugL<T0, T1>(this ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
            => logger.Debug(exception, LocalizeTemplate("debug." + messageTemplate), propertyValue0, propertyValue1);

        public static void DebugL<T0, T1, T2>(this ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
            => logger.Debug(exception, LocalizeTemplate("debug." + messageTemplate), propertyValue0, propertyValue1, propertyValue2);

        public static void DebugL(this ILogger logger, Exception exception, string messageTemplate, params object[] propertyValues)
            => logger.Debug(exception, LocalizeTemplate("debug." + messageTemplate), propertyValues);
        #endregion

        #region Information (10 overloads)
        public static void InformationL(this ILogger logger, string messageTemplate)
            => logger.Information(LocalizeTemplate("info." + messageTemplate));

        public static void InformationL<T>(this ILogger logger, string messageTemplate, T propertyValue)
            => logger.Information(LocalizeTemplate("info." + messageTemplate), propertyValue);

        public static void InformationL<T0, T1>(this ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
            => logger.Information(LocalizeTemplate("info." + messageTemplate), propertyValue0, propertyValue1);

        public static void InformationL<T0, T1, T2>(this ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
            => logger.Information(LocalizeTemplate("info." + messageTemplate), propertyValue0, propertyValue1, propertyValue2);

        public static void InformationL(this ILogger logger, string messageTemplate, params object[] propertyValues)
            => logger.Information(LocalizeTemplate("info." + messageTemplate), propertyValues);

        public static void InformationL(this ILogger logger, Exception exception, string messageTemplate)
            => logger.Information(exception, LocalizeTemplate("info." + messageTemplate));

        public static void InformationL<T>(this ILogger logger, Exception exception, string messageTemplate, T propertyValue)
            => logger.Information(exception, LocalizeTemplate("info." + messageTemplate), propertyValue);

        public static void InformationL<T0, T1>(this ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
            => logger.Information(exception, LocalizeTemplate("info." + messageTemplate), propertyValue0, propertyValue1);

        public static void InformationL<T0, T1, T2>(this ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
            => logger.Information(exception, LocalizeTemplate("info." + messageTemplate), propertyValue0, propertyValue1, propertyValue2);

        public static void InformationL(this ILogger logger, Exception exception, string messageTemplate, params object[] propertyValues)
            => logger.Information(exception, LocalizeTemplate("info." + messageTemplate), propertyValues);
        #endregion

        #region Warning (10 overloads)
        public static void WarningL(this ILogger logger, string messageTemplate)
            => logger.Warning(LocalizeTemplate("warning." + messageTemplate));

        public static void WarningL<T>(this ILogger logger, string messageTemplate, T propertyValue)
            => logger.Warning(LocalizeTemplate("warning." + messageTemplate), propertyValue);

        public static void WarningL<T0, T1>(this ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
            => logger.Warning(LocalizeTemplate("warning." + messageTemplate), propertyValue0, propertyValue1);

        public static void WarningL<T0, T1, T2>(this ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
            => logger.Warning(LocalizeTemplate("warning." + messageTemplate), propertyValue0, propertyValue1, propertyValue2);

        public static void WarningL(this ILogger logger, string messageTemplate, params object[] propertyValues)
            => logger.Warning(LocalizeTemplate("warning." + messageTemplate), propertyValues);

        public static void WarningL(this ILogger logger, Exception exception, string messageTemplate)
            => logger.Warning(exception, LocalizeTemplate("warning." + messageTemplate));

        public static void WarningL<T>(this ILogger logger, Exception exception, string messageTemplate, T propertyValue)
            => logger.Warning(exception, LocalizeTemplate("warning." + messageTemplate), propertyValue);

        public static void WarningL<T0, T1>(this ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
            => logger.Warning(exception, LocalizeTemplate("warning." + messageTemplate), propertyValue0, propertyValue1);

        public static void WarningL<T0, T1, T2>(this ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
            => logger.Warning(exception, LocalizeTemplate("warning." + messageTemplate), propertyValue0, propertyValue1, propertyValue2);

        public static void WarningL(this ILogger logger, Exception exception, string messageTemplate, params object[] propertyValues)
            => logger.Warning(exception, LocalizeTemplate("warning." + messageTemplate), propertyValues);
        #endregion

        #region Error (10 overloads)
        public static void ErrorL(this ILogger logger, string messageTemplate)
            => logger.Error(LocalizeTemplate("error." + messageTemplate));

        public static void ErrorL<T>(this ILogger logger, string messageTemplate, T propertyValue)
            => logger.Error(LocalizeTemplate("error." + messageTemplate), propertyValue);

        public static void ErrorL<T0, T1>(this ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
            => logger.Error(LocalizeTemplate("error." + messageTemplate), propertyValue0, propertyValue1);

        public static void ErrorL<T0, T1, T2>(this ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
            => logger.Error(LocalizeTemplate("error." + messageTemplate), propertyValue0, propertyValue1, propertyValue2);

        public static void ErrorL(this ILogger logger, string messageTemplate, params object[] propertyValues)
            => logger.Error(LocalizeTemplate("error." + messageTemplate), propertyValues);

        public static void ErrorL(this ILogger logger, Exception exception, string messageTemplate)
            => logger.Error(exception, LocalizeTemplate("error." + messageTemplate));

        public static void ErrorL<T>(this ILogger logger, Exception exception, string messageTemplate, T propertyValue)
            => logger.Error(exception, LocalizeTemplate("error." + messageTemplate), propertyValue);

        public static void ErrorL<T0, T1>(this ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
            => logger.Error(exception, LocalizeTemplate("error." + messageTemplate), propertyValue0, propertyValue1);

        public static void ErrorL<T0, T1, T2>(this ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
            => logger.Error(exception, LocalizeTemplate("error." + messageTemplate), propertyValue0, propertyValue1, propertyValue2);

        public static void ErrorL(this ILogger logger, Exception exception, string messageTemplate, params object[] propertyValues)
            => logger.Error(exception, LocalizeTemplate("error." + messageTemplate), propertyValues);
        #endregion

        #region Fatal (10 overloads)
        public static void FatalL(this ILogger logger, string messageTemplate)
            => logger.Fatal(LocalizeTemplate("fatal." + messageTemplate));

        public static void FatalL<T>(this ILogger logger, string messageTemplate, T propertyValue)
            => logger.Fatal(LocalizeTemplate("fatal." + messageTemplate), propertyValue);

        public static void FatalL<T0, T1>(this ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
            => logger.Fatal(LocalizeTemplate("fatal." + messageTemplate), propertyValue0, propertyValue1);

        public static void FatalL<T0, T1, T2>(this ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
            => logger.Fatal(LocalizeTemplate("fatal." + messageTemplate), propertyValue0, propertyValue1, propertyValue2);

        public static void FatalL(this ILogger logger, string messageTemplate, params object[] propertyValues)
            => logger.Fatal(LocalizeTemplate("fatal." + messageTemplate), propertyValues);

        public static void FatalL(this ILogger logger, Exception exception, string messageTemplate)
            => logger.Fatal(exception, LocalizeTemplate("fatal." + messageTemplate));

        public static void FatalL<T>(this ILogger logger, Exception exception, string messageTemplate, T propertyValue)
            => logger.Fatal(exception, LocalizeTemplate("fatal." + messageTemplate), propertyValue);

        public static void FatalL<T0, T1>(this ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
            => logger.Fatal(exception, LocalizeTemplate("fatal." + messageTemplate), propertyValue0, propertyValue1);

        public static void FatalL<T0, T1, T2>(this ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
            => logger.Fatal(exception, LocalizeTemplate("fatal." + messageTemplate), propertyValue0, propertyValue1, propertyValue2);

        public static void FatalL(this ILogger logger, Exception exception, string messageTemplate, params object[] propertyValues)
            => logger.Fatal(exception, LocalizeTemplate("fatal." + messageTemplate), propertyValues);
        #endregion
    }
}