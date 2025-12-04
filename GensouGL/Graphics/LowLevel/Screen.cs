namespace GensouGL.Graphics;

public sealed class Screen : IScreen, IDisposable
{
    public static Screen Default { get; } = new(0);


    private int _framebuffer;
    private Texture _texture;
    private int _width;
    private int _height;

    private bool _disposed = false;

    public int ScreenId => _framebuffer;
    public Texture Texture => _texture;
    public int Width => _width;
    public int Height => _height;

    private Screen(int framebuffer)
    {
        _framebuffer = framebuffer;
        _width = 0;
        _height = 0;
        _texture = null;
    }

    public Screen(int width, int height)
    {
        _width = width;
        _height = height;

        _framebuffer = GL.GenFramebuffer();

        FramebufferService.PushAndBind(_framebuffer);

        // TODO: Implement...

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