namespace GensouGL.Core;

/// <summary>
/// Manages Overlays.
/// </summary>
public sealed class OverlayService
{
    private readonly List<Overlay> _overlays = new();
    private Game _game;
    
    /// <summary>
    /// Gets whether one or more overlays are currently shown.
    /// </summary>
    public bool Any => _overlays.Count > 0;
    /// <summary>
    /// Gets the number of overlays shown.
    /// </summary>
    public int Count => _overlays.Count;

    internal OverlayService(Game game)
    {
        _game = game;
    }

    /// <summary>
    /// Shows the specified overlay.
    /// </summary>
    /// <param name="overlay">Overlay to show.</param>
    public void Show(Overlay overlay)
    {
        ArgumentNullException.ThrowIfNull(overlay);
        if (!overlay.Initialized)
        {
            overlay._context = new GameSceneContext(_game);
            overlay.OnInitialize();
            overlay.Initialized = true;
        }
        overlay.OnEnter();
        _overlays.Add(overlay);
    }

    /// <summary>
    /// Closes the top-level overlay.
    /// </summary>
    public void CloseTop()
    {
        if (_overlays.Count > 0)
        {
            _overlays.RemoveAt(_overlays.Count - 1);
        }
    }

    /// <summary>
    /// Closes all overlays.
    /// </summary>
    public void CloseAll()
    {
        _overlays.Clear();
    }

    /// <summary>
    /// Closes the overlay at the specified level.
    /// </summary>
    /// <param name="level">Level.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the level is invalid.</exception>
    public void CloseLevel(int level)
    {
        if (level < 0 || level >= _overlays.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(level), "Level is out of range.");
        }
        _overlays.RemoveAt(_overlays.Count - level - 1);
    }

    /// <summary>
    /// Gets the top-level overlay.
    /// </summary>
    /// <returns>Top-level overlay.</returns>
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