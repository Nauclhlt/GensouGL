namespace GensouGL.Core;

/// <summary>
/// Represents a GensouGL Game.
/// </summary>
/// <remarks>
/// <see cref="Run()"/> starts the game. Instance of Game class works as a builder until <see cref="Run()"/> method is called. Use With*() to set the needed parameters. Game class does not provide any public constructors, and <see cref="Gensou.Create()"/> returns a new instance of <see cref="Game"/> instead, with all the parameters set to default values.
/// </remarks>
public sealed class Game
{
    private int _windowWidth = 640;
    private int _windowHeight = 480;
    private int _screenWidth = 640;
    private int _screenHeight = 480;
    private bool _vsync = false;
    private bool _fullscreen = false;
    private bool _cursorHidden = false;
    private double _fps = 60.0;
    private string _title = "GensouGL Game";

    private Scene _initialScene = Scene.Empty;

    /// <summary>
    /// Gets the window width.
    /// </summary>
    public int WindowWidth => _windowWidth;
    /// <summary>
    /// Gets the window height.
    /// </summary>
    public int WindowHeight => _windowHeight;
    /// <summary>
    /// Gets the screen width.
    /// </summary>
    public int ScreenWidth => _screenWidth;
    /// <summary>
    /// Gets the screen height.
    /// </summary>
    public int ScreenHeight => _screenHeight;
    /// <summary>
    /// Gets whether the v-sync is enabled.
    /// </summary>
    public bool VSync => _vsync;
    /// <summary>
    /// Gets whether the game is full-screen.
    /// </summary>
    public bool Fullscreen => _fullscreen;
    /// <summary>
    /// Gets whether the cursor is hidden.
    /// </summary>
    public bool CursorHidden => _cursorHidden;
    public double Fps => _fps;
    public string Title => _title;


    // these fields are not visible to the user
    private GameWindow _window;
    private SceneManager _sceneManager;
    private OverlayService _overlayService;
    private InputService _inputService;
    private List<TextInputListener> _textInputListeners;

    internal GameWindow Window => _window;
    internal SceneManager SceneManager => _sceneManager;
    internal OverlayService OverlayService => _overlayService;
    internal InputService InputService => _inputService;

    internal Game()
    {
    }

    /// <summary>
    /// Sets the window title.
    /// </summary>
    /// <param name="title">Window title.</param>
    /// <returns>Current Game instance.</returns>
    public Game WithTitle(string title)
    {
        _title = title;
        return this;
    }

    /// <summary>
    /// Sets the window size.
    /// </summary>
    /// <param name="width">Window width.</param>
    /// <param name="height">Window height.</param>
    /// <returns>Current Game instance.</returns>
    public Game WithWindowSize(int width, int height)
    {
        _windowWidth = width;
        _windowHeight = height;
        return this;
    }

    /// <summary>
    /// Sets the screen size.
    /// </summary>
    /// <param name="width">Screen width.</param>
    /// <param name="height">Screen height.</param>
    /// <returns>Current Game instance.</returns>
    public Game WithScreenSize(int width, int height)
    {
        _screenWidth = width;
        _screenHeight = height;
        return this;
    }

    /// <summary>
    /// Sets the FPS(frames-per-second) value.
    /// </summary>
    /// <param name="fps">FPS.</param>
    /// <returns>Current Game instance.</returns>
    public Game WithFps(double fps)
    {
        _fps = fps;
        return this;
    }

    /// <summary>
    /// Enables the v-sync.
    /// </summary>
    /// <returns>Current Game instance.</returns>
    public Game UseVSync()
    {
        _vsync = true;
        return this;
    }

    /// <summary>
    /// Make the window full-screen.
    /// </summary>
    /// <returns>Current Game instance.</returns>
    public Game UseFullscreen()
    {
        _fullscreen = true;
        return this;
    }

    /// <summary>
    /// Hides the mouse cursor.
    /// </summary>
    /// <returns>Current Game instance.</returns>
    public Game HideCursor()
    {
        _cursorHidden = true;
        return this;
    }

    /// <summary>
    /// Sets the first scene to be shown.
    /// </summary>
    /// <param name="scene">Initial scene.</param>
    /// <returns>Current Game instance.</returns>
    public Game WithInitialScene(Scene scene)
    {
        _initialScene = scene;
        return this;
    }



    // internal
    internal void RegisterTextInputListener(TextInputListener listener)
    {
        _textInputListeners.Add(listener);
    }

    internal void UnregisterTextInputListener(TextInputListener listener)
    {
        _textInputListeners.Remove(listener);
    }

    internal void SetCursorVisibility(bool visible)
    {
        _cursorHidden = visible;
        _window.CursorState = visible ? CursorState.Normal : CursorState.Hidden;
    }





