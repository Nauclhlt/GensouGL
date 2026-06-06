namespace GensouGL.Graphics;

/// <summary>
/// Represents a screen(render target).
/// </summary>
public sealed class Screen : IScreen, IDisposable
{
    /// <summary>
    /// Gets the default screen.
    /// </summary>
    /// <remarks>When you use the default screen, that means you are drawing directly on the game window buffer.</remarks>
    public static Screen Default { get; internal set; } = new Screen(0, 0, 0, 0, 0);

    private int _framebuffer;
    private Texture _texture;
    private int _width;
    private int _height;
    private int _projectionWidth;
    private int _projectionHeight;
    private int _depthTexture = 0;

    private bool _disposed = false;

    /// <summary>
    /// Gets the screen id.
    /// </summary>
    public int ScreenId => _framebuffer;
    /// <summary>
    /// Gets the texture associated with the screen.
    /// </summary>
    public Texture Texture => _texture;
    /// <summary>
    /// Gets the width of the screen.
    /// </summary>
    public int Width => _width;
    /// <summary>
    /// Gets the height of the screen.
    /// </summary>
    public int Height => _height;
    /// <summary>
    /// Gets the width value used to make projection matrices.
    /// </summary>
    public int ProjectionWidth => _projectionWidth;
    /// <summary>
    /// Gets the height value used to make projection matrices.
    /// </summary>
    public int ProjectionHeight => _projectionHeight;

    internal Screen(int framebuffer, int width, int height, int projectionWidth, int projectionHeight)
    {
        _framebuffer = framebuffer;
        _width = width;
        _height = height;
        _projectionWidth = projectionWidth;
        _projectionHeight = projectionHeight;
        _texture = null;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Screen"/> with the size.
    /// </summary>
    /// <param name="width">Width of the screen.</param>
    /// <param name="height">Height of the screen.</param>
    /// <exception cref="ArgumentException">Thrown when the size is invalid.</exception>
    public Screen(int width, int height)
    {
        if (width <= 0 || height <= 0)
        {
            throw new ArgumentException("Width and height need to be positiove integers.");
        }

        _width = width;
        _height = height;
        _projectionWidth = width;
        _projectionHeight = height;

        _texture = new Texture(width, height);

        _framebuffer = GL.GenFramebuffer();

        FramebufferService.PushAndBind(_framebuffer);
        TextureService.PushAndBind(_texture.TextureId);

        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, _texture.TextureId, 0);

        TextureService.Pop();
        FramebufferService.Pop();
    }

    /// <summary>
    /// Clears the screen with the specified color.
    /// </summary>
    /// <param name="color">Clear color.</param>
    public void Clear(Color color)
    {
        FramebufferService.PushAndBind(_framebuffer);

        GL.ClearColor(color.R, color.G, color.B, color.A);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        FramebufferService.Pop();
    }

    /// <summary>
    /// Prepares the depth buffer.
    /// </summary>
    public void UseDepth()
    {
        if (_depthTexture != 0) return;

        _depthTexture = GL.GenTexture();
        TextureService.PushAndBind(_depthTexture);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent24, _width, _height, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);

        FramebufferService.PushAndBind(_framebuffer);

        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, _depthTexture, 0);

        TextureService.Pop();
        FramebufferService.Pop();
    }

    internal void __SetSize(int width, int height)
    {
        if (_framebuffer != 0) return;
        _width = width;
        _height = height;
    }

    internal void __SetProjSize(int width, int height)
    {
        if (_framebuffer != 0) return;
        _projectionWidth = width;
        _projectionHeight = height;
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
            if (_depthTexture != 0)
                GL.DeleteTexture(_depthTexture);

            _disposed = true;
        }
    }

    /// <summary>
    /// Releases all resources used by <see cref="Screen"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// <see cref="Screen"/> destructor.
    /// </summary>
    ~Screen()
    {
        Dispose(false);
    }
}