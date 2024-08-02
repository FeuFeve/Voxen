using System.Reflection;

namespace GenericInputSystem.Data;

internal class Command<TKey> where TKey : Enum
{
    #region Properties
    
    public required string Name { get; set; }

    public required TKey[] Keys { get; set; }
    
    public required FieldInfo EventBackingFieldInfo { get; set; }

    #endregion
}