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

    public void SetUniformFloat(string name, float value)
    {
        GL.UseProgram(_program);
        int loc = GL.GetUniformLocation(_program, name);
        GL.Uniform1(loc, value);
    }

    public void SetUniformInt(string name, int value)
    {
        GL.UseProgram(_program);
        int loc = GL.GetUniformLocation(_program, name);
        GL.Uniform1(loc, value);
    }

    public void SetUniformVector2(string name, float x, float y)
    {
        GL.UseProgram(_program);
        int loc = GL.GetUniformLocation(_program, name);
        GL.Uniform2(loc, x, y);
    }

    public void SetUniformVector4(string name, float x, float y, float z, float w)
    {
        GL.UseProgram(_program);
        int loc = GL.GetUniformLocation(_program, name);
        GL.Uniform4(loc, new OpenTK.Mathematics.Vector4(x, y, z, w));
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
    private static Shader s_bullet;
    private static Shader s_brightPass;
    private static Shader s_bloomBlur;
    private static Shader s_blindfold;
    private static Shader s_simpleText;
    private static Shader s_outlinedText;
    private static Shader s_colorGrading;
    private static Shader s_colorInversion;
    private static Shader s_textureMask;

    internal static Shader SimpleShape => s_simpleShape;
    internal static Shader SimpleTexture => s_simpleTexture;
    internal static Shader Bullet => s_bullet;
    internal static Shader BrightPass => s_brightPass;
    internal static Shader BloomBlur => s_bloomBlur;
    internal static Shader Blindfold => s_blindfold;
    internal static Shader SimpleText => s_simpleText;
    internal static Shader OutlinedText => s_outlinedText;
    internal static Shader ColorGrading => s_colorGrading;
    internal static Shader ColorInversion => s_colorInversion;
    internal static Shader TextureMask => s_textureMask;

    internal static void LoadShaders()
    {
        Assembly asm = typeof(Shader).Assembly;
        s_simpleShape = Gen(asm, "SimpleShape");
        s_simpleTexture = Gen(asm, "SimpleTexture");
        s_bullet = Gen(asm, "Bullet");
        s_brightPass = Gen(asm, "BrightPass");
        s_bloomBlur = Gen(asm, "BrightPass", "BloomBlur");
        s_blindfold = Gen(asm, "BrightPass", "Blindfold");
        s_simpleText = Gen(asm, "SimpleText");
        s_colorGrading = Gen(asm, "BrightPass", "ColorGrading");
        s_colorInversion = Gen(asm, "BrightPass", "ColorInversion");
        s_textureMask = Gen(asm, "BrightPass", "TextureMask");
    }

    private static Shader Gen(Assembly asm, string vert, string frag = null)
    {
        using Stream vstream = asm.GetManifestResourceStream($"GensouGL.Graphics.Shaders.{vert}.vert");
        using Stream fstream = asm.GetManifestResourceStream(frag is null ? $"GensouGL.Graphics.Shaders.{vert}.frag" : $"GensouGL.Graphics.Shaders.{frag}.frag");

        using StreamReader vreader = new StreamReader(vstream);
        using StreamReader freader = new StreamReader(fstream);

        string vsource = vreader.ReadToEnd();
        string fsource = freader.ReadToEnd();

        return new Shader(vsource, fsource);
    }
}