namespace GensouGL.Graphics;

/// <summary>
/// Handles animation.
/// </summary>
public sealed class Animator
{
    private Dictionary<string, AnimationClip> _animationList;
    private float _speed;

    private int _currentFrame;
    private string _current;
    private AnimationClip _currentClip;
    private bool _playing;
    private float _elapsed;
    private float _elapsedInFrame;

    /// <summary>
    /// Gets the name of the animation being played.
    /// </summary>
    public string Current => _current;
    /// <summary>
    /// Gets the animation clip being played.
    /// </summary>
    public AnimationClip CurrentClip => _currentClip;
    /// <summary>
    /// Gets the current frame number.
    /// </summary>
    public int CurrentFrame => _currentFrame;
    /// <summary>
    /// Gets the elapsed time in seconds.
    /// </summary>
    public float Elapsed => _elapsed;
    /// <summary>
    /// Gets or sets the speed of animation playback.
    /// </summary>
    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }
    /// <summary>
    /// Gets whether the animation is currently playing.
    /// </summary>
    public bool Playing => _playing;

    /// <summary>
    /// Initializes a new instance of <see cref="Animator"/>.
    /// </summary>
    public Animator()
    {
        _animationList = [];
        _current = string.Empty;
        _currentFrame = 0;
        _elapsed = 0;
        _speed = 1f;
        _playing = false;
    }

    /// <summary>
    /// Adds the specified animation clip with the specified name.
    /// </summary>
    /// <param name="name">Animation name.</param>
    /// <param name="clip">Animation clip.</param>
    public void Add(string name, AnimationClip clip)
    {
        if (_animationList.ContainsKey(name)) return;

        if (!ValidateClip(clip))
        {
            DebugLog.Error($"{nameof(Animator)}: Invalid AnimationClip.");
            return;
        }

        _animationList.Add(name, clip);
    }

    private static bool ValidateClip(AnimationClip clip)
    {
        return clip is not null && clip.Frames is not null && clip.Texture is not null;
    }

    /// <summary>
    /// Removes the animation with the specified name.
    /// </summary>
    /// <param name="name">Target animation name.</param>
    public void Remove(string name)
    {
        _animationList.Remove(name);
    }

    /// <summary>
    /// Plays the animation with the specified name.
    /// </summary>
    /// <param name="name">Target animation name.</param>
    public void Play(string name)
    {
        if (!_animationList.ContainsKey(name))
        {
            DebugLog.Error($"{nameof(Animator)}: Animation '{name}' is not present.");
            return;
        }

        _current = name;
        _currentClip = _animationList[name];
        _elapsed = 0f;
        _elapsedInFrame = 0f;
        _currentFrame = 0;
        _playing = true;
    }

    /// <summary>
    /// Updates the animation playback state.
    /// </summary>
    /// <remarks>
    /// Call Update() every frame to play the animation currectly.
    /// </remarks>
    /// <param name="deltaTime">Delta time.</param>
    public void Update(float deltaTime)
    {
        if (!_playing || _current == string.Empty) return;

        _elapsed += deltaTime * _speed;
        _elapsedInFrame += deltaTime * _speed;

        while (_elapsedInFrame > _currentClip.Frames[_currentFrame].Duration)
        {
            _elapsedInFrame -= _currentClip.Frames[_currentFrame].Duration;

            if (_currentFrame == _currentClip.Frames.Count - 1)
            {
                // END
                if (_currentClip.Loop)
                {
                    _currentFrame = 0;
                }
                else
                {
                    _playing = false;
                    break;
                }
            }
            else
            {
                _currentFrame++;
            }
        }
    }

    /// <summary>
    /// Gets the texture of the current frame.
    /// </summary>
    /// <returns>Texture of the current frame.</returns>
    public ITexture GetCurrentFrameTexture()
    {
        if (!_playing) return Texture.Empty;

        return _currentClip.Texture.GetSubtexture(_currentClip.Frames[_currentFrame].Source);
    }
}