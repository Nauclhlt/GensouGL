namespace GensouGL.Core;

public sealed class SceneManager
{
    private Scene _currentScene;
    private Game _game;

    public Scene CurrentScene => _currentScene;

    internal SceneManager(Game game)
    {
        _currentScene = Scene.Empty;
        _game = game;
    }

    public void Navigate(Scene scene)
    {
        _currentScene.OnLeave();
        _currentScene = scene;
        if (!_currentScene.Initialized)
        {
            _currentScene._sceneManager = this;
            _currentScene._overlayService = _game._overlayService;
            _currentScene._input = _game._inputService;
            _currentScene._game = _game;
            _currentScene.OnInitialize();
            _currentScene.Initialized = true;
        }
        _currentScene.OnEnter();
    }

    internal void Update(float deltaTime)
    {
        _currentScene.Update(deltaTime);
    }

    internal void FocusUpdate(float deltaTime)
    {
        _currentScene.FocusUpdate(deltaTime);
    }

    internal void Render(float deltaTime)
    {
        _currentScene.Render(deltaTime);
    }

    internal void FocusRender(float deltaTime)
    {
        _currentScene.FocusRender(deltaTime);
    }
}