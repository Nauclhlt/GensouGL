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

    protected SceneManager SceneManager => _sceneManager;
    protected OverlayService OverlayService => _overlayService;
    protected InputService Input => _input;


    protected QuickRenderer CreateQuickRenderer(Screen screen)
    {
        return new QuickRenderer(_game, screen);
    }

    protected void RegisterPersistentResource(IDisposable resource)
    {
        PersistentResourceService.Register(resource);
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