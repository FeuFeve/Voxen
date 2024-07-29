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

        EventInfo[] commands = registry.GetType().GetEvents(BindingFlags.Static | BindingFlags.Public);

        foreach (EventInfo command in commands)
        {
            Console.WriteLine(command);

            object[] attributes = command.GetCustomAttributes(typeof(KeyBindingCommandAttribute<TKey>), false);

            if (attributes.Length != 1)
            {
                Console.WriteLine($"Command '{command}' does not have an associated {nameof(KeyBindingCommandAttribute<TKey>)}");
                
                // All events should have only one KeyBindingCommandAttribute
                Debug.Assert(false);
                
                break;
            }

            KeyBindingCommandAttribute<TKey> keyBindingCommandAttribute = (KeyBindingCommandAttribute<TKey>)attributes[0];

            Console.WriteLine($"Bound to keys: {string.Join(", ", keyBindingCommandAttribute.Keys)}");
        }
        
        s_registries.Add(registry);
    }

    public static void OnKeyDown(TKey key)
    {
        if (s_keysPressed.Add(key))
        {
            // TODO: check for commands
        }

        // TODO: check for inputs
    }

    public static void OnKeyUp(TKey key)
    {
        s_keysPressed.Remove(key);
    }

    #endregion

    #region Private static mehods

    private static void WriteToConsole(string message)
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

    #endregion

    #region Private static variables

    private static readonly HashSet<TKey> s_keysPressed = [];

    private static List<InputBindingRegistry<TKey>> s_registries = new();

    #endregion
}