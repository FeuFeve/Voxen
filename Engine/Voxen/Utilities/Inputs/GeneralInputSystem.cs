using InputSystem;
using InputSystem.Attributes;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Voxen.Utilities.Inputs;

public class GeneralInputBindingRegistry : InputBindingRegistry<Keys>
{
    #region Commands

    [KeyBindingCommand<Keys>(Keys.LeftControl, Keys.B)]
    [KeyBindingCommand<Keys>(Keys.LeftControl, Keys.S)]
    public static event Action? SaveCommand;

    [KeyBindingCommand<Keys>(Keys.LeftControl, Keys.LeftShift, Keys.S)]
    public static event Action? OpenSettingsCommand;

    #endregion

    #region Constructor

    public GeneralInputBindingRegistry(string name) : base(name) { }

    #endregion
}