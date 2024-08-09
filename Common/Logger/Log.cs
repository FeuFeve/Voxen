namespace Logger;

public static class Log
{
    // TODO: delegate for verbose to fatal log, and to get/set the IsEnabled and LogLevel
    // Or maybe just use a ILogger with the DefaultLogger, and override it with Serilog redirection?

    #region Public static properties

    public static ILogger Logger { get; set; } = new DefaultLogger();

    #endregion
    
    #region Public static methods

    public static void Verbose(string messageTemplate, Exception? exception = null)
    {
        Logger.LogVerbose(messageTemplate, exception);
    }

    public static void Debug(string messageTemplate, Exception? exception = null)
    {
        Logger.LogDebug(messageTemplate, exception);
    }

    public static void Information(string messageTemplate, Exception? exception = null)
    {
        Logger.LogInformation(messageTemplate, exception);
    }

    public static void Warning(string messageTemplate, Exception? exception = null)
    {
        Logger.LogWarning(messageTemplate, exception);
    }

    public static void Error(string messageTemplate, Exception? exception = null)
    {
        Logger.LogError(messageTemplate, exception);
    }

    public static void Fatal(string messageTemplate, Exception? exception = null)
    {
        Logger.LogFatal(messageTemplate, exception);
    }

    #endregion
}