using System.Diagnostics;

namespace Logger;

public class DefaultLogger : ILogger
{
    #region ILogger properties

    public bool IsEnabled { get; set; } = true;

    public LogLevel LogLevel { get; set; } = LogLevel.Information;

    #endregion

    #region ILogger methods

    public void LogVerbose(string messageTemplate, Exception? exception = null)
    {
        Log(LogLevel.Verbose, messageTemplate, exception);
    }

    public void LogDebug(string messageTemplate, Exception? exception = null)
    {
        Log(LogLevel.Debug, messageTemplate, exception);
    }

    public void LogInformation(string messageTemplate, Exception? exception = null)
    {
        Log(LogLevel.Information, messageTemplate, exception);
    }

    public void LogWarning(string messageTemplate, Exception? exception = null)
    {
        Log(LogLevel.Warning, messageTemplate, exception);
    }

    public void LogError(string messageTemplate, Exception? exception = null)
    {
        Log(LogLevel.Error, messageTemplate, exception);
    }

    public void LogFatal(string messageTemplate, Exception? exception = null)
    {
        Log(LogLevel.Fatal, messageTemplate, exception);
    }

    #endregion

    #region Private methods

    private void Log(LogLevel logLevel, string messageTemplate, Exception? exception)
    {
        if (!IsEnabled || LogLevel > logLevel)
        {
            return;
        }
        
        string currentTime = DateTime.Now.ToString("HH:mm:ss:fff");

        string messagePrefix = logLevel switch
        {
            LogLevel.Verbose => "[VRB]",
            LogLevel.Debug => "[DBG]",
            LogLevel.Information => "[INF]",
            LogLevel.Warning => "[WRN]",
            LogLevel.Error => "[ERR]",
            LogLevel.Fatal => "[FTL]",
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
        };

        Console.WriteLine(exception is null
            ? $"{currentTime} {messagePrefix} {messageTemplate}"
            : $"{currentTime} {messagePrefix} {messageTemplate}.\n{exception}");
    }

    #endregion
}