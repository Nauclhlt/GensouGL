namespace GensouGL.Graphics;

public sealed class FontSettings
{
    private FontStyleMode _style = FontStyleMode.Regular;
    private float _outlineWidth = 0f;

    public FontStyleMode Style => _style;

    public float OutlineWidth => _outlineWidth;

    public FontSettings(FontStyleMode style)
    {
        _style = style;
        _outlineWidth = 0.0f;
    }

    public FontSettings(FontStyleMode style, float outlineWidth)
    {
        _style = style;
        _outlineWidth = float.Max(0f, outlineWidth);
    }

    public static FontSettings Regular { get; } = new(FontStyleMode.Regular);
    public static FontSettings Italic { get; } = new(FontStyleMode.Italic);
    public static FontSettings Bold { get; } = new(FontStyleMode.Bold);
    public static FontSettings BoldItalic { get; } = new(FontStyleMode.BoldItalic);

    public FontSettings Outlined(float width)
    {
        return new FontSettings(_style, width);
    }
}