    private void InitializeEngineCore()
    {
        _overlayService = new(this);
        _textInputListeners = new();
        _inputService = new(this);

        _sceneManager = new(this);
        
        Shader.LoadShaders();
        QuickRenderer2D.Initialize();
        BatchTextRenderer2D.Initialize();
        BatchBulletRenderer2D.Initialize();
        FullscreenQuadRenderer.Initialize();
        AudioPlayer.Initialize();
        InGameDebug.Initialize(this);

        Screen.Default = new(0, _screenWidth, _screenHeight);
    }

    private void InitializeOpenGL()
    {
        GL.Viewport(0, 0, _window.ClientSize.X, _window.ClientSize.Y);
        GL.Enable(EnableCap.Texture2D);
        GL.Enable(EnableCap.Blend);


        GL.Disable(EnableCap.DepthTest);
        GL.Disable(EnableCap.StencilTest);
    }

    private void FinalizeEngineCore()
    {
        QuickRenderer2D.Release();
        BatchTextRenderer2D.Release();
        BatchBulletRenderer2D.Release();
        FullscreenQuadRenderer.Release();
        AudioPlayer.Release();
        InGameDebug.Release();
        PersistentResourceService.ReleaseAll();
    }

    internal void ExitGame()
    {
        _window.Close();
    }



    // events
    private void OnUpdateWindow(FrameEventArgs args)
    {
        float deltaTime = (float)args.Time;

        AudioPlayer.Update();

        _sceneManager.Update(deltaTime);

        if (_overlayService.Any)
        {
            _overlayService.Update(deltaTime);
            _overlayService.TopUpdate(deltaTime);
        }
        else
        {
            _sceneManager.FocusUpdate(deltaTime);
        }

        if (_overlayService.Any) _overlayService.CheckState();

        InGameDebug.Update(deltaTime);
    }

    private void OnRenderWindow(FrameEventArgs args)
    {
        FramebufferService.PushAndBind(Screen.Default.ScreenId);

        Color bg = _sceneManager.CurrentScene.BackgroundColor;
        GL.ClearColor(bg.R, bg.G, bg.B, 1.0f);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        FramebufferService.Pop();

        float deltaTime = (float)args.Time;

        _sceneManager.Render(deltaTime);

        if (_overlayService.Any)
        {
            _overlayService.Render(deltaTime);
            _overlayService.TopRender(deltaTime);
        }
        else
        {
            _sceneManager.FocusRender(deltaTime);
        }

        InGameDebug.RenderDebugOverlay(deltaTime);

        _window.SwapBuffers();
    }

    private void OnTextInput(TextInputEventArgs args)
    {
        for (int i = 0; i < _textInputListeners.Count; i++)
        {
            _textInputListeners[i].Send(args.AsString);
        }
    }

    /// <summary>
    /// Runs the game.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when one or more parameters are invalid.</exception>
    public void Run()
    {
        if (_windowWidth <= 0 || _windowHeight <= 0)
        {
            throw new InvalidOperationException("Window width and height must be greater than zero.");
        }

        if (_screenWidth <= 0 || _screenHeight <= 0)
        {
            throw new InvalidOperationException("Screen width and height must be greater than zero.");
        }

        ArgumentNullException.ThrowIfNull(_initialScene, "Initial scene cannot be null.");

        DebugLog.Info("Initializing...");

        GameWindowSettings gs = new();
        NativeWindowSettings ns = new();

        ns.ClientSize = new OpenTK.Mathematics.Vector2i(_windowWidth, _windowHeight);
        ns.Title = _title;
        ns.WindowState = _fullscreen ? WindowState.Fullscreen : WindowState.Normal;

        ns.APIVersion = new Version(4, 6);

        gs.Win32SuspendTimerOnDrag = true;
        ns.Vsync = _vsync ? VSyncMode.On : VSyncMode.Off;
        gs.UpdateFrequency = _fps;

        using GameWindow gw = new(gs, ns);
        _window = gw;

        _window.CursorState = _cursorHidden ? CursorState.Hidden : CursorState.Normal;
        _window.WindowBorder = WindowBorder.Fixed;

        _window.CenterWindow();

        _window.UpdateFrame += OnUpdateWindow;
        _window.RenderFrame += OnRenderWindow;
        _window.TextInput += OnTextInput;

        _windowWidth = _window.ClientSize.X;
        _windowHeight = _window.ClientSize.Y;



        InitializeOpenGL();
        InitializeEngineCore();

        
        _sceneManager.Navigate(_initialScene);


        _window.Run();

        DebugLog.Info("Game window has been closed. Releasing all the resources...");

        FinalizeEngineCore();

        DebugLog.Info("Successfully ended.");
    }
}