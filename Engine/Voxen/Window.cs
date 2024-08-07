using InputSystem;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using ProfilingSystem;
using Voxen.Utilities.Inputs;

namespace Voxen;

// We can now move around objects. However, how can we move our "camera", or modify our perspective?
// In this tutorial, I'll show you how to setup a full projection/view/model (PVM) matrix.
// In addition, we'll make the rectangle rotate over time.
public class Window : GameWindow
{
    #region Constructors

    public Window() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        UpdateFrequency = 60.0;
        Title = "Voxen";
        WindowState = WindowState.Fullscreen;
        
        // TODO: remove (for debug)
        GeneralInputBindingRegistry.SaveCommand += () => Console.WriteLine("SaveCommand was called");
        GeneralInputBindingRegistry.SaveCommand += () => Console.WriteLine("SaveCommand was called (other handler)");
        
        // Input binding registries
        GeneralInputBindingRegistry generalInputBindingRegistry = new("General"); // TODO: merge with next line (for debug)
        InputSystem<Keys>.AddRegistry(generalInputBindingRegistry);
    }

    #endregion

    #region GameWindow overriden methods
    
    [Profile]
    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        // We enable depth testing here. If you try to draw something more complex than one plane without this,
        // you'll notice that polygons further in the background will occasionally be drawn over the top of the ones in the foreground.
        // Obviously, we don't want this, so we enable depth testing. We also clear the depth buffer in GL.Clear over in OnRenderFrame.
        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.Blend);

        // Set the blending function
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray((int)_vertexArrayObject);

        int vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        int elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _triangleIndices.Length * sizeof(uint), _triangleIndices, BufferUsageHint.StaticDraw);

        double[] bytes = [255, 127, 0, 127, 255, 0, 255, 0];
        int ssbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ShaderStorageBuffer, ssbo);
        GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 0, ssbo);
        GL.BufferData(BufferTarget.ShaderStorageBuffer, bytes.Length * sizeof(double), bytes, BufferUsageHint.StaticDraw);

        // shader.vert has been modified. Take a look at it after the explanation in OnRenderFrame.
        _shader = new Shader(
            "../../../Shaders/vshader.glsl",
            "../../../Shaders/fshader.glsl");
        _shader.Use();

        int vertexPositionLocation = _shader.GetAttribLocation("vertexPosition");
        GL.EnableVertexAttribArray(vertexPositionLocation);
        GL.VertexAttribPointer(vertexPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        
        int vertexColorLocation = _shader.GetAttribLocation("vertexColor");
        GL.EnableVertexAttribArray(vertexColorLocation);
        GL.VertexAttribPointer(vertexColorLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

        // Ox plane shader
        _oxPlaneVertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray((int)_oxPlaneVertexArrayObject);

        int oxPlaneVertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, oxPlaneVertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _oxPlaneVertices.Length * sizeof(float), _oxPlaneVertices, BufferUsageHint.StaticDraw);

        int oxPlaneElementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, oxPlaneElementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _oxPlaneTriangleIndices.Length * sizeof(uint), _oxPlaneTriangleIndices, BufferUsageHint.StaticDraw);

        _oxPlaneShader = new Shader(
            "../../../Shaders/vOxPlaneShader.glsl",
            "../../../Shaders/fOxPlaneShader.glsl");
        _oxPlaneShader.Use();
        
        int oxPlaneVertexPositionLocation = _oxPlaneShader.GetAttribLocation("vertexPosition");
        GL.EnableVertexAttribArray(oxPlaneVertexPositionLocation);
        GL.VertexAttribPointer(oxPlaneVertexPositionLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

        // We initialize the camera so that it is 3 units back from where the rectangle is.
        // We also give it the proper aspect ratio.
        _camera = new Camera(new Vector3(0.0f, 1.0f, 3.0f), Size.X / (float)Size.Y);

        // We make the mouse cursor invisible and captured so we can have proper FPS-camera movement.
        CursorState = CursorState.Grabbed;
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        if (_camera is null)
        {
            return;
        }

        // We clear the depth buffer in addition to the color buffer.
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        if (_vertexArrayObject is not null && _shader is not null)
        {
            GL.BindVertexArray((int)_vertexArrayObject);

            _shader.Use();
        
            _shader.SetMatrix4(CommonConstants.ShaderModelMatrixName, Matrix4.Identity);
            _shader.SetMatrix4(CommonConstants.ShaderViewMatrixName, _camera.GetViewMatrix());
            _shader.SetMatrix4(CommonConstants.ShaderProjectionMatrixName, _camera.GetProjectionMatrix());

            GL.DrawElements(PrimitiveType.Triangles, _triangleIndices.Length, DrawElementsType.UnsignedInt, 0);
        }

        if (_oxPlaneVertexArrayObject is not null && _oxPlaneShader is not null)
        {
            GL.BindVertexArray((int)_oxPlaneVertexArrayObject);
        
            _oxPlaneShader.Use();
        
            _oxPlaneShader.SetVector3("cameraPos", _camera.Position);
            _oxPlaneShader.SetMatrix4(CommonConstants.ShaderViewMatrixName, _camera.GetViewMatrix());
            _oxPlaneShader.SetMatrix4(CommonConstants.ShaderProjectionMatrixName, _camera.GetProjectionMatrix());
        
            GL.DrawElements(PrimitiveType.Triangles, _oxPlaneTriangleIndices.Length, DrawElementsType.UnsignedInt, 0);
        }

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        HandleKeyboardEvents((float)e.Time);
        HandleMouseEvents((float)e.Time);
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Size.X, Size.Y);
            
        // We need to update the aspect ratio once the window has been resized.
        if (_camera is not null)
        {
            _camera.AspectRatio = Size.X / (float)Size.Y;
        }
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);

        if (_camera is not null)
        {
            _camera.Fov -= e.OffsetY;
        }
    }

    protected override void OnKeyDown(KeyboardKeyEventArgs e)
    {
        base.OnKeyDown(e);
        
        InputSystem<Keys>.OnKeyDown(e.Key);
    }

    protected override void OnKeyUp(KeyboardKeyEventArgs e)
    {
        base.OnKeyUp(e);
        
        InputSystem<Keys>.OnKeyUp(e.Key);
    }

    #endregion

    #region Private methods

    private void HandleKeyboardEvents(float deltaTime)
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

        if (_camera is not null)
        {
            float cameraSpeed = 2.0f;
            
            if (KeyboardState.IsKeyDown(Keys.LeftShift))
            {
                cameraSpeed *= 2;
            }
            if (KeyboardState.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * cameraSpeed * deltaTime; // Forward
            }
            if (KeyboardState.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * cameraSpeed * deltaTime; // Backwards
            }
            if (KeyboardState.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * cameraSpeed * deltaTime; // Left
            }
            if (KeyboardState.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * cameraSpeed * deltaTime; // Right
            }
            if (KeyboardState.IsKeyDown(Keys.Space) || KeyboardState.IsKeyDown(Keys.E))
            {
                _camera.Position += _camera.Up * cameraSpeed * deltaTime; // Up
            }
            if (KeyboardState.IsKeyDown(Keys.LeftControl) || KeyboardState.IsKeyDown(Keys.Q))
            {
                _camera.Position -= _camera.Up * cameraSpeed * deltaTime; // Down
            }
        }
    }

    private void HandleMouseEvents(float deltaTime)
    {
        if (_camera is null)
        {
            return;
        }
        
        // Get the mouse state
        float sensitivity = 0.2f;

        if (_firstMove) // This bool variable is initially set to true.
        {
            _lastPos = new Vector2(MouseState.X, MouseState.Y);
            _firstMove = false;
        }
        else
        {
            // Calculate the offset of the mouse position
            var deltaX = MouseState.X - _lastPos.X;
            var deltaY = MouseState.Y - _lastPos.Y;
            _lastPos = new Vector2(MouseState.X, MouseState.Y);

            // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
            _camera.Yaw += deltaX * sensitivity;
            _camera.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
        }
    }

    #endregion

    #region Fields

    private int? _vertexArrayObject;
    private int? _oxPlaneVertexArrayObject;
    
    private bool _firstMove = true;
    private Vector2 _lastPos;

    private Shader? _shader;
    private Shader? _oxPlaneShader;
    private Camera? _camera;

    #endregion

    #region Constants

    private readonly float[] _vertices =
    [
        // Left rainbow quad
        -1.5f,  1.0f, 0.0f, 1.0f, 0.0f, 0.0f, // Top-right vertex
        -1.5f,  0.0f, 0.0f, 0.0f, 1.0f, 0.0f, // Bottom-right vertex
        -2.5f,  0.0f, 0.0f, 0.0f, 0.0f, 1.0f, // Bottom-left vertex
        -2.5f,  1.0f, 0.0f, 1.0f, 1.0f, 1.0f, // Top-left vertex
         
        // Middle rainbow quad
         0.5f,  1.0f, 0.0f, 1.0f, 0.0f, 0.0f, // Top-right vertex
         0.5f,  0.0f, 0.0f, 0.0f, 1.0f, 0.0f, // Bottom-right vertex
        -0.5f,  0.0f, 0.0f, 0.0f, 0.0f, 1.0f, // Bottom-left vertex
        -0.5f,  1.0f, 0.0f, 1.0f, 1.0f, 1.0f, // Top-left vertex
         
        // Right rainbow quad
         2.5f,  1.0f, 0.0f, 1.0f, 0.0f, 0.0f, // Top-right vertex
         2.5f,  0.0f, 0.0f, 0.0f, 1.0f, 0.0f, // Bottom-right vertex
         1.5f,  0.0f, 0.0f, 0.0f, 0.0f, 1.0f, // Bottom-left vertex
         1.5f,  1.0f, 0.0f, 1.0f, 1.0f, 1.0f, // Top-left vertex
    ];

    private readonly uint[] _triangleIndices =
    [
        // Left rainbow quad
        0, 1, 3, // Top-right triangle
        1, 2, 3, // Bottom-left triangle
        
        // Middle rainbow quad
        4, 5, 7, // Top-right triangle
        5, 6, 7, // Bottom-left triangle
        
        // Right rainbow quad
        8, 9, 11,  // Top-right triangle
        9, 10, 11, // Bottom-left triangle
    ];

    private readonly float[] _oxPlaneVertices =
    [
        -100.0f, 0.0f, -100.0f, // Bottom-left vertex
         100.0f, 0.0f, -100.0f, // Bottom-right vertex
         100.0f, 0.0f,  100.0f, // Top-right vertex
        -100.0f, 0.0f,  100.0f, // Top-left vertex
    ];

    private readonly uint[] _oxPlaneTriangleIndices =
    [
        0, 2, 1,
        0, 3, 2,
    ];

    #endregion
}