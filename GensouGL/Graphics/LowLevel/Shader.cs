using System.Reflection;

namespace GensouGL.Graphics;

public sealed class Shader : IDisposable
{
    private string _vertexShaderSource;
    private string _fragmentShaderSource;
    private int _program;

    public string VertexShaderSource => _vertexShaderSource;
    public string FragmentShaderSource => _fragmentShaderSource;

    public Shader(string vertexShaderSource, string fragmentShaderSource)
    {
        _vertexShaderSource = vertexShaderSource;
        _fragmentShaderSource = fragmentShaderSource;
    
        // compile shaders
        int vshader = GL.CreateShader(ShaderType.VertexShader);
        int fshader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(vshader, vertexShaderSource);
        GL.ShaderSource(fshader, fragmentShaderSource);

        GL.CompileShader(vshader);
        GL.CompileShader(fshader);

        GL.GetShader(vshader, ShaderParameter.CompileStatus, out int vstatus);
        if (vstatus != (int)All.True)
        {
            string infoLog = GL.GetShaderInfoLog(vshader);
            throw new Exception($"Vertex shader compilation failed: {infoLog}");
        }

        GL.GetShader(fshader, ShaderParameter.CompileStatus, out int fstatus);

        if (fstatus != (int)All.True)
        {
            string infoLog = GL.GetShaderInfoLog(fshader);
            throw new Exception($"Fragment shader compilation failed: {infoLog}");
        }

        _program = GL.CreateProgram();

        GL.AttachShader(_program, vshader);
        GL.AttachShader(_program, fshader);

        GL.LinkProgram(_program);

        GL.DetachShader(_program, vshader);
        GL.DetachShader(_program, fshader);

        GL.DeleteShader(vshader);
        GL.DeleteShader(fshader);
    }

    public void Bind()
    {
        GL.UseProgram(_program);
    }

    public void Unbind()
    {
        GL.UseProgram(0);
    }

    internal static void SendMatrix0(OpenTK.Mathematics.Matrix4 matrix)
    {
        GL.UniformMatrix4(0, false, ref matrix);
    }

    private void Dispose(bool disposing)
    {
        GL.DeleteProgram(_program);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~Shader()
    {
        Dispose(false);
    }





    private static Shader s_simpleShape;
    private static Shader s_simpleTexture;

    internal static Shader SimpleShape => s_simpleShape;
    internal static Shader SimpleTexture => s_simpleTexture;

    internal static void LoadShaders()
    {
        Assembly asm = typeof(Shader).Assembly;
        s_simpleShape = Gen(asm, "SimpleShape");
        s_simpleTexture = Gen(asm, "SimpleTexture");
    }

    private static Shader Gen(Assembly asm, string name)
    {
        using Stream vstream = asm.GetManifestResourceStream($"GensouGL.Graphics.Shaders.{name}.vert");
        using Stream fstream = asm.GetManifestResourceStream($"GensouGL.Graphics.Shaders.{name}.frag");

        using StreamReader vreader = new StreamReader(vstream);
        using StreamReader freader = new StreamReader(fstream);

        string vsource = vreader.ReadToEnd();
        string fsource = freader.ReadToEnd();

        return new Shader(vsource, fsource);
    }
}