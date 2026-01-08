namespace GensouGL.Graphics;

/// <summary>
/// Represents an animation clip.
/// </summary>
public sealed class AnimationClip
{
    private bool _loop = false;
    private ITexture _texture;
    private ImmutableList<AnimationFrame> _frames;

    /// <summary>
    /// Gets or sets if the animation loops.
    /// </summary>
    public bool Loop
    {
        get => _loop;
        set => _loop = value;
    }

    /// <summary>
    /// Gets or sets the animation sprite texture associated with the animation.
    /// </summary>
    public ITexture Texture
    {
        get => _texture;
        set => _texture = value;
    }

    /// <summary>
    /// Gets the animation frames.
    /// </summary>
    public IReadOnlyList<AnimationFrame> Frames => _frames;

    /// <summary>
    /// Initializes a new instance of <see cref="AnimationClip" /> using the specified parameters.
    /// </summary>
    /// <param name="frames">Animation frames.</param>
    /// <param name="texture">Animation sprite texture.</param>
    public AnimationClip(IList<AnimationFrame> frames, ITexture texture)
    {
        _frames = frames.ToImmutableList();
        _texture = texture;
    }

    /// <summary>
    /// Clones the animation clip.
    /// </summary>
    /// <returns>Clone.</returns>
    public AnimationClip Clone()
    {
        return new AnimationClip(_frames, _texture)
        {
            Loop = _loop
        };
    }
}