namespace GensouGL.Core;

internal sealed class GameControlContext : IControlContext
{
    private Game _game;
    private GameWindow _gameWindow;

    public GameControlContext(Game game)
    {
        _game = game;
        _gameWindow = game.Window;
    }

    public void SetFullscreenMode(bool fullscreen)
    {
        _gameWindow.WindowState = fullscreen ? WindowState.Fullscreen : WindowState.Normal;
        _game.ReloadWindowSize();
    }

    public void SetTitle(string title)
    {
        _gameWindow.Title = title;
    }

    public void SetFps(float fps)
    {
        _gameWindow.UpdateFrequency = fps;
    }

    public void ExitGame()
    {
        _game.ExitGame();
    }
}