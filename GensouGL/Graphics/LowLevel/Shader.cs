using System.Reflection;

namespace GensouGL.Graphics;

/// <summary>
/// Represents a shader program.
/// </summary>
public sealed class Shader : IDisposable
{
    private string _vertexShaderSource;
    private string _fragmentShaderSource;
    private int _program;

    /// <summary>
    /// Gets the source code of the vertex shader.
    /// </summary>
    public string VertexShaderSource => _vertexShaderSource;
    /// <summary>
    /// Gets the source code of the fragment shader.
    /// </summary>
    public string FragmentShaderSource => _fragmentShaderSource;
    /// <summary>
    /// Gets the OpenGL shader program id.
    /// </summary>
    public int ProgramId => _program;

    /// <summary>
    /// Initializes a new instance of <see cref="Shader"/> with the vertex and fragment shader sources.
    /// </summary>
    /// <param name="vertexShaderSource">Vertex shader source code.</param>
    /// <param name="fragmentShaderSource">Fragment shader source code.</param>
    /// <exception cref="Exception">Thrown when an compile error occurred.</exception>
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

    /// <summary>
    /// Binds the shader program.
    /// </summary>
    public void Bind()
    {
        GL.UseProgram(_program);
    }

    /// <summary>
    /// Unbind the shader program.
    /// </summary>
    public void Unbind()
    {
        GL.UseProgram(0);
    }

    /// <summary>
    /// Sets the specified uniform value.
    /// </summary>
    /// <param name="name">Name of the uniform.</param>
    /// <param name="value">Value.</param>
    public void SetUniformFloat(string name, float value)
    {
        GL.UseProgram(_program);
        int loc = GL.GetUniformLocation(_program, name);
        GL.Uniform1(loc, value);
    }

    /// <summary>
    /// Sets the specified uniform value.
    /// </summary>
    /// <param name="name">Name of the uniform.</param>
    /// <param name="value">Value.</param>
    public void SetUniformInt(string name, int value)
    {
        GL.UseProgram(_program);
        int loc = GL.GetUniformLocation(_program, name);
        GL.Uniform1(loc, value);
    }

    /// <summary>
    /// Sets the specified uniform value.
    /// </summary>
    /// <param name="name">Name of the uniform.</param>
    /// <param name="x">X component.</param>
    /// <param name="y">Y component.</param>
    public void SetUniformVector2(string name, float x, float y)
    {
        GL.UseProgram(_program);
        int loc = GL.GetUniformLocation(_program, name);
        GL.Uniform2(loc, x, y);
    }

    /// <summary>
    /// Sets the specified uniform value.
    /// </summary>
    /// <param name="name">Name of the uniform.</param>
    /// <param name="x">X component.</param>
    /// <param name="y">Y component.</param>
    /// <param name="z">Z component.</param>
    public void SetUniformVector3(string name, float x, float y, float z)
    {
        GL.UseProgram(_program);
        int loc = GL.GetUniformLocation(_program, name);
        GL.Uniform3(loc, new OpenTK.Mathematics.Vector3(x, y, z));
    }

    /// <summary>
    /// Sets the specified uniform value.
    /// </summary>
    /// <param name="name">Name of the uniform.</param>
    /// <param name="x">X component.</param>
    /// <param name="y">Y component.</param>
    /// <param name="z">Z component.</param>
    /// <param name="w">W component.</param>
    public void SetUniformVector4(string name, float x, float y, float z, float w)
    {
        GL.UseProgram(_program);
        int loc = GL.GetUniformLocation(_program, name);
        GL.Uniform4(loc, new OpenTK.Mathematics.Vector4(x, y, z, w));
    }

    internal void SetUniformMatrix4(string name, bool transpose, OpenTK.Mathematics.Matrix4 matrix)
    {
        GL.UseProgram(_program);
        int loc = GL.GetUniformLocation(_program, name);
        GL.UniformMatrix4(loc, false, ref matrix);
    }

    internal static void SendMatrix0(OpenTK.Mathematics.Matrix4 matrix)
    {
        GL.UniformMatrix4(0, false, ref matrix);
    }

    internal static void SendMatrix012(OpenTK.Mathematics.Matrix4 matrix0, OpenTK.Mathematics.Matrix4 matrix1, OpenTK.Mathematics.Matrix4 matrix2)
    {
        GL.UniformMatrix4(0, false, ref matrix0);
        GL.UniformMatrix4(1, false, ref matrix1);
        GL.UniformMatrix4(2, false, ref matrix2);
    }

    private void Dispose(bool disposing)
    {
        GL.DeleteProgram(_program);
    }

    /// <summary>
    /// Releases all resources used by <see cref="Shader"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// <see cref="Shader"/> destructor.
    /// </summary>
    ~Shader()
    {
        Dispose(false);
    }





    // 2D
    private static Shader s_simpleShape;
    private static Shader s_simpleTexture;
    private static Shader s_bullet;
    private static Shader s_brightPass;
    private static Shader s_bloomBlur;
    private static Shader s_bloomAdd;
    private static Shader s_blindfold;
    private static Shader s_simpleText;
    private static Shader s_outlinedText;
    private static Shader s_colorGrading;
    private static Shader s_colorInversion;
    private static Shader s_textureMask;
    private static Shader s_depthDebug;


