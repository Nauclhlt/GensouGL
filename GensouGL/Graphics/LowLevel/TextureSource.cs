namespace GensouGL.Graphics;

public sealed class TextureSource
{
    private TextureSourceType _type;
    private string _filename;
    private Stream _stream = null;
    private byte[] _byteArray = null;
    private SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32> _image = null;

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

    internal TextureSource(SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32> image)
    {
        _image = image;
        _type = TextureSourceType.Temporary;
    }

    internal SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32> LoadImageSharpImage()
    {
        return _type switch
        {
            TextureSourceType.File => SixLabors.ImageSharp.Image.Load<SixLabors.ImageSharp.PixelFormats.Rgba32>(_filename),
            TextureSourceType.Stream => SixLabors.ImageSharp.Image.Load<SixLabors.ImageSharp.PixelFormats.Rgba32>(_stream),
            TextureSourceType.ByteArray => SixLabors.ImageSharp.Image.Load<SixLabors.ImageSharp.PixelFormats.Rgba32>(_byteArray),
            TextureSourceType.Temporary => _image,
            _ => throw new InvalidOperationException("Invalid TextureSourceType."),
        };
    }
}