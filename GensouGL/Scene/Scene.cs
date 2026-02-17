namespace GensouGL.Core;

/// <summary>
/// Represents a scene.
/// </summary>
/// <remarks>To create a scene, inherit <see cref="Scene"/> and override needed methods. 
/// Use <see cref="Scene.SceneNavigator"/> to navigate to your scene.</remarks>
public class Scene
{
    /// <summary>
    /// Gets the empty scene.
    /// </summary>
    public static Scene Empty { get; } = new Scene();

    
    internal bool Initialized { get; set; }    
    internal ISceneContext _context;


    /// <summary>
    /// Returns the bounding rectangle of the screen.
    /// </summary>
    protected Rect ScreenBounds => new Rect(0, 0, ScreenWidth, ScreenHeight);
    /// <summary>
    /// Returns the bounding rectangle of the client area of the window.
    /// </summary>
    protected Rect WindowBounds => new Rect(0, 0, WindowWidth, WindowHeight);
    /// <summary>
    /// Gets the width of the screen.
    /// </summary>
    protected int ScreenWidth => _context.ScreenWidth;
    /// <summary>
    /// Gets the height of the screen.
    /// </summary>
    protected int ScreenHeight => _context.ScreenHeight;
    /// <summary>
    /// Gets the width of the window.
    /// </summary>
    protected int WindowWidth => _context.WindowWidth;
    /// <summary>
    /// Gets the height of the window.
    /// </summary>
    protected int WindowHeight => _context.WindowHeight;
    /// <summary>
    /// Returns the center point of the screen.
    /// </summary>
    protected Point ScreenCenter => new Point(ScreenWidth / 2f, ScreenHeight / 2f);
    /// <summary>
    /// Returns the center point of the client area of the window.
    /// </summary>
    protected Point WindowCenter => new Point(WindowWidth / 2f, WindowHeight / 2f);
    /// <summary>
    /// Gets the scene navigator instance.
    /// </summary>
    protected ISceneNavigator SceneNavigator => _context.SceneNavigator;
    /// <summary>
    /// Gets the overlay service.
    /// </summary>
    protected OverlayService OverlayService => _context.OverlayService;
    /// <summary>
    /// Gets the input service.
    /// </summary>
    protected InputService Input => _context.InputService;

    /// <summary>
    /// Gets or sets the background color of the scene.
    /// </summary>
    public Color BackgroundColor { get; set; } = new Color(0, 0, 0, 255);

    /// <summary>
    /// Registers the specified resource as a persistent resource. Objects registered as persistent resources will be released together when the game ends.
    /// </summary>
    /// <param name="resource">Resource to register.</param>
    protected void RegisterPersistentResource(IDisposable resource)
    {
        PersistentResourceService.Register(resource);
    }

    /// <summary>
    /// Ends the game.
    /// </summary>
    protected void ExitGame() => _context.ControlContext.ExitGame();
    /// <summary>
    /// Sets the fullscreen flag.
    /// </summary>
    /// <param name="fullscreen">New fullscreen flag.</param>
    protected void SetFullscreenMode(bool fullscreen) => _context.ControlContext.SetFullscreenMode(fullscreen);
    /// <summary>
    /// Sets the FPS.
    /// </summary>
    /// <param name="fps">New fps value.</param>
    protected void SetFps(float fps) => _context.ControlContext.SetFps(fps);
    /// <summary>
    /// Sets the window title.
    /// </summary>
    /// <param name="title">New window title.</param>
    protected void SetTitle(string title) => _context.ControlContext.SetTitle(title);

    /// <summary>
    /// Sets the visibility of the mouse cursor.
    /// </summary>
    /// <param name="visible">New visibility.</param>
    protected void SetCursorVisibility(bool visible) => _context.SetCursorVisibility(visible);

    /// <summary>
    /// When overridden, runs the scene update logic.
    /// </summary>
    /// <param name="deltaTime">Delta time.</param>
    public virtual void Update(float deltaTime)
    {
    }

    /// <summary>
    /// When overridden, runs the scene update logic(with focus).
    /// </summary>
    /// <param name="deltaTime">Delta time.</param>
    public virtual void FocusUpdate(float deltaTime)
    {
    }

    /// <summary>
    /// When overridden, runs the scene rendering logic.
    /// </summary>
    /// <param name="deltaTime">Delta time.</param>
    public virtual void Render(float deltaTime)
    {
    }

    /// <summary>
    /// When overridden, runs the scene rendering logic(with focus).
    /// </summary>
    /// <param name="deltaTime">Delta time.</param>
    public virtual void FocusRender(float deltaTime)
    {
    }

    /// <summary>
    /// When overridden, initializes the scene.
    /// </summary>
    public virtual void OnInitialize()
    {
    }

    /// <summary>
    /// When overridden, runs the entering process.
    /// </summary>
    public virtual void OnEnter()
    {
    }

    /// <summary>
    /// When overridden, runs the leaving process.
    /// </summary>
    public virtual void OnLeave()
    {
    }
}