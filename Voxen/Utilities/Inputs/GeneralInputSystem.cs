using GenericInputSystem;
using GenericInputSystem.Attributes;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Voxen.Utilities.Inputs;

public class GeneralInputBindingRegistry : InputBindingRegistry<Keys>
{
    #region Commands

    [KeyBindingCommand<Keys>(Keys.LeftControl, Keys.S)]
    public event Action? SaveCommand;

    [KeyBindingCommand<Keys>(Keys.LeftControl, Keys.LeftShift, Keys.S)]
    public event Action? OpenSettingsCommand;

    // TODO: remove these methods (for debug)
    public void OnSaveCommand()
    {
        SaveCommand?.Invoke();
    }

    public void OnOpenSettingsCommand()
    {
        OpenSettingsCommand?.Invoke();
    }

    #endregion

    #region Constructor

    public GeneralInputBindingRegistry(string name) : base(name) { }

    #endregion
}