namespace GenericInputSystem;

public abstract class InputBindingRegistry<TKey> where TKey : Enum
{
    #region Properties

    public string Name { get; }

    #endregion

    #region Constructor

    public InputBindingRegistry(string name)
    {
        Name = name;
    }

    #endregion
}