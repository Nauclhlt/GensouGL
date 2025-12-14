namespace GensouGL.Graphics;

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

    public string Current => _current;
    public AnimationClip CurrentClip => _currentClip;
    public int CurrentFrame => _currentFrame;
    public float Elapsed => _elapsed;
    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }
    public bool Playing => _playing;

    public Animator()
    {
        _animationList = [];
        _current = string.Empty;
        _currentFrame = 0;
        _elapsed = 0;
        _speed = 1f;
        _playing = false;
    }

    public void Add(string name, AnimationClip clip)
    {
        if (_animationList.ContainsKey(name)) return;

        if (!ValidateClip(clip))
        {
#if DEBUG
            DebugLog.Error("Animator: Invalid AnimationClip.");
#endif
            return;
        }

        _animationList.Add(name, clip);
    }

    private static bool ValidateClip(AnimationClip clip)
    {
        return clip is not null && clip.Frames is not null && clip.Texture is not null;
    }

    public void Remove(string name)
    {
        _animationList.Remove(name);
    }

    public void Play(string name)
    {
        if (!_animationList.ContainsKey(name))
        {
#if DEBUG
            DebugLog.Error($"Animator: Animation '{name}' is not present.");
#endif
            return;
        }

        _current = name;
        _currentClip = _animationList[name];
        _elapsed = 0f;
        _elapsedInFrame = 0f;
        _currentFrame = 0;
        _playing = true;
    }

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

    public ITexture GetCurrentFrameTexture()
    {
        if (!_playing) return Texture.Empty;

        return _currentClip.Texture.GetSubtexture(_currentClip.Frames[_currentFrame].Source);
    }
}