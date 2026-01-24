using SixLabors.ImageSharp.Processing;

namespace GensouGL.Graphics;

using static System.Runtime.InteropServices.JavaScript.JSType;
using IMS = SixLabors.ImageSharp;

/// <summary>
/// Represents a texture.
/// </summary>
public sealed class Texture : ITexture, IDisposable
{
    private int _id;
    private int _width;
    private int _height;
    private Rect _coords;
    private TextureResizeMode _resizeMode = TextureResizeMode.Bilinear;

    private bool _disposed = false;

    /// <summary>
    /// Gets the empty texture.
    /// </summary>
    /// <remarks>Returns a 32x32 white texture.</remarks>
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

    /// <summary>
    /// Initializes a new instance of <see cref="Texture"/> with the size and color.
    /// </summary>
    /// <param name="width">Width of the texture.</param>
    /// <param name="height">Heigbt of the texture.</param>
    /// <param name="fillColor">Fill color.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the texture size is invalid.</exception>
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

    /// <summary>
    /// Initializes a new instance of <see cref="Texture"/> using the specified texture source.
    /// </summary>
    /// <param name="source">Texture source.</param>
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

    /// <summary>
    /// Initializes a new instance of <see cref="Texture"/> with the source file name.
    /// </summary>
    /// <param name="filename">Source file name.</param>
    public Texture(string filename) : this(new TextureSource(filename)) { }
    /// <summary>
    /// Initializes a new instance of <see cref="Texture"/> with the source stream.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    public Texture(Stream stream) : this(new TextureSource(stream)) { }
    /// <summary>
    /// Initializes a new instance of <see cref="Texture"/> with the source byte array.
    /// </summary>
    /// <param name="byteArray">Source byte array.</param>
    public Texture(byte[] byteArray) : this(new TextureSource(byteArray)) { }

    /// <summary>
    /// Gets the texture id.
    /// </summary>
    public int TextureId => _id;
    /// <summary>
    /// Gets the width of the base texture(this).
    /// </summary>
    public int BaseWidth => _width;
    /// <summary>
    /// Gets the height of the base texture(this).
    /// </summary>
    public int BaseHeight => _height;
    /// <summary>
    /// Gets the width of the texture. Same value as <see cref="BaseWidth"/>.
    /// </summary>
    public float Width => _width;
    /// <summary>
    /// Gets the height of the texture. Same value as <see cref="BaseHeight"/>.
    /// </summary>
    public float Height => _height;
    /// <summary>
    /// Gets the UV coords.
    /// </summary>
    public Rect Coords => _coords;
    /// <summary>
    /// Gets or sets the resize mode.
    /// </summary>
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

    /// <summary>
    /// Saves the texture image as a png file.
    /// </summary>
    /// <param name="filename">File name.</param>
    public void SaveAsPngFile(string filename)
    {
        byte[] pixels = GetPixelData();

        using var image = IMS.Image.LoadPixelData<SixLabors.ImageSharp.PixelFormats.Rgba32>(pixels, _width, _height);
        image.Mutate(x => x.Flip(FlipMode.Vertical));

        image.Mutate(x => x.Flip(FlipMode.Vertical));

        IMS.ImageExtensions.SaveAsPng(image, filename);
    }

    /// <summary>
    /// Saves the texture image as a png stream.
    /// </summary>
    /// <param name="target">Target stream.</param>
    public void SaveAsPngFileToStream(Stream target)
    {
        byte[] pixels = GetPixelData();

        using var image = IMS.Image.LoadPixelData<SixLabors.ImageSharp.PixelFormats.Rgba32>(pixels, _width, _height);
        image.Mutate(x => x.Flip(FlipMode.Vertical));

        image.Mutate(x => x.Flip(FlipMode.Vertical));

        IMS.ImageExtensions.SaveAsPng(image, target);
    }

    /// <summary>
    /// Gets the partial texture representing the specified rectangle area.
    /// </summary>
    /// <param name="bounds">Rectangle bounds.</param>
    /// <returns>Partial texture.</returns>
    public ITexture GetPartial(Rect bounds)
    {
        return GetPartial(bounds.X, bounds.Y, bounds.Width, bounds.Height);
    }

    /// <summary>
    /// Gets the partial texture representing the specified rectangle area.
    /// </summary>
    /// <param name="x">X coordinate of the rectangle bounds.</param>
    /// <param name="y">Y coordinate of the rectangle bounds.</param>
    /// <param name="width">Width of the rectangle bounds.</param>
    /// <param name="height">Height of the rectangle bounds.</param>
    /// <returns>Partial texture.</returns>
    public ITexture GetPartial(float x, float y, float width, float height)
    {
        x /= _width;
        y /= _height;
        width /= _width;
        height /= _height;

        return new Subtexture(this, new Rect(x, y, width, height));
    }


    internal static Texture LoadFromPixelArray(byte[] pixels, int width, int height)
    {
        IMS.Image<IMS.PixelFormats.Rgba32> img = IMS.Image.LoadPixelData<IMS.PixelFormats.Rgba32>(pixels, width, height);
        return new(new TextureSource(img));
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

    /// <summary>
    /// Releases all resources used by <see cref="Texture"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// <see cref="Texture"/> destructor.
    /// </summary>
    ~Texture()
    {
        Dispose(false);
    }
}