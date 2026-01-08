namespace GensouGL.Graphics;

/// <summary>
/// Represents settings of font.
/// </summary>
public sealed class FontSettings
{
    private FontStyleMode _style = FontStyleMode.Regular;
    private float _outlineWidth = 0f;

    /// <summary>
    /// Gets the font style.
    /// </summary>
    public FontStyleMode Style => _style;

    /// <summary>
    /// Gets the font outline width.
    /// </summary>
    public float OutlineWidth => _outlineWidth;

    /// <summary>
    /// Initializes a new instance of <see cref="FontSettings"/> using the specified font style.
    /// </summary>
    /// <param name="style">Font style.</param>
    public FontSettings(FontStyleMode style)
    {
        _style = style;
        _outlineWidth = 0.0f;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="FontSettings"/> using the specified font style and outline width.
    /// </summary>
    /// <param name="style">Font style.</param>
    /// <param name="outlineWidth">Outline width.</param>
    public FontSettings(FontStyleMode style, float outlineWidth)
    {
        _style = style;
        _outlineWidth = float.Max(0f, outlineWidth);
    }

    /// <summary>
    /// Gets the regular font settings.
    /// </summary>
    public static FontSettings Regular { get; } = new(FontStyleMode.Regular);
    /// <summary>
    /// Gets the italic font settings.
    /// </summary>
    public static FontSettings Italic { get; } = new(FontStyleMode.Italic);
    /// <summary>
    /// Gets the bold font settings.
    /// </summary>
    public static FontSettings Bold { get; } = new(FontStyleMode.Bold);
    /// <summary>
    /// Gets the bolditalic font settings.
    /// </summary>
    public static FontSettings BoldItalic { get; } = new(FontStyleMode.BoldItalic);

    /// <summary>
    /// Gets the current font settings with the new outline width.
    /// </summary>
    /// <param name="width">Outline width.</param>
    /// <returns>Modified font settings.</returns>
    public FontSettings Outlined(float width)
    {
        return new FontSettings(_style, width);
    }
}