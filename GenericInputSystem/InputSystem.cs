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
            WriteToConsole($"Warning: trying to add an already registered {nameof(InputBindingRegistry<TKey>)}. Ignored this registry");
            Debug.Assert(false);
            return;
        }

        Type registryType = registry.GetType();
        EventInfo[] eventInfos = registryType.GetEvents(BindingFlags.Static | BindingFlags.Public);

        foreach (EventInfo eventInfo in eventInfos)
        {
            WriteToConsole(eventInfo);

            KeyBindingCommandAttribute<TKey>[] keyBindingCommandAttributes = eventInfo
                .GetCustomAttributes<KeyBindingCommandAttribute<TKey>>()
                .ToArray();

            if (keyBindingCommandAttributes.Length != 1)
            {
                WriteToConsole($"Event '{eventInfo}' has {keyBindingCommandAttributes.Length} associated {nameof(KeyBindingCommandAttribute<TKey>)} (expected 1)");
                Debug.Assert(false);
                continue;
            }
            
            KeyBindingCommandAttribute<TKey> keyBindingCommandAttribute = keyBindingCommandAttributes[0];
            WriteToConsole($"Bound to keys: {string.Join(", ", keyBindingCommandAttribute.Keys)}");

            // Find the backing field for this event
            FieldInfo? eventBackingFieldInfo = registryType.GetField(eventInfo.Name, BindingFlags.Static | BindingFlags.NonPublic);

            if (eventBackingFieldInfo is null)
            {
                WriteToConsole($"Could not find a backing field for '{eventInfo}'");
                Debug.Assert(false);
                break;
            }

            var eventDelegate = (Delegate?)eventBackingFieldInfo.GetValue(null);

            if (eventDelegate is null)
            {
                continue;
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