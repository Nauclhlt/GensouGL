namespace GensouGL.Graphics;

public abstract class Renderer
{
    protected Game _game;
    protected Screen _screen;
    protected Camera _camera;
    private OpenTK.Mathematics.Matrix4 _projMatrix;

    public Camera Camera
    {
        get => _camera;
        set
        {
            _camera = value is not null ? value : Camera.Default;
        }
    }

    protected OpenTK.Mathematics.Matrix4 GetDefaultVMatrix()
    {
        return _projMatrix * _camera.GetModelMatrix();
    }

    protected void Setup(Game game, Screen screen)
    {
        _game = game;
        _screen = screen;
        _camera = Camera.Default;
        int w = _screen.ScreenId == 0 ? _game.ScreenWidth : _screen.Width;
        int h = _screen.ScreenId == 0 ? _game.ScreenHeight : _screen.Height;
        _projMatrix = OpenTK.Mathematics.Matrix4.CreateOrthographicOffCenter(0, w, h, 0, -1, 1);
    }
}