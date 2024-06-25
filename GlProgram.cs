using OpenTK.Graphics.ES20;

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

    public void Use()
    {
        if (_handle is null)
        {
            Console.Error.WriteLine($"Can't use this program: handle is null");
            return;
        }
        
        GL.UseProgram((int)_handle);
    }

    #endregion

    #region Fields

    private readonly int? _handle = null;

    #endregion
}