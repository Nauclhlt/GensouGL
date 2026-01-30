namespace GensouGL.Graphics;

using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;

/// <summary>
/// Represents a texture source.
/// </summary>
public sealed class TextureSource
{
    private TextureSourceType _type;
    private string _filename;
    private Stream _stream = null;
    private byte[] _byteArray = null;
    private Image<Rgba32> _image = null;

    /// <summary>
    /// Gets the type of the texture source.
    /// </summary>
    public TextureSourceType Type => _type;
    /// <summary>
    /// Gets the file name.
    /// </summary>
    public string Filename => _filename;
    /// <summary>
    /// Gets the stream. If <see cref="Type"/> is not <see cref="TextureSourceType.Stream"/>, returns null.
    /// </summary>
    public Stream Stream => _stream;
    /// <summary>
    /// Gets the byte array. If <see cref="Type"/> is not <see cref="TextureSourceType.ByteArray"/>, returns null.
    /// </summary>
    public byte[] ByteArray => _byteArray;

    /// <summary>
    /// Initializes a new instance of <see cref="TextureSource"/> with the file name.
    /// </summary>
    /// <param name="filename">Source file name.</param>
    public TextureSource(string filename)
    {
        _filename = filename;
        _type = TextureSourceType.File;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="TextureSource"/> with the stream.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    public TextureSource(Stream stream)
    {
        _stream = stream;
        _type = TextureSourceType.Stream;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="TextureSource"/> with the byte array.
    /// </summary>
    /// <param name="byteArray">Source byte array.</param>
    public TextureSource(byte[] byteArray)
    {
        _byteArray = byteArray;
        _type = TextureSourceType.ByteArray;
    }

    internal TextureSource(Image<Rgba32> image)
    {
        _image = image;
        _type = TextureSourceType.Temporary;
    }

    internal Image<Rgba32> LoadImageSharpImage()
    {
        if (_type == TextureSourceType.File)
        {
            using var fileStream = GensouFileSystem.OpenReadFile(_filename);
            return Image.Load<Rgba32>(fileStream);
        }

        return _type switch
        {
            TextureSourceType.Stream => Image.Load<Rgba32>(_stream),
            TextureSourceType.ByteArray => Image.Load<Rgba32>(_byteArray),
            TextureSourceType.Temporary => _image,
            _ => throw new InvalidOperationException("Invalid TextureSourceType."),
        };
    }

    internal async Task<Image<Rgba32>> LoadImageSharpImageAsync()
    {
        if (_type == TextureSourceType.File)
        {
            byte[] bytes = await GensouFileSystem.ReadAllBytesAsync(_filename);
            return Image.Load<Rgba32>(bytes);
        }

        return _type switch
        {
            TextureSourceType.Stream => await Image.LoadAsync<Rgba32>(_stream),
            TextureSourceType.ByteArray => Image.Load<Rgba32>(_byteArray),
            TextureSourceType.Temporary => _image,
            _ => throw new InvalidOperationException("Invalid TextureSourceType.")
        };
    }
}