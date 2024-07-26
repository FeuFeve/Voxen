using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Voxen.Utilities.Inputs;

public static class GeneralInputSystem
{
    [KeyBindingCommand(Keys.LeftControl, Keys.S)] public static event Action? SaveCommand;
    [KeyBindingCommand(Keys.LeftControl, Keys.LeftShift, Keys.S)] public static event Action? OpenSettingsCommand;

    // public static event EventHandler? Save;
    //
    // public static void Execute()
    // {
    //     Save += () => Console.WriteLine("Save executed");
    //     
    //     Save.Invoke();
    // }
}