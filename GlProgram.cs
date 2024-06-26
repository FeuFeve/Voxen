using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Voxen;

public class GlProgram : IDisposable
{
    #region Constructors

    public GlProgram(params Shader[] shaders)
    {
        if (shaders.Length == 0)
        {
            Console.Error.WriteLine($"No shader passed when creating {nameof(GlProgram)}");
            return;
        }

        _handle = GL.CreateProgram();
        var handle = (int)_handle;

        foreach (Shader shader in shaders)
        {
            GL.AttachShader(handle, shader.Handle);
        }
        
        GL.LinkProgram(handle);
        
        GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetProgramInfoLog(handle);
            Console.WriteLine(infoLog);
        }
        
        foreach (Shader shader in shaders)
        {
            GL.DetachShader(handle, shader.Handle);
            GL.DeleteShader(shader.Handle);
        }
        
        // VAO
        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray((int)_vertexArrayObject);
        
        // VBO
        int vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        // VAO indexes
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);
        
        // EBO
        int elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _triangleIndices.Length * sizeof(uint), _triangleIndices, BufferUsageHint.StaticDraw);
    }

    #endregion

    #region Destructors

    ~GlProgram()
    {
        Dispose();
    }

    #endregion

    #region IDisposable implementation

    public void Dispose()
    {
        if (_handle is null)
        {
            return;
        }
        
        GL.DeleteProgram((int)_handle);
        
        GC.SuppressFinalize(this);
    }

    #endregion

    #region Public methods

    public void Draw(int viewportWidth, int viewportHeight)
    {
        if (_handle is null || _vertexArrayObject is null)
        {
            Console.Error.WriteLine($"Can't use this program: handle is null");
            return;
        }
        
        GL.UseProgram((int)_handle);
        
        // Uniforms
        Matrix4 model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-55.0f));
        Matrix4 view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
        Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)viewportWidth / viewportHeight, 0.1f, 100.0f);
        
        string modelName = GL.GetActiveUniform((int)_handle, 0, out _, out _);
        string viewName = GL.GetActiveUniform((int)_handle, 1, out _, out _);
        string projectionName = GL.GetActiveUniform((int)_handle, 2, out _, out _);
        
        int modelLocation = GL.GetUniformLocation((int)_handle, modelName);
        int viewLocation = GL.GetUniformLocation((int)_handle, viewName);
        int projectionLocation = GL.GetUniformLocation((int)_handle, projectionName);
        
        GL.UniformMatrix4(modelLocation, true, ref model);
        GL.UniformMatrix4(viewLocation, true, ref view);
        GL.UniformMatrix4(projectionLocation, true, ref projection);
        
        GL.BindVertexArray((int)_vertexArrayObject);
        GL.DrawElements(PrimitiveType.Triangles, _triangleIndices.Length, DrawElementsType.UnsignedInt, 0);
    }

    #endregion

    #region Fields

    private readonly int? _handle;
    private readonly int? _vertexArrayObject;
        
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