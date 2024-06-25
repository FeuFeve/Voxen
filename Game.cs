using OpenTK.Graphics.ES20;
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

    protected override void OnLoad()
    {
        base.OnLoad();
        
        GL.ClearColor(0.2f, 0.2f, 0.3f, 1.0f);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        HandleKeyboardEvents();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        
        GL.Clear(ClearBufferMask.ColorBufferBit);
        
        
        
        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);
        
        GL.Viewport(0, 0, e.Width, e.Width);
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
        else if (KeyboardState.IsKeyPressed(Keys.F11))
        {
            WindowState = WindowState == WindowState.Fullscreen
                ? WindowState.Normal
                : WindowState.Fullscreen;
        }
    }

    #endregion
}