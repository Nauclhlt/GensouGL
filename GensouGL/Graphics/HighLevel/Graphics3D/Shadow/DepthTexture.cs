using SixLabors.ImageSharp;

namespace GensouGL.Graphics;

internal sealed class DepthTexture : IDisposable, ITexture
{
    private int _texture;
    private int _width;
    private int _height;
    private bool _disposed = false;

    public int TextureId => _texture;
    public int BaseWidth => _width;
    public int BaseHeight => _height;
    public float Width => _width;
    public float Height => _height;
    public Rect Coords => new Rect(0, 0, 1.0f, 1.0f);

    public DepthTexture(int width, int height)
    {
        _width = width;
        _height = height;

        // create texture

        _texture = GL.GenTexture();

        TextureService.PushAndBind(_texture);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent, width, height, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

        float[] borderColor = [1f, 1f, 1f, 1f];
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, borderColor);

        TextureService.Pop();
    }

    public void SavePng(string filename)
    {
        float[] data = new float[_width * _height];

        TextureService.PushAndBind(_texture);

        GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.DepthComponent, PixelType.Float, data);

        using SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32> image = new(_width, _height);
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                image[x, y] = new SixLabors.ImageSharp.PixelFormats.Rgba32(data[y * _width + x], data[y * _width + x], data[y * _width + x]);
            }
        }

        image.SaveAsPng(filename);

        TextureService.Pop();
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            if (_texture != 0)
                GL.DeleteTexture(_texture);
            GC.SuppressFinalize(this);
            _disposed = true;
        }
    }

    ~DepthTexture()
    {
        Dispose();
    }
}