using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Voxen;

public class Game : GameWindow
{
    #region Constructors
    
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

        Shader vertexShader = new(ShaderType.VertexShader, "../../../Shaders/vshader.glsl");
        Shader fragmentShader = new(ShaderType.FragmentShader, "../../../Shaders/fshader.glsl");
        _program = new GlProgram(vertexShader, fragmentShader);
    }

    protected override void OnUnload()
    {
        base.OnUnload();

        _program?.Dispose();
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

        if (_program is not null)
        {
            _program.Draw();
        }
        
        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);
        
        GL.Viewport(0, 0, e.Width, e.Height);
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

    #region Fields

    private GlProgram? _program = null;

    #endregion
}