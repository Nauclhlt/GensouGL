namespace GensouGL.Graphics;

public sealed class AnimationClip
{
    private bool _loop = false;
    private ITexture _texture;
    private ImmutableList<AnimationFrame> _frames;

    public bool Loop
    {
        get => _loop;
        set => _loop = value;
    }

    public ITexture Texture
    {
        get => _texture;
        set => _texture = value;
    }

    public IReadOnlyList<AnimationFrame> Frames => _frames;

    public AnimationClip(IList<AnimationFrame> frames, ITexture texture)
    {
        _frames = frames.ToImmutableList();
        _texture = texture;
    }

    public AnimationClip Clone()
    {
        return new AnimationClip(_frames, _texture)
        {
            Loop = _loop
        };
    }
}