namespace GensouGL.Graphics;

internal sealed class DepthFramebuffer : IDisposable
{
    private int _framebuffer;
    private int _width;
    private int _height;
    private DepthTexture _texture;
    private bool _disposed = false;

    public int Framebuffer => _framebuffer;
    public DepthTexture Texture => _texture;

    public DepthFramebuffer(int width, int height)
    {
        _width = width;
        _height = height;

        _texture = new(width, height);

        // make framebuffer
        _framebuffer = GL.GenFramebuffer();

        FramebufferService.PushAndBind(_framebuffer);
        TextureService.PushAndBind(_texture.TextureId);

        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, _texture.TextureId, 0);
        GL.DrawBuffer(DrawBufferMode.None);
        GL.ReadBuffer(ReadBufferMode.None);

        TextureService.Pop();
        FramebufferService.Pop();
    }

    public void Clear()
    {
        FramebufferService.PushAndBind(_framebuffer);

        GL.ClearDepth(1.0f);
        GL.Clear(ClearBufferMask.DepthBufferBit);

        FramebufferService.Pop();
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _texture.Dispose();
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

    ~DepthFramebuffer()
    {
        Dispose(false);
    }
}