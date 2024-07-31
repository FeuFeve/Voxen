namespace GenericInputSystem;

internal class Command<TKey> where TKey : Enum
{
    public TKey[] Keys { get; set; } = [];
    
    // TODO: store an accessor to the event so we can Invoke() it
    // TODO: store the name of the command?
    // TODO: store the name of the registry?
}