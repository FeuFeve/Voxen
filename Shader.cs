using OpenTK.Graphics.OpenGL4;

namespace Voxen;

public class Shader
{
    #region Properties

    public int Handle { get; }
    
    #endregion
    
    #region Constructors

    public Shader(ShaderType shaderType, string path)
    {
        if (!File.Exists(path))
        {
            Console.Error.WriteLine($"Could not find shader file: '{path}'");
            return;
        }

        Handle = GL.CreateShader(shaderType);
        GL.ShaderSource(Handle, File.ReadAllText(path));
        GL.CompileShader(Handle);

        GL.GetShader(Handle, ShaderParameter.CompileStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetShaderInfoLog(Handle);
            Console.WriteLine(infoLog);
        }
    }

    #endregion
}