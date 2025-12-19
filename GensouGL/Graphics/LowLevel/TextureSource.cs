namespace GensouGL.Graphics;

using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;

public sealed class TextureSource
{
    private TextureSourceType _type;
    private string _filename;
    private Stream _stream = null;
    private byte[] _byteArray = null;
    private Image<Rgba32> _image = null;

    public TextureSourceType Type => _type;
    public string Filename => _filename;
    public Stream Stream => _stream;
    public byte[] ByteArray => _byteArray;

    public TextureSource(string filename)
    {
        _filename = filename;
        _type = TextureSourceType.File;
    }

    public TextureSource(Stream stream)
    {
        _stream = stream;
        _type = TextureSourceType.Stream;
    }

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
        return _type switch
        {
            TextureSourceType.File => Image.Load<Rgba32>(_filename),
            TextureSourceType.Stream => Image.Load<Rgba32>(_stream),
            TextureSourceType.ByteArray => Image.Load<Rgba32>(_byteArray),
            TextureSourceType.Temporary => _image,
            _ => throw new InvalidOperationException("Invalid TextureSourceType."),
        };
    }

    internal async Task<Image<Rgba32>> LoadImageSharpImageAsync()
    {
        return _type switch
        {
            TextureSourceType.File => await Image.LoadAsync<Rgba32>(_filename),
            TextureSourceType.Stream => await Image.LoadAsync<Rgba32>(_stream),
            TextureSourceType.ByteArray => Image.Load<Rgba32>(_byteArray),
            TextureSourceType.Temporary => _image,
            _ => throw new InvalidOperationException("Invalid TextureSourceType.")
        };
    }
}