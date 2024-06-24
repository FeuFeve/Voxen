using OpenTK.Graphics.ES20;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

int p = -1;

GameWindowSettings gameWindowSettings = new GameWindowSettings()
{
    UpdateFrequency = 60.0f,
};

NativeWindowSettings navNativeWindowSettings = new NativeWindowSettings()
{
    Title = "Voxen",
    WindowState = WindowState.Maximized
};

GameWindow gameWindow = new(gameWindowSettings, navNativeWindowSettings);

gameWindow.Load += () =>
{
    Console.WriteLine("Loaded");
    
    p = CreateShaders();
    GL.UseProgram(p);
};

gameWindow.Unload += () =>
{
    GL.DeleteProgram(p);
};

gameWindow.Run();

int CreateShaders()
{
    int vShader = GL.CreateShader(ShaderType.VertexShader);
    int fShader = GL.CreateShader(ShaderType.FragmentShader);
    
    GL.ShaderSource(vShader, File.ReadAllText("../../../Shaders/vshader.glsl"));
    GL.ShaderSource(fShader, File.ReadAllText("../../../Shaders/fshader.glsl"));
    
    GL.CompileShader(vShader);
    GL.CompileShader(fShader);
    
    Console.WriteLine(GL.GetShaderInfoLog(vShader));
    Console.WriteLine(GL.GetShaderInfoLog(fShader));

    int program = GL.CreateProgram();
    GL.AttachShader(program, vShader);
    GL.AttachShader(program, fShader);
    GL.DetachShader(program, vShader);
    GL.DetachShader(program, fShader);
    GL.DeleteShader(vShader);
    GL.DeleteShader(fShader);
    
    Console.WriteLine(GL.GetProgramInfoLog(program));
    
    return program;
}