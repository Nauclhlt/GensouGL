namespace GensouGL.Core;

internal sealed class GameSceneContext : ISceneContext
{
    private Game _game;
    private IControlContext _controlContext;

    public GameSceneContext(Game game)
    {
        _game = game;
        _controlContext = new GameControlContext(game);
    }

    public int ScreenWidth => _game.ScreenWidth;
    public int ScreenHeight => _game.ScreenHeight;
    public int WindowWidth => _game.WindowWidth;
    public int WindowHeight => _game.WindowHeight;
    public ISceneNavigator SceneNavigator => _game.SceneManager;
    public OverlayService OverlayService => _game.OverlayService;
    public InputService InputService => _game.InputService;
    public IControlContext ControlContext => _controlContext;

    public void SetCursorVisibility(bool visible)
    {
        _game.SetCursorVisibility(visible);
    }
}