    // 3D
    private static Shader s_simpleShape3D;
    private static Shader s_simpleShape3DLighting;
    private static Shader s_simpleShape3DLightingShadow;
    private static Shader s_depth3D;
    private static Shader s_depthTexture3D;
    private static Shader s_texture3D;
    private static Shader s_texture3DLighting;
    private static Shader s_texture3DLightingShadow;
    private static Shader s_cubemapSkybox;


    // 2D
    internal static Shader SimpleShape => s_simpleShape;
    internal static Shader SimpleTexture => s_simpleTexture;
    internal static Shader Bullet => s_bullet;
    internal static Shader BrightPass => s_brightPass;
    internal static Shader BloomBlur => s_bloomBlur;
    internal static Shader BloomAdd => s_bloomAdd;
    internal static Shader Blindfold => s_blindfold;
    internal static Shader SimpleText => s_simpleText;
    internal static Shader OutlinedText => s_outlinedText;
    internal static Shader ColorGrading => s_colorGrading;
    internal static Shader ColorInversion => s_colorInversion;
    internal static Shader TextureMask => s_textureMask;
    internal static Shader DepthDebug => s_depthDebug;


    // 3D
    internal static Shader SimpleShape3D => s_simpleShape3D;
    internal static Shader SimpleShape3DLighting => s_simpleShape3DLighting;
    internal static Shader SimpleShape3DLightingShadow => s_simpleShape3DLightingShadow;
    internal static Shader Depth3D => s_depth3D;
    internal static Shader DepthTexture3D => s_depthTexture3D;
    internal static Shader Texture3D => s_texture3D;
    internal static Shader Texture3DLighting => s_texture3DLighting;
    internal static Shader Texture3DLightingShadow => s_texture3DLightingShadow;
    internal static Shader CubemapSkybox => s_cubemapSkybox;

    internal static void LoadShaders()
    {
        Assembly asm = typeof(Shader).Assembly;
        ShaderPreprocessor.SetSourceAssembly(asm);

        s_simpleShape = Gen2D(asm, "SimpleShape");
        s_simpleTexture = Gen2D(asm, "SimpleTexture");
        s_bullet = Gen2D(asm, "Bullet");
        s_brightPass = Gen2D(asm, "BrightPass");
        s_bloomBlur = Gen2D(asm, "BrightPass", "BloomBlur");
        s_bloomAdd = Gen2D(asm, "BrightPass", "BloomAdd");
        s_blindfold = Gen2D(asm, "BrightPass", "Blindfold");
        s_simpleText = Gen2D(asm, "SimpleText");
        s_outlinedText = Gen2D(asm, "OutlinedText");
        s_colorGrading = Gen2D(asm, "BrightPass", "ColorGrading");
        s_colorInversion = Gen2D(asm, "BrightPass", "ColorInversion");
        s_textureMask = Gen2D(asm, "BrightPass", "TextureMask");
        s_depthDebug = Gen2D(asm, "SimpleTexture", "DepthDebug");


        s_simpleShape3D = Gen3D(asm, "SimpleShape");
        s_simpleShape3DLighting = Gen3D(asm, "Lighting_SimpleShape");
        s_simpleShape3DLightingShadow = Gen3D(asm, "Lighting_Shadow_SimpleShape");
        s_depth3D = Gen3D(asm, "Lighting_SimpleShape", "Depth");
        s_depthTexture3D = Gen3D(asm, "Depth_Texture");
        s_texture3D = Gen3D(asm, "Texture");
        s_texture3DLighting = Gen3D(asm, "Lighting_Texture");
        s_texture3DLightingShadow = Gen3D(asm, "Lighting_Shadow_Texture");
        s_cubemapSkybox = Gen3D(asm, "CubemapSkybox");
    }

    private static Shader Gen2D(Assembly asm, string vert, string frag = null)
    {
        using Stream vstream = asm.GetManifestResourceStream($"GensouGL.Graphics.Shaders.Shader2D.{vert}.vert");
        using Stream fstream = asm.GetManifestResourceStream(frag is null ? $"GensouGL.Graphics.Shaders.Shader2D.{vert}.frag" : $"GensouGL.Graphics.Shaders.Shader2D.{frag}.frag");

        using StreamReader vreader = new StreamReader(vstream);
        using StreamReader freader = new StreamReader(fstream);

        string vsource = vreader.ReadToEnd();
        string fsource = freader.ReadToEnd();

        vsource = ShaderPreprocessor.ExpandInclude(vsource);
        fsource = ShaderPreprocessor.ExpandInclude(fsource);

        return new Shader(vsource, fsource);
    }

    private static Shader Gen3D(Assembly asm, string vert, string frag = null)
    {
        using Stream vstream = asm.GetManifestResourceStream($"GensouGL.Graphics.Shaders.Shader3D.{vert}.vert");
        using Stream fstream = asm.GetManifestResourceStream(frag is null ? $"GensouGL.Graphics.Shaders.Shader3D.{vert}.frag" : $"GensouGL.Graphics.Shaders.Shader3D.{frag}.frag");

        using StreamReader vreader = new StreamReader(vstream);
        using StreamReader freader = new StreamReader(fstream);

        string vsource = vreader.ReadToEnd();
        string fsource = freader.ReadToEnd();

        vsource = ShaderPreprocessor.ExpandInclude(vsource);
        fsource = ShaderPreprocessor.ExpandInclude(fsource);

        return new Shader(vsource, fsource);
    }
}
