namespace GensouGL.Graphics;

public sealed class FontSource
{
    private string _filename;
    private Stream _stream;
    private FontSourceType _type;


    public string FileName => _filename;
    public Stream Stream => _stream;
    public FontSourceType Type => _type;

    public FontSource(string filename)
    {
        _filename = filename;
        _type = FontSourceType.File;
    }

    public FontSource(Stream stream)
    {
        _stream = stream;
        _type = FontSourceType.Stream;
    }

    internal SixLabors.Fonts.FontFamily GetFontFamily()
    {
        SixLabors.Fonts.FontCollection collection = new();
        if (_type == FontSourceType.File) collection.Add(_filename);
        else if (_type == FontSourceType.Stream) collection.Add(_stream);

        return collection.Families.First();
    }
}