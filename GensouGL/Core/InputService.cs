namespace GensouGL.Core;

using OTK = OpenTK.Windowing.GraphicsLibraryFramework;

public sealed class InputService
{
    private readonly Game _game;

    internal InputService(Game game)
    {
        _game = game;
    }

    public bool IsKeyPressed(Key key)
    {
        return _game._window.IsKeyPressed((OTK.Keys)key);
    }

    public bool IsKeyReleased(Key key)
    {
        return _game._window.IsKeyReleased((OTK.Keys)key);
    }

    public bool IsKeyDown(Key key)
    {
        return _game._window.IsKeyDown((OTK.Keys)key);
    }

    public bool IsMouseButtonPressed(Mouse button)
    {
        return _game._window.IsMouseButtonPressed((OTK.MouseButton)button);
    }

    public bool IsMouseButtonReleased(Mouse button)
    {
        return _game._window.IsMouseButtonReleased((OTK.MouseButton)button);
    }

    public bool IsMouseButtonDown(Mouse button)
    {
        return _game._window.IsMouseButtonDown((OTK.MouseButton)button);
    }

    public Point GetMousePosition()
    {
        var pos = _game._window.MousePosition;
        return new Point((float)pos.X, (float)pos.Y);
    }
}