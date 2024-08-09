using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using InputSystem.Attributes;
using InputSystem.Data;

namespace InputSystem;

// TODO: keep as little code/data in this class as possible. InputSystem is the entry point: we want users trying to
// understand how it works to be lost in large amounts of code
public static class InputSystem<TKey> where TKey : Enum
{
    #region Public static methods

    public static void AddRegistry(InputBindingRegistry<TKey> inputBindingRegistry)
    {
        WriteToConsole($"Registering {nameof(InputBindingRegistry<TKey>)}: {inputBindingRegistry.Name}");

        if (!TryAddCommandRegistry(inputBindingRegistry, out CommandRegistry<TKey>? commandRegistry))
        {
            // We should not try to add an already existing registry
            return;
        }

        Type registryType = inputBindingRegistry.GetType();
        EventInfo[] eventInfos = registryType.GetEvents(BindingFlags.Static | BindingFlags.Public);

        foreach (EventInfo eventInfo in eventInfos)
        {
            RegisterCommand(eventInfo, commandRegistry);
        }
    }

    public static void OnKeyDown(TKey key)
    {
        if (s_keysPressed.Add(key))
        {
            CheckForCommands();
        }
    }

    public static void OnKeyUp(TKey key)
    {
        s_keysPressed.Remove(key);
    }

    #endregion

    #region Private static mehods

    // TODO: replace with a logger class that will use Serilog
    private static void WriteToConsole(object message)
    {
        Console.WriteLine($"{nameof(InputSystem<TKey>)}: {message}");
    }

    private static bool TryAddCommandRegistry(InputBindingRegistry<TKey> registry, [NotNullWhen(true)] out CommandRegistry<TKey>? commandRegistry)
    {
        if (s_commandRegistries.Any(existingCommandRegistry => existingCommandRegistry.InputBindingRegistry.Name == registry.Name))
        {
            WriteToConsole($"Warning: trying to add an already registered {nameof(InputBindingRegistry<TKey>)}. Ignored this registry");
            Debug.Assert(false);
            commandRegistry = null;
            return false;
        }

        commandRegistry = new CommandRegistry<TKey>
        {
            InputBindingRegistry = registry,
        };
        s_commandRegistries.Add(commandRegistry);

        return true;
    }

    private static void RegisterCommand(EventInfo eventInfo, CommandRegistry<TKey> commandRegistry)
    {
        WriteToConsole(eventInfo); // TODO: remove (debug)

        // Find the backing field for this event
        Type registryType = commandRegistry.InputBindingRegistry.GetType();
        FieldInfo? eventBackingFieldInfo = registryType.GetField(eventInfo.Name, BindingFlags.Static | BindingFlags.NonPublic);

        if (eventBackingFieldInfo is null)
        {
            WriteToConsole($"Could not find a backing field for '{eventInfo}'");
            Debug.Assert(false);
            return;
        }

        // Create the command, save the backing field info. The key combinations will be filled just after
        Command<TKey> command = new()
        {
            Name = eventInfo.Name,
            EventBackingFieldInfo = eventBackingFieldInfo,
        };

        // Find all the KeyBindingCommandAttribute attached to this event
        KeyBindingCommandAttribute<TKey>[] keyBindingCommandAttributes = eventInfo
            .GetCustomAttributes<KeyBindingCommandAttribute<TKey>>()
            .ToArray();

        if (keyBindingCommandAttributes.Length == 0)
        {
            WriteToConsole($"Event '{eventInfo}' has 0 associated {nameof(KeyBindingCommandAttribute<TKey>)} (expected at least 1)");
            Debug.Assert(false);
            return;
        }

        // For each KeyBindingCommandAttribute, add its keys to the command's key combinations
        foreach (KeyBindingCommandAttribute<TKey> keyBindingCommandAttribute in keyBindingCommandAttributes)
        {
            WriteToConsole($"Bound to keys: {string.Join(", ", keyBindingCommandAttribute.Keys)}"); // TODO: remove (debug)
            
            command.KeyCombinations.Add(keyBindingCommandAttribute.Keys);
        }
        
        commandRegistry.Commands.Add(command);
    }

    private static void CheckForCommands()
    {
        if (s_keysPressed.Count == 0)
        {
            return;
        }
        
        TKey[] keysPressed = s_keysPressed.ToArray();
        Array.Sort(keysPressed);

        foreach (CommandRegistry<TKey> commandRegistry in s_commandRegistries)
        {
            if (!commandRegistry.InputBindingRegistry.IsEnabled)
            {
                return;
            }

            foreach (Command<TKey> command in commandRegistry.Commands)
            {
                // If the currently pressed keys don't match any of the key combinations, the event should not be triggered
                if (!command.KeyCombinations.Any(keys => keys.SequenceEqual(keysPressed)))
                {
                    continue;
                }
                
                var eventDelegate = (Delegate?)command.EventBackingFieldInfo.GetValue(null);

                if (eventDelegate is null)
                {
                    return;
                }

                foreach (Delegate eventHandler in eventDelegate.GetInvocationList())
                {
                    eventHandler.DynamicInvoke();
                }

                break;
            }
        }
    }

    #endregion

    #region Private static variables

    private static readonly HashSet<TKey> s_keysPressed = [];

    private static readonly List<CommandRegistry<TKey>> s_commandRegistries = [];

    #endregion
}