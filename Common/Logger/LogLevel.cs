namespace Logger;

/// <remarks>
/// We rely on the order of these. Please keep them ordered from least to most important.
/// </remarks>
public enum LogLevel
{
    Verbose = 0,
    Debug = 1,
    Information = 2,
    Warning = 3,
    Error = 4,
    Fatal = 5,
}