using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace GensouGL.Graphics;

using IMS = SixLabors.ImageSharp;
using IMSP = SixLabors.ImageSharp.PixelFormats;

public sealed class DynamicTextureAtlas : IDisposable
{
    private readonly Texture _atlasTexture;
    private readonly Subtexture[] _items;

    private bool _disposed = false;

    public int Count => _items.Length;
    public Texture AtlasTexture => _atlasTexture;
    public int AtlasWidth => _atlasTexture.BaseWidth;
    public int AtlasHeight => _atlasTexture.BaseHeight;

    public ITexture this[int index]
    {
        get => Get(index);
    }

    public DynamicTextureAtlas(TextureSource[] sources, int atlasWidth, int atlasHeight)
    {
        if (atlasWidth < 0 || atlasHeight < 0)
        {
            throw new ArgumentException();
        }
        using IMS.Image<IMSP.Rgba32> atlasImage = new(atlasWidth, atlasHeight);

        IMS.Image<IMSP.Rgba32>[] sourceImages = new IMS.Image<IMSP.Rgba32>[sources.Length];
        Rect[] bounds = new Rect[sources.Length];
        _items = new Subtexture[sources.Length];

        for (int i = 0; i < sources.Length; i++)
        {
            sourceImages[i] = sources[i].LoadImageSharpImage();
        }

        try
        {
            atlasImage.Mutate(ctx =>
            {
                int top = 0;
                int left = 0;
                int maxHeight = 0;

                for (int i = 0; i < sourceImages.Length; i++)
                {
                    int w = sourceImages[i].Width;
                    int h = sourceImages[i].Height;

                    if (left + w > atlasWidth)
                    {
                        left = 0;
                        top += maxHeight;
                        maxHeight = 0;
                    }

                    ctx.DrawImage(sourceImages[i], new IMS.Rectangle(left, top, w, h), 1.0f);
                    bounds[i] = new Rect(left, top, w, h);
                    left += w;
                    maxHeight = int.Max(maxHeight, h);
                }
            });

            _atlasTexture = new Texture(new TextureSource(atlasImage));

            for (int i = 0; i < sources.Length; i++)
            {
                _items[i] = _atlasTexture.GetPartial(bounds[i]);
            }
        }
        finally
        {
            for (int i = 0; i <  sourceImages.Length; i++)
            {
                sourceImages[i].Dispose();
            }
        }
    }

    public ITexture Get(int index)
    {
        if (index < 0 || index >= _items.Length)
        {
            throw new IndexOutOfRangeException();
        }

        return _items[index];
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _atlasTexture?.Dispose();
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~DynamicTextureAtlas()
    {
        Dispose(false);
    }
}