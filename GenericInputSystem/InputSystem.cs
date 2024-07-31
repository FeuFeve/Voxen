using System.Diagnostics;
using System.Reflection;
using GenericInputSystem.Attributes;

namespace GenericInputSystem;

public static class InputSystem<TKey> where TKey : Enum
{
    #region Public static methods

    public static void AddRegistry(InputBindingRegistry<TKey> registry)
    {
        WriteToConsole($"Registering {nameof(InputBindingRegistry<TKey>)}: {registry.Name}");

        if (!IsNewRegistry(registry))
        {
            // We should not try to add an already existing registry 
            Debug.Assert(false);

            return;
        }

        EventInfo[] commands = registry.GetType().GetEvents(BindingFlags.Instance | BindingFlags.Public);

        foreach (EventInfo command in commands)
        {
            WriteToConsole(command);

            object[] attributes = command.GetCustomAttributes(typeof(KeyBindingCommandAttribute<TKey>), false);

            if (attributes.Length != 1)
            {
                WriteToConsole($"Command '{command}' does not have an associated {nameof(KeyBindingCommandAttribute<TKey>)}");
                
                // All events should have only one KeyBindingCommandAttribute
                Debug.Assert(false);
                
                break;
            }

            FieldInfo? fieldInfo = registry.GetType().GetField(command.Name, BindingFlags.Instance | BindingFlags.Public);
            // TODO: investigate why fieldInfo is always null here
            if (fieldInfo is null)
            {
                // All events should have an associated field
                Debug.Assert(false);
                
                break;
            }

            Delegate? eventDelegate = (Delegate?)fieldInfo.GetValue(null);
            
            if (eventDelegate is null)
            {
                // All events should have an associated Delegate
                Debug.Assert(false);
                
                break;
            }

            KeyBindingCommandAttribute<TKey> keyBindingCommandAttribute = (KeyBindingCommandAttribute<TKey>)attributes[0];

            WriteToConsole($"Bound to keys: {string.Join(", ", keyBindingCommandAttribute.Keys)}");
            
            foreach (Delegate handler in eventDelegate.GetInvocationList())
            {
                WriteToConsole($"Delegate: {handler.Method.Name}");
            }
        }
        
        s_registries.Add(registry);
    }

    public static void OnKeyDown(TKey key)
    {
        if (s_keysPressed.Add(key))
        {
            CheckForCommands();
        }

        // TODO: check for inputs
    }

    public static void OnKeyUp(TKey key)
    {
        s_keysPressed.Remove(key);
    }

    #endregion

    #region Private static mehods

    private static void WriteToConsole(object message)
    {
        Console.WriteLine($"{nameof(InputSystem<TKey>)}: {message}");
    }

    private static bool IsNewRegistry(InputBindingRegistry<TKey> registry)
    {
        if (s_registries.Any(existingRegistry => existingRegistry.Name == registry.Name))
        {
            return false;
        }

        return !s_registries.Contains(registry);
    }

    private static void CheckForCommands()
    {
        TKey[] keysPressed = s_keysPressed.ToArray();
        Array.Sort(keysPressed);
        
        // TODO: check for commands using the Command class?
    }

    #endregion

    #region Private static variables

    private static readonly HashSet<TKey> s_keysPressed = [];

    private static List<InputBindingRegistry<TKey>> s_registries = new();

    #endregion
}