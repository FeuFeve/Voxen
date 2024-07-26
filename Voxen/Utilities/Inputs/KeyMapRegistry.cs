using System.Reflection;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Voxen.Utilities.Inputs;

public class KeyMapRegistry
{
    #region Singleton

    public static KeyMapRegistry Instance { get; } = new();

    private KeyMapRegistry()
    {
        Initialize();
    }

    #endregion

    #region Public methods

    public void Initialize()
    {
        Console.WriteLine("Hello world");
        EventInfo[] commands = typeof(GeneralInputSystem).GetEvents(BindingFlags.Static | BindingFlags.Public);

        foreach (EventInfo command in commands)
        {
            Console.WriteLine(command);

            object[] attributes = command.GetCustomAttributes(false);

            foreach (object attribute in attributes)
            {
                if (attribute is KeyBindingCommandAttribute keyMapAttribute)
                {
                    Console.WriteLine($"Bound to keys: {string.Join(", ", keyMapAttribute.Keys)}");

                    foreach (Keys key in keyMapAttribute.Keys)
                    {
                        Console.WriteLine(key);
                    }
                }
            }
        }
    }

    // public static void RegisterKeyMap(object target)
    // {
    //     var events = target.GetType().GetEvents(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
    //
    //     foreach (var eventInfo in events)
    //     {
    //         var attribute = eventInfo.GetCustomAttribute<KeyMapAttribute>();
    //         if (attribute != null)
    //         {
    //             var keyCombination = attribute.Keys;
    //             if (Commands.ContainsKey(keyCombination))
    //             {
    //                 throw new InvalidOperationException($"Duplicate key combination: {keyCombination.Key} + {keyCombination.Modifiers}");
    //             }
    //
    //             var handler = (EventHandler)Delegate.CreateDelegate(typeof(EventHandler), target, eventInfo.Name);
    //             Commands[keyCombination] = handler;
    //         }
    //     }
    // }

    #endregion

    #region Private variables

    // private static readonly Dictionary<KM, EventHandler> Commands = new();

    #endregion
}