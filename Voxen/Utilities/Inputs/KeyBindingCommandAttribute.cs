using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Voxen.Utilities.Inputs;

[AttributeUsage(AttributeTargets.Event)]
public class KeyBindingCommandAttribute : Attribute
{
    #region Properties

    public Keys[] Keys { get; }

    #endregion

    #region Constructor
    
    public KeyBindingCommandAttribute(params Keys[] keys)
    {
        // Array.Sort(keys);
        Keys = keys;
    }

    #endregion
}