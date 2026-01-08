namespace GensouGL.Core;

internal interface ISceneContext
{
    public int WindowWidth { get; }
    public int WindowHeight { get; }
    public int ScreenWidth { get; }
    public int ScreenHeight { get; }
    public InputService InputService { get; }
    public OverlayService OverlayService { get; }
    public ISceneNavigator SceneNavigator { get; }

    public void ExitGame();
    public void SetCursorVisibility(bool visible);
}