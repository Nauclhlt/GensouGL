namespace GensouGL.Graphics;

public abstract class Renderer
{
    protected Game _game;
    protected ScreenStack _screenStack;
    protected Camera _camera;
    protected RenderState _state;

    internal ScreenStack ScreenStack => _screenStack;

    public Camera Camera
    {
        get => _camera;
        set
        {
            _camera = value is not null ? value : Camera.Default;
        }
    }

    public RenderState State
    {
        get => _state;
        set => _state = value;
    }

    protected OpenTK.Mathematics.Matrix4 GetDefaultVMatrix()
    {
        OpenTK.Mathematics.Matrix4 projMatrix = OpenTK.Mathematics.Matrix4.CreateOrthographicOffCenter(0, _screenStack.BaseScreenWidth, _screenStack.BaseScreenHeight, 0, -1, 1);
        return projMatrix * _camera.GetModelMatrix();
    }

    // called from constructors of renderers.
    protected void Setup(Game game, Screen screen)
    {
        _game = game;
        _screenStack = new(screen);
        _camera = Camera.Default;
        _state = RenderState.Default;
    }

    public IDisposable BeginPostProc(PostProcessEffect effect)
    {
        Screen tempScreen = TempScreenPool.Rent((int)(_screenStack.BaseScreenWidth * effect.ActualSizeScale), (int)(_screenStack.BaseScreenHeight * effect.ActualSizeScale));
        _screenStack.Push(tempScreen);
        tempScreen.Clear(Color.Transparent);

        return new PostProcessScope(this, _game, effect);
    }

    public void SetBlendMode(BlendMode blendMode)
    {
        _state.BlendMode = blendMode;
    }

    public void SetScreen(Screen screen)
    {
        if (_screenStack.Count >= 2)
        {
#if DEBUG
            DebugLog.Error("Renderer: Do not change the target screen while postprocessing.");
#endif
            return;
        }

        _screenStack.UpdateBaseScreen(screen);
    }

    protected void LoadState()
    {
        Screen screen = _screenStack.Peek();
        _state.LoadBlendMode();
        FramebufferService.PushAndBind(screen.ScreenId);
        int viewportWidth = screen.ScreenId == 0 ? _game.WindowWidth : screen.Width;
        int viewportHeight = screen.ScreenId == 0 ? _game.WindowHeight : screen.Height;
        GL.Viewport(0, 0, viewportWidth, viewportHeight);
    }

    protected void RestoreState()
    {
        FramebufferService.Pop();
    }
}