namespace GensouGL.Graphics;

/// <summary>
/// Represents a font source.
/// </summary>
public sealed class FontSource
{
    private string _filename;
    private Stream _stream;
    private FontSourceType _type;
    private SixLabors.Fonts.FontCollection _collection = new();

    /// <summary>
    /// Gets the source file name.
    /// </summary>
    public string FileName => _filename;
    /// <summary>
    /// Gets the source stream.
    /// </summary>
    public Stream Stream => _stream;
    /// <summary>
    /// Gets the type of the font source.
    /// </summary>
    public FontSourceType Type => _type;

    /// <summary>
    /// Initializes a new instance of <see cref="FontSource"/> using the specified file name.
    /// </summary>
    /// <param name="filename">Source file name.</param>
    public FontSource(string filename)
    {
        _filename = filename;
        using var fileStream = GensouFileSystem.OpenReadFile(filename);
        _collection = new();
        _collection.Add(fileStream);
        _type = FontSourceType.File;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="FontSource"/> using the specified stream.
    /// </summary>
    /// <param name="stream">Source stream.</param>
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