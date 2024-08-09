using System.Reflection;

namespace GenericInputSystem.Data;

internal class Command<TKey> where TKey : Enum
{
    #region Properties
    
    public required string Name { get; set; }

    /// <summary>
    /// All the key combinations that can trigger the command.
    /// E.g. the "Save" command could be bound to CTRL+S or CTRL+SHIFT+S
    /// </summary>
    public List<TKey[]> KeyCombinations { get; set; } = [];
    
    public required FieldInfo EventBackingFieldInfo { get; set; }

    #endregion
}