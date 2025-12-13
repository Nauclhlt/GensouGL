namespace GensouGL.Graphics;

public sealed class FontSource
{
    private string _filename;
    private Stream _stream;
    private FontSourceType _type;
    private SixLabors.Fonts.FontCollection _collection = new();

    public string FileName => _filename;
    public Stream Stream => _stream;
    public FontSourceType Type => _type;

    public FontSource(string filename)
    {
        _filename = filename;
        _collection = new();
        _collection.Add(filename);
        _type = FontSourceType.File;
    }

    public FontSource(Stream stream)
    {
        _stream = stream;
        stream.Seek(0, SeekOrigin.Begin);
        _collection = new();
        _collection.Add(stream);
        stream.Seek(0, SeekOrigin.Begin);
        _type = FontSourceType.Stream;
    }

    internal SixLabors.Fonts.FontFamily GetFontFamily()
    {
        return _collection.Families.First();
    }
}