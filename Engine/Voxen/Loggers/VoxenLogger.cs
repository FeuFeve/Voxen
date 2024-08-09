using Logger;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Voxen.Loggers;

public class VoxenLogger : Logger.ILogger
{
    #region Public static methods

    public static void Init()
    {
        // var customTheme = new AnsiConsoleTheme(new Dictionary<ConsoleThemeStyle, string>
        // {
        //     [ConsoleThemeStyle.Text] = "\x1b[37m",  // White
        //     [ConsoleThemeStyle.SecondaryText] = "\x1b[90m", // Dark Gray
        //     [ConsoleThemeStyle.TertiaryText] = "\x1b[90m", // Dark Gray
        //     [ConsoleThemeStyle.Invalid] = "\x1b[33m", // Yellow
        //     [ConsoleThemeStyle.Null] = "\x1b[37m", // White
        //     [ConsoleThemeStyle.Name] = "\x1b[36m", // Cyan
        //     [ConsoleThemeStyle.String] = "\x1b[32m", // Green
        //     [ConsoleThemeStyle.Number] = "\x1b[33m", // Yellow
        //     [ConsoleThemeStyle.Boolean] = "\x1b[35m", // Magenta
        //     [ConsoleThemeStyle.Scalar] = "\x1b[32m", // Green
        //     [ConsoleThemeStyle.LevelVerbose] = "\x1b[30m", // Black
        //     [ConsoleThemeStyle.LevelDebug] = "\x1b[34m", // Blue
        //     [ConsoleThemeStyle.LevelInformation] = "\x1b[32m", // Green
        //     [ConsoleThemeStyle.LevelWarning] = "\x1b[33m", // Yellow
        //     [ConsoleThemeStyle.LevelError] = "\x1b[31m", // Red
        //     [ConsoleThemeStyle.LevelFatal] = "\x1b[31m\x1b[1m", // Bold Red
        // });
        
        // Create the Serilog logger
        Serilog.Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss:fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();
        
        Logger.Log.Logger = new VoxenLogger();
    }

    #endregion

    #region Constructor

    protected VoxenLogger() { }

    #endregion

    #region ILogger properties

    public bool IsEnabled { get; set; } = true;

    public LogLevel LogLevel { get; set; } = LogLevel.Verbose;

    #endregion

    #region ILogger methods

    public void LogVerbose(string messageTemplate, Exception? exception = null)
    {
        if (!IsEnabled || LogLevel > LogLevel.Verbose)
        {
            return;
        }
        
        Serilog.Log.Verbose(exception, messageTemplate);
    }

    public void LogDebug(string messageTemplate, Exception? exception = null)
    {
        if (!IsEnabled || LogLevel > LogLevel.Debug)
        {
            return;
        }

        Serilog.Log.Debug(exception, messageTemplate);
    }

    public void LogInformation(string messageTemplate, Exception? exception = null)
    {
        if (!IsEnabled || LogLevel > LogLevel.Information)
        {
            return;
        }

        Serilog.Log.Information(exception, messageTemplate);
    }

    public void LogWarning(string messageTemplate, Exception? exception = null)
    {
        if (!IsEnabled || LogLevel > LogLevel.Warning)
        {
            return;
        }

        Serilog.Log.Warning(exception, messageTemplate);
    }

    public void LogError(string messageTemplate, Exception? exception = null)
    {
        if (!IsEnabled || LogLevel > LogLevel.Error)
        {
            return;
        }

        Serilog.Log.Error(exception, messageTemplate);
    }

    public void LogFatal(string messageTemplate, Exception? exception = null)
    {
        if (!IsEnabled || LogLevel > LogLevel.Fatal)
        {
            return;
        }

        Serilog.Log.Fatal(exception, messageTemplate);
    }

    #endregion
}