namespace GensouGL.Core;

public class Scene
{
    public static Scene Empty { get; } = new Scene();

    
    internal bool Initialized { get; set; }
    public Color BackgroundColor { get; set; } = new Color(0, 0, 0, 255);


    internal SceneManager _sceneManager;
    internal OverlayService _overlayService;
    internal InputService _input;
    internal Game _game;

    protected Rect ScreenBounds => new Rect(0, 0, _game.ScreenWidth, _game.ScreenHeight);
    protected Rect WindowBounds => new Rect(0, 0, _game.WindowWidth, _game.WindowHeight);
    protected Point ScreenCenter => new Point(_game.ScreenWidth / 2f, _game.ScreenHeight / 2f);
    protected SceneManager SceneManager => _sceneManager;
    protected OverlayService OverlayService => _overlayService;
    protected InputService Input => _input;


    protected QuickRenderer CreateQuickRenderer(Screen screen)
    {
        return new QuickRenderer(_game, screen);
    }

    protected BatchTextRenderer CreateBatchTextRenderer(Screen screen)
    {
        return new BatchTextRenderer(_game, screen);
    }

    protected BatchBulletRenderer CreateBatchBulletRenderer(Screen screen)
    {
        return new BatchBulletRenderer(_game, screen);
    }

    protected void RegisterPersistentResource(IDisposable resource)
    {
        PersistentResourceService.Register(resource);
    }

    protected void ExitGame()
    {
        _game.ExitGame();
    }

    public virtual void Update(float deltaTime)
    {
    }

    public virtual void FocusUpdate(float deltaTime)
    {
    }

    public virtual void Render(float deltaTime)
    {
    }

    public virtual void FocusRender(float deltaTime)
    {
    }

    public virtual void OnInitialize()
    {
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnLeave()
    {
    }
}