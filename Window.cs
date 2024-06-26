using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;

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
    }

    #endregion

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        // We enable depth testing here. If you try to draw something more complex than one plane without this,
        // you'll notice that polygons further in the background will occasionally be drawn over the top of the ones in the foreground.
        // Obviously, we don't want this, so we enable depth testing. We also clear the depth buffer in GL.Clear over in OnRenderFrame.
        GL.Enable(EnableCap.DepthTest);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray((int)_vertexArrayObject);

        int vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        int elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _triangleIndices.Length * sizeof(uint), _triangleIndices, BufferUsageHint.StaticDraw);

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

        // For the view, we don't do too much here. Next tutorial will be all about a Camera class that will make it much easier to manipulate the view.
        // For now, we move it backwards three units on the Z axis.
        _view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);

        // For the matrix, we use a few parameters.
        //   Field of view. This determines how much the viewport can see at once. 45 is considered the most "realistic" setting, but most video games nowadays use 90
        //   Aspect ratio. This should be set to Width / Height.
        //   Near-clipping. Any vertices closer to the camera than this value will be clipped.
        //   Far-clipping. Any vertices farther away from the camera than this value will be clipped.
        _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Size.X / (float)Size.Y, 0.1f, 100.0f);

        // Now, head over to OnRenderFrame to see how we setup the model matrix.
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        if (_vertexArrayObject is null || _shader is null)
        {
            return;
        }

        // We add the time elapsed since last frame, times 4.0 to speed up animation, to the total amount of time passed.
        _time += 20.0 * e.Time;

        // We clear the depth buffer in addition to the color buffer.
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.BindVertexArray((int)_vertexArrayObject);

        _shader.Use();

        // Finally, we have the model matrix. This determines the position of the model.
        Matrix4 model = Matrix4.Identity
                        * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(_time))
                        * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(_time));

        // Then, we pass all of these matrices to the vertex shader.
        // You could also multiply them here and then pass, which is faster, but having the separate matrices available is used for some advanced effects.

        // IMPORTANT: OpenTK's matrix types are transposed from what OpenGL would expect - rows and columns are reversed.
        // They are then transposed properly when passed to the shader. 
        // This means that we retain the same multiplication order in both OpenTK c# code and GLSL shader code.
        // If you pass the individual matrices to the shader and multiply there, you have to do in the order "model * view * projection".
        // You can think like this: first apply the modelToWorld (aka model) matrix, then apply the worldToView (aka view) matrix, 
        // and finally apply the viewToProjectedSpace (aka projection) matrix.
        _shader.SetMatrix4("model", model);
        _shader.SetMatrix4("view", _view);
        _shader.SetMatrix4("projection", _projection);

        GL.DrawElements(PrimitiveType.Triangles, _triangleIndices.Length, DrawElementsType.UnsignedInt, 0);

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        KeyboardState? input = KeyboardState;

        if (input.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Size.X, Size.Y);
    }

    #region Fields

    private int? _vertexArrayObject;

    private Shader? _shader;

    // We create a double to hold how long has passed since the program was opened.
    private double _time;

    // Then, we create two matrices to hold our view and projection. They're initialized at the bottom of OnLoad.
    // The view matrix is what you might consider the "camera". It represents the current viewport in the window.
    private Matrix4 _view;

    // This represents how the vertices will be projected. It's hard to explain through comments,
    // so check out the web version for a good demonstration of what this does.
    private Matrix4 _projection;

    #endregion

    #region Constants

    private readonly float[] _vertices =
    [
         0.5f,  0.5f, 0.0f, 1.0f, 0.0f, 0.0f, // Top-right vertex
         0.5f, -0.5f, 0.0f, 0.0f, 1.0f, 0.0f, // Bottom-right vertex
        -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, 1.0f, // Bottom-left vertex
        -0.5f,  0.5f, 0.0f, 1.0f, 1.0f, 1.0f, // Top-left vertex
    ];

    private readonly uint[] _triangleIndices =
    [
        0, 1, 3, // Top-right triangle
        1, 2, 3, // Bottom-left triangle
    ];

    #endregion
}