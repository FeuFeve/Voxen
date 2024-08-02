namespace GenericInputSystem.Data;

internal class CommandRegistry<TKey> where TKey : Enum
{
    #region Properties

    public required InputBindingRegistry<TKey> InputBindingRegistry { get; set; }

    public List<Command<TKey>> Commands { get; set; } = [];

    #endregion
}