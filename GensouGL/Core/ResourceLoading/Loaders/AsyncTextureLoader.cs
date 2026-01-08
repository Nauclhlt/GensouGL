namespace GensouGL.Core.AsyncLoaders;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

internal sealed class AsyncTextureLoader : IAsyncResourceLoadingProcess
{
    private TextureSource _source;
    private Texture _texture;
    private Image<Rgba32> _img;

    public AsyncTextureLoader(TextureSource source)
    {
        _source = source;
    }

    public object Resource => _texture;

    public async Task Load()
    {
        _img = await _source.LoadImageSharpImageAsync();
    }

    public void Complete()
    {
        _texture = new(new TextureSource(_img));
        _img.Dispose();
    }
}