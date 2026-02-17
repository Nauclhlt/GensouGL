namespace GensouGL.Core;

internal interface IControlContext
{
    public void SetFullscreenMode(bool fullscreen);
    public void SetFps(float fps);
    public void SetTitle(string title);
    public void ExitGame();
}