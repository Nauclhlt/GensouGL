namespace GensouGL.Core;

/// <summary>
/// Specifies the type of texture source.
/// </summary>
public enum TextureSourceType
{
    /// <summary>
    /// From a file.
    /// </summary>
    File,
    /// <summary>
    /// From a stream.
    /// </summary>
    Stream,
    /// <summary>
    /// From a byte array.
    /// </summary>
    ByteArray,
    /// <summary>
    /// Temporary source type.
    /// </summary>
    Temporary
}