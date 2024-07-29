using System.Reflection;
using GenericInputSystem.Attributes;

namespace GenericInputSystem;

public static class InputSystem<TKey> where TKey : Enum
{
    #region Public static methods

    public static void Register(InputBindingRegistry<TKey> registry)
    {
        Console.WriteLine($"Registering new {nameof(InputBindingRegistry<TKey>)}: {registry.Name}");
        
        EventInfo[] commands = registry.GetType().GetEvents(BindingFlags.Static | BindingFlags.Public);

        foreach (EventInfo command in commands)
        {
            Console.WriteLine(command);

            object[] attributes = command.GetCustomAttributes(typeof(KeyBindingCommandAttribute<TKey>), false);

            if (attributes.Length != 1)
            {
                Console.WriteLine($"Command '{command}' does not have an associated {nameof(KeyBindingCommandAttribute<TKey>)}");
                break;
            }

            KeyBindingCommandAttribute<TKey> keyBindingCommandAttribute = (KeyBindingCommandAttribute<TKey>)attributes[0];

            Console.WriteLine($"Bound to keys: {string.Join(", ", keyBindingCommandAttribute.Keys)}");
        }
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

    #region Private static variables

    private static readonly HashSet<TKey> s_keysPressed = [];

    #endregion
}