namespace GensouGL.Graphics;

public sealed class Screen : IScreen, IDisposable
{
    public static Screen Default { get; internal set; } = new Screen(0, 0, 0);

    private int _framebuffer;
    private Texture _texture;
    private int _width;
    private int _height;

    private bool _disposed = false;

    public int ScreenId => _framebuffer;
    public Texture Texture => _texture;
    public int Width => _width;
    public int Height => _height;

    internal Screen(int framebuffer, int width, int height)
    {
        _framebuffer = framebuffer;
        _width = width;
        _height = height;
        _texture = null;
    }

    public Screen(int width, int height)
    {
        if (width <= 0 || height <= 0)
        {
            throw new ArgumentException("Width and height need to be positiove integers.");
        }

        _width = width;
        _height = height;

        _texture = new Texture(width, height);

        _framebuffer = GL.GenFramebuffer();

        FramebufferService.PushAndBind(_framebuffer);
        TextureService.PushAndBind(_texture.TextureId);

        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, _texture.TextureId, 0);

        TextureService.Pop();
        FramebufferService.Pop();
    }

    public void MakeHDR()
    {
        if (_texture == null)
            throw new InvalidOperationException("Cannot make default screen HDR.");

        GL.TextureStorage2D(_texture.TextureId, 1, SizedInternalFormat.Rgba16f, _width, _height);
    }

    public void Clear(Color color)
    {
        FramebufferService.PushAndBind(_framebuffer);

        GL.ClearColor(color.R, color.G, color.B, color.A);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        FramebufferService.Pop();
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _texture?.Dispose();
            }

            if (_framebuffer != 0)
                GL.DeleteFramebuffer(_framebuffer);

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~Screen()
    {
        Dispose(false);
    }
}