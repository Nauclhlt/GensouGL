namespace GensouGL.Core;

public sealed class Game
{
    private int _windowWidth;
    private int _windowHeight;
    private int _screenWidth;
    private int _screenHeight;
    private bool _vsync = false;
    private bool _fullscreen = false;
    private bool _cursorHidden = false;
    private double _fps = 60.0;
    private string _title = "GensouGL Game";

    private Scene _initialScene = Scene.Empty;

    public int WindowWidth => _windowWidth;
    public int WindowHeight => _windowHeight;
    public int ScreenWidth => _screenWidth;
    public int ScreenHeight => _screenHeight;
    public bool VSync => _vsync;
    public bool Fullscreen => _fullscreen;
    public bool CursorHidden => _cursorHidden;
    public double Fps => _fps;
    public string Title => _title;



    // these fields are not visible to the user
    internal GameWindow _window;
    internal SceneManager _sceneManager;
    internal OverlayService _overlayService;
    internal InputService _inputService;
    private List<TextInputListener> _textInputListeners;

    internal Game()
    {
    }

    public Game WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public Game WithWindowSize(int width, int height)
    {
        _windowWidth = width;
        _windowHeight = height;
        return this;
    }

    public Game WithScreenSize(int width, int height)
    {
        _screenWidth = width;
        _screenHeight = height;
        return this;
    }

    public Game WithFps(double fps)
    {
        _fps = fps;
        return this;
    }

    public Game UseVSync()
    {
        _vsync = true;
        return this;
    }

    public Game UseFullscreen()
    {
        _fullscreen = true;
        return this;
    }

    public Game HideCursor()
    {
        _cursorHidden = true;
        return this;
    }

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





    private void InitializeEngineCore()
    {
        _overlayService = new(this);
        _textInputListeners = new();
        _inputService = new(this);

        _sceneManager = new(this);
        
        Shader.LoadShaders();
        QuickRenderer.Initialize();
        BatchTextRenderer.Initialize();
        BatchBulletRenderer.Initialize();
        FullscreenQuadRenderer.Initialize();
        AudioPlayer.Initialize();
        InGameDebug.Initialize(this);
    }

    private void InitializeOpenGL()
    {
        GL.Viewport(0, 0, _window.ClientSize.X, _window.ClientSize.Y);
        GL.Enable(EnableCap.Texture2D);
        GL.Enable(EnableCap.Blend);
        GL.Disable(EnableCap.DepthTest);
        GL.Disable(EnableCap.StencilTest);
        GL.Enable(EnableCap.Blend);
    }

    private void FinalizeEngineCore()
    {
        QuickRenderer.Release();
        BatchTextRenderer.Release();
        BatchBulletRenderer.Release();
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

#if DEBUG
        InGameDebug.Update(deltaTime);
#endif
    }

    private void OnRenderWindow(FrameEventArgs args)
    {
        Color bg = _sceneManager.CurrentScene.BackgroundColor;
        GL.ClearColor(bg.Rf, bg.Gf, bg.Bf, 1.0f);
        GL.Clear(ClearBufferMask.ColorBufferBit);

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

#if DEBUG
        InGameDebug.RenderDebugOverlay(deltaTime);
#endif

        _window.SwapBuffers();
    }

    private void OnTextInput(TextInputEventArgs args)
    {
        for (int i = 0; i < _textInputListeners.Count; i++)
        {
            _textInputListeners[i].Send(args.AsString);
        }
    }

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

#if DEBUG
        DebugLog.Info("Initializing...");
#endif

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

        Screen.Default = new(0, _screenWidth, _screenHeight);

        
        _sceneManager.Navigate(_initialScene);


        _window.Run();

#if DEBUG
        DebugLog.Info("The window has been closed. Releasing all the resources...");
#endif

        FinalizeEngineCore();

#if DEBUG
        DebugLog.Info("Successfully ended.");
#endif
    }
}