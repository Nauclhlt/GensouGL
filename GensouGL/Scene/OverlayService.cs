namespace GensouGL.Core;

public sealed class OverlayService
{
    private readonly List<Overlay> _overlays = new();
    private Game _game;
    
    public bool Any => _overlays.Count > 0;
    public int Count => _overlays.Count;

    internal OverlayService(Game game)
    {
        _game = game;
    }

    public void Show(Overlay overlay)
    {
        ArgumentNullException.ThrowIfNull(overlay);
        if (!overlay.Initialized)
        {
            overlay._sceneManager = null;
            overlay._overlayService = _game._overlayService;
            overlay._input = _game._inputService;
            overlay._game = _game;
            overlay.OnInitialize();
            overlay.Initialized = true;
        }
        overlay.OnEnter();
        _overlays.Add(overlay);
    }

    public void CloseTop()
    {
        if (_overlays.Count > 0)
        {
            _overlays.RemoveAt(_overlays.Count - 1);
        }
    }

    public void CloseAll()
    {
        _overlays.Clear();
    }

    public void CloseLevel(int level)
    {
        if (level < 0 || level >= _overlays.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(level), "Level is out of range.");
        }
        _overlays.RemoveAt(_overlays.Count - level - 1);
    }

    public Overlay GetTop() => _overlays.Count > 0 ? _overlays[^1] : Overlay.Empty;

    internal void CheckState()
    {
        for (int i = _overlays.Count - 1; i >= 0; i--)
        {
            if (_overlays[i].Closing)
            {
                _overlays.RemoveAt(i);
            }
        }
    }

    internal void Update(float deltaTime)
    {
        for (int i = 0; i < _overlays.Count; i++)
        {
            _overlays[i].Update(deltaTime);
        }
    }

    internal void TopUpdate(float deltaTime)
    {
        if (_overlays.Count > 0) 
        {
            _overlays[^1].FocusUpdate(deltaTime);
        }
    }

    internal void Render(float deltaTime)
    {
        for (int i = 0; i < _overlays.Count; i++)
        {
            _overlays[i].Render(deltaTime);
        }
    }

    internal void TopRender(float deltaTime)
    {
        if (_overlays.Count > 0) 
        {
            _overlays[^1].FocusRender(deltaTime);
        }
    }

}