namespace GensouGL.Graphics;

internal static class FullscreenQuadRenderer
{
    private static FullscreenQuadRendererBuffer _buffer;

    internal static void Initialize()
    {
        _buffer = new();
    }

    internal static void Release()
    {
        _buffer?.Dispose();
    }

    public static void Draw(Screen target, Texture texture)
    {
        TextureService.PushAndBind(texture.TextureId);
        FramebufferService.PushAndBind(target.ScreenId);

        GL.Disable(EnableCap.DepthTest);
        GL.Disable(EnableCap.CullFace);
        GL.Viewport(0, 0, target.Width, target.Height);

        _buffer.Draw();

        FramebufferService.Pop();
        TextureService.Pop();
    }
}

internal sealed class FullscreenQuadRendererBuffer : IDisposable
{
    private int _vao;
    private int _vbuf;
    private int _tbuf;

    public FullscreenQuadRendererBuffer()
    {
        _vao = GL.GenVertexArray();
        _vbuf = GL.GenBuffer();
        _tbuf = GL.GenBuffer();

        float[] va = [-1f, -1f,    -1f, 1f,    1f, -1f,    1f, 1f];
        float[] ta = [0f, 0f,    0f, 1f,    1f, 0f,    1f, 1f];

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbuf);
        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * va.Length, va, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _tbuf);
        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * ta.Length, ta, BufferUsageHint.StaticDraw);

        GL.BindVertexArray(_vao);

        GL.EnableVertexAttribArray(0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbuf);
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);

        GL.EnableVertexAttribArray(1);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _tbuf);
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
    }

    public void Draw()
    {
        GL.BindVertexArray(_vao);

        GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);

        GL.BindVertexArray(0);
    }

    public void Dispose()
    {
        if (_vao != 0) GL.DeleteVertexArray(_vao);
        if (_vbuf != 0) GL.DeleteBuffer(_vbuf);
        if (_tbuf != 0) GL.DeleteBuffer(_tbuf);
        GC.SuppressFinalize(this);
    }
}