namespace Logger;

public interface ILogger
{
    #region Properties
    
    public bool IsEnabled { get; set; }

    public LogLevel LogLevel { get; set; }

    #endregion

    #region Public methods

    public void LogVerbose(string messageTemplate, Exception? exception = null);
    public void LogDebug(string messageTemplate, Exception? exception = null);
    public void LogInformation(string messageTemplate, Exception? exception = null);
    public void LogWarning(string messageTemplate, Exception? exception = null);
    public void LogError(string messageTemplate, Exception? exception = null);
    public void LogFatal(string messageTemplate, Exception? exception = null);

    #endregion
}