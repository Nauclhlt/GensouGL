using SixLabors.ImageSharp.Processing;

namespace GensouGL.Graphics;

using static System.Runtime.InteropServices.JavaScript.JSType;
using IMS = SixLabors.ImageSharp;

public sealed class Texture : ITexture, IDisposable
{
    private int _id;
    private int _width;
    private int _height;
    private Rect _coords;
    private TextureResizeMode _resizeMode = TextureResizeMode.Bilinear;

    private bool _disposed = false;

    public static Texture Empty
    {
        get;
    } = new Texture(32, 32, Color.White);

    private Texture()
    {

    }

    internal Texture(int width, int height)
    {
        _id = GL.GenTexture();
        _width = width;
        _height = height;
        _coords = new Rect(0, 0, 1, 1);

        TextureService.PushAndBind(_id);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, _width, _height, 0,
            PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        
        TextureService.Pop();
    }

    public Texture(int width, int height, Color fillColor)
    {
        if (width < 0 || height < 0)
            throw new ArgumentOutOfRangeException("Width and Height must be non-negative.");

        _id = GL.GenTexture();
        _width = width;
        _height = height;
        _coords = new Rect(0, 0, 1, 1);

        byte[] pixels = new byte[width * height * 4];
        for (int i = 0; i < width * height; i++)
        {
            pixels[i * 4 + 0] = fillColor.Rb;
            pixels[i * 4 + 1] = fillColor.Gb;
            pixels[i * 4 + 2] = fillColor.Bb;
            pixels[i * 4 + 3] = fillColor.Ab;
        }

        TextureService.PushAndBind(_id);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, _width, _height, 0,
            PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

        TextureService.Pop();
    }

    public Texture(TextureSource source)
    {
        using SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32> img = source.LoadImageSharpImage();
        byte[] data = new byte[img.Width * img.Height * 4];
        img.CopyPixelDataTo(data);
        _width = img.Width;
        _height = img.Height;
        _coords = new Rect(0, 0, 1, 1);

        _id = GL.GenTexture();

        TextureService.PushAndBind(_id);
        
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, _width, _height, 0,
            PixelFormat.Rgba, PixelType.UnsignedByte, data);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        
        TextureService.Pop();
    }

    public Texture(string filename) : this(new TextureSource(filename)) { }
    public Texture(Stream stream) : this(new TextureSource(stream)) { }
    public Texture(byte[] byteArray) : this(new TextureSource(byteArray)) { }

    public int TextureId => _id;
    public int BaseWidth => _width;
    public int BaseHeight => _height;
    public float Width => _width;
    public float Height => _height;
    public Rect Coords => _coords;
    public TextureResizeMode ResizeMode
    {
        get => _resizeMode;
        set
        {
            TextureService.PushAndBind(_id);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)value);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)value);

            _resizeMode = value;

            TextureService.Pop();
        }
    }

    private byte[] GetPixelData()
    {
        byte[] pixels = new byte[_width * _height * 4];

        TextureService.PushAndBind(_id);

        GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);

        TextureService.Pop();

        return pixels;
    }

    public void SaveAsPngFile(string filename)
    {
        byte[] pixels = GetPixelData();

        using var image = IMS.Image.LoadPixelData<SixLabors.ImageSharp.PixelFormats.Rgba32>(pixels, _width, _height);
        image.Mutate(x => x.Flip(FlipMode.Vertical));

        image.Mutate(x => x.Flip(FlipMode.Vertical));

        IMS.ImageExtensions.SaveAsPng(image, filename);
    }

    public void SaveAsPngFileToStream(Stream target)
    {
        byte[] pixels = GetPixelData();

        using var image = IMS.Image.LoadPixelData<SixLabors.ImageSharp.PixelFormats.Rgba32>(pixels, _width, _height);
        image.Mutate(x => x.Flip(FlipMode.Vertical));

        image.Mutate(x => x.Flip(FlipMode.Vertical));

        IMS.ImageExtensions.SaveAsPng(image, target);
    }

    public Subtexture GetPartial(Rect bounds)
    {
        return GetPartial(bounds.X, bounds.Y, bounds.Width, bounds.Height);
    }

    public Subtexture GetPartial(float x, float y, float width, float height)
    {
        x /= _width;
        y /= _height;
        width /= _width;
        height /= _height;

        return new Subtexture(this, new Rect(x, y, width, height));
    }

    public static Texture CreateMonoColor(int width, int height, Color color)
    {
        var image = new IMS.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>(width, height, new IMS.PixelFormats.Rgba32(color.R, color.G, color.B, color.A));

        return new Texture(new TextureSource(image));
    }


    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {

            }

            GL.DeleteTexture(_id);

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~Texture()
    {
        Dispose(false);
    }
}