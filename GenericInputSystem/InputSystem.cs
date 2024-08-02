using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using GenericInputSystem.Attributes;
using GenericInputSystem.Data;

namespace GenericInputSystem;

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

        // TODO: check for inputs
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

        KeyBindingCommandAttribute<TKey>[] keyBindingCommandAttributes = eventInfo
            .GetCustomAttributes<KeyBindingCommandAttribute<TKey>>()
            .ToArray();

        if (keyBindingCommandAttributes.Length != 1)
        {
            WriteToConsole($"Event '{eventInfo}' has {keyBindingCommandAttributes.Length} associated {nameof(KeyBindingCommandAttribute<TKey>)} (expected 1)");
            Debug.Assert(false);
            return;
        }

        KeyBindingCommandAttribute<TKey> keyBindingCommandAttribute = keyBindingCommandAttributes[0];
        WriteToConsole($"Bound to keys: {string.Join(", ", keyBindingCommandAttribute.Keys)}"); // TODO: remove (debug)

        // Find the backing field for this event
        Type registryType = commandRegistry.InputBindingRegistry.GetType();
        FieldInfo? eventBackingFieldInfo = registryType.GetField(eventInfo.Name, BindingFlags.Static | BindingFlags.NonPublic);

        if (eventBackingFieldInfo is null)
        {
            WriteToConsole($"Could not find a backing field for '{eventInfo}'");
            Debug.Assert(false);
            return;
        }

        commandRegistry.Commands.Add(
            new Command<TKey>
            {
                Name = eventInfo.Name,
                Keys = keyBindingCommandAttribute.Keys,
                EventBackingFieldInfo = eventBackingFieldInfo,
            }
        );

        // TODO: remove (debug)
        var eventDelegate = (Delegate?)eventBackingFieldInfo.GetValue(null);

        if (eventDelegate is null)
        {
            return;
        }

        foreach (Delegate handler in eventDelegate.GetInvocationList())
        {
            WriteToConsole($"Delegate: {handler.Method.Name}");
        }

        // TODO: store the registry type, the keys bound to the attribute, and the event delegate, so we can ...
        // check and invoke the various delegate bound to the event
        // Note: verify that storing the delegate is ok: if we store it and someone attaches itself to the event
        // afterwards, does the delegate "invocation list" reflects these changes?
        // If not: we might need to store the event backing field info (not even sure about that)
    }

    private static void CheckForCommands()
    {
        TKey[] keysPressed = s_keysPressed.ToArray();
        Array.Sort(keysPressed);

        // TODO: definitely move that into its own logic-related class
    }

    #endregion

    #region Private static variables

    private static readonly HashSet<TKey> s_keysPressed = [];

    private static readonly List<CommandRegistry<TKey>> s_commandRegistries = [];

    #endregion
}