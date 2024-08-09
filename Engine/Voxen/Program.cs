using Serilog;
using Voxen;

// Create the Serilog logger
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss:fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

using Window game = new();
game.Run();