namespace GensouGL.Graphics;

public abstract class Renderer
{
    protected Game _game;
    protected Screen _screen;
    protected Camera _camera;
    protected RenderState _state;
    private OpenTK.Mathematics.Matrix4 _projMatrix;

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
        return _projMatrix * _camera.GetModelMatrix();
    }

    // called from constructors of derivative classes
    protected void Setup(Game game, Screen screen)
    {
        _game = game;
        _screen = screen;
        _camera = Camera.Default;
        _state = RenderState.Default;
        int w = _screen.ScreenId == 0 ? _game.ScreenWidth : _screen.Width;
        int h = _screen.ScreenId == 0 ? _game.ScreenHeight : _screen.Height;
        _projMatrix = OpenTK.Mathematics.Matrix4.CreateOrthographicOffCenter(0, w, h, 0, -1, 1);
    }

    public void SetBlendMode(BlendMode blendMode)
    {
        _state.BlendMode = blendMode;
    }

    protected void LoadState()
    {
        _state.LoadBlendMode();
        FramebufferService.PushAndBind(_screen.ScreenId);
    }

    protected void RestoreState()
    {
        FramebufferService.Pop();
    }
}