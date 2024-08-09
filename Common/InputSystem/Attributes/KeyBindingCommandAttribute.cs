namespace InputSystem.Attributes;

[AttributeUsage(AttributeTargets.Event, AllowMultiple = true)]
public class KeyBindingCommandAttribute<TKey>
    : Attribute
    where TKey : Enum
{
    #region Properties

    public TKey[] Keys { get; }

    #endregion

    #region Constructor
    
    public KeyBindingCommandAttribute(params TKey[] keys)
    {
        Array.Sort(keys);
        Keys = keys;
    }

    #endregion
}