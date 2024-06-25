using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Voxen;

public class Game : GameWindow
{
    #region Constructor
    
    public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        UpdateFrequency = 60.0;
        Title = "Voxen";
        WindowState = WindowState.Fullscreen;
    }

    #endregion

    #region GameWindow overriden methods

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        HandleKeyboardEvents();
    }

    #endregion

    #region Private methods

    private void HandleKeyboardEvents()
    {
        if (!KeyboardState.IsAnyKeyDown)
        {
            return;
        }

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }

    #endregion
}