namespace GensouGL.Core;

using OTK = OpenTK.Windowing.GraphicsLibraryFramework;

/// <summary>
/// Handles user input.
/// </summary>
/// <remarks>
/// This class does not provide any public constructors. To deal with user input, use protected property <see cref="Scene.Input"/> to access InputService instance.
/// </remarks>
public sealed class InputService
{
    private readonly Game _game;

    internal InputService(Game game)
    {
        _game = game;
    }

    /// <summary>
    /// Gets whether the specified key is pressed in the current frame.
    /// </summary>
    /// <param name="key">Target key.</param>
    /// <returns>Flag representing whether the specified key is pressed.</returns>
    public bool IsKeyPressed(Key key)
    {
        return _game.Window.IsKeyPressed((OTK.Keys)key);
    }

    /// <summary>
    /// Gets whether the specified key is released in the current frame.
    /// </summary>
    /// <param name="key">Target key.</param>
    /// <returns>Flag representing whether the specified key is released.</returns>
    public bool IsKeyReleased(Key key)
    {
        return _game.Window.IsKeyReleased((OTK.Keys)key);
    }

    /// <summary>
    /// Gets whether the specified key is down in the current frame.
    /// </summary>
    /// <param name="key">Target key.</param>
    /// <returns>Flag representing whether the specified key is down.</returns>
    public bool IsKeyDown(Key key)
    {
        return _game.Window.IsKeyDown((OTK.Keys)key);
    }

    /// <summary>
    /// Gets whether the specified mouse button is pressed in the current frame.
    /// </summary>
    /// <param name="button">Target mouse button.</param>
    /// <returns>Flag representing whether the specified mouse button is pressed.</returns>
    public bool IsMouseButtonPressed(Mouse button)
    {
        return _game.Window.IsMouseButtonPressed((OTK.MouseButton)button);
    }

    /// <summary>
    /// Gets whether the specified mouse button is released in the current frame.
    /// </summary>
    /// <param name="button">Target mouse button.</param>
    /// <returns>Flag representing whether the specified mouse button is released.</returns>
    public bool IsMouseButtonReleased(Mouse button)
    {
        return _game.Window.IsMouseButtonReleased((OTK.MouseButton)button);
    }

    /// <summary>
    /// Gets whether the specified mouse button is down in the current frame.
    /// </summary>
    /// <param name="button">Target mouse button.</param>
    /// <returns>Flag representing whether the specified mouse button is down.</returns>
    public bool IsMouseButtonDown(Mouse button)
    {
        return _game.Window.IsMouseButtonDown((OTK.MouseButton)button);
    }

    /// <summary>
    /// Gets the mouse position.
    /// </summary>
    /// <returns>Point structure representing the mouse position.</returns>
    public Point GetMousePosition()
    {
        var pos = _game.Window.MousePosition;
        return new Point((float)pos.X, (float)pos.Y);
    }

    /// <summary>
    /// Sets the mouse position.
    /// </summary>
    /// <param name="point">New mouse position.</param>
    public void SetMousePosition(Point point)
    {
        _game.Window.MousePosition = new(point.X, point.Y);
    }

    /// <summary>
    /// Gets the mouse position in screen scale.
    /// </summary>
    /// <remarks>
    /// For uses in cases where the window size and screen size are not same. This method returns the mouse position converted from window-space into screen-space.
    /// For example: WindowSize=(200, 200), ScreenSize=(100, 100)    if GetMousePosition()->(100, 100), GetScreenMousePosition()->(50, 50).
    /// </remarks>
    /// <returns>Point structure representing the mouse position.</returns>
    public Point GetScreenMousePosition()
    {
        Point pos = GetMousePosition();
        return new Point(pos.X / _game.WindowWidth * _game.ScreenWidth, pos.Y / _game.WindowHeight * _game.ScreenHeight);
    }

    /// <summary>
    /// Sets the mouse position in screen scale.
    /// </summary>
    /// <param name="point">New screen mouse point.</param>
    public void SetScreenMousePosition(Point point)
    {
        _game.Window.MousePosition = new(point.X / _game.ScreenWidth * _game.WindowWidth, point.Y / _game.ScreenHeight * _game.ScreenWidth);
    }

    /// <summary>
    /// Returns a new instance of TextInputListener.
    /// </summary>
    /// <returns>TextInputListener instance.</returns>
    public TextInputListener CreateTextInputListener()
    {
        TextInputListener listener = new();
        _game.RegisterTextInputListener(listener);
        return listener;
    }

    /// <summary>
    /// Unregisters the specified instance of TextInputListener.
    /// </summary>
    /// <param name="listener">Instance of TextInputListener to be unregistered.</param>
    public void DestroyTextInputListener(TextInputListener listener)
    {
        _game.UnregisterTextInputListener(listener);
    }
}