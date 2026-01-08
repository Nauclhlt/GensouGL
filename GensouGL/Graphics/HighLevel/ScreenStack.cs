namespace GensouGL.Graphics;

/// <summary>
/// Represents a screen stack.
/// </summary>
public sealed class ScreenStack
{
    private List<Screen> _stack;

    /// <summary>
    /// Gets the base screen.
    /// </summary>
    public Screen BaseScreen => _stack[0];
    /// <summary>
    /// Gets the width of the base screen.
    /// </summary>
    public int BaseScreenWidth => _stack[0].Width;
    /// <summary>
    /// Gets the height of the base screen.
    /// </summary>
    public int BaseScreenHeight => _stack[0].Height;
    /// <summary>
    /// Gets the screen count.
    /// </summary>
    public int Count => _stack.Count;

    /// <summary>
    /// Initializes a new instance of <see cref="ScreenStack"/> with the base screen.
    /// </summary>
    /// <param name="baseScreen">Base screen.</param>
    public ScreenStack(Screen baseScreen)
    {
        _stack = [baseScreen];
    }

    /// <summary>
    /// Pushes the specified screen on the stack.
    /// </summary>
    /// <param name="screen">Target screen.</param>
    public void Push(Screen screen)
    {
        _stack.Add(screen);
    }

    /// <summary>
    /// Gets the top screen.
    /// </summary>
    /// <returns>Top screen.</returns>
    public Screen Peek()
    {
        return _stack[^1];
    }

    /// <summary>
    /// Pops the screen from the stack.
    /// </summary>
    /// <returns>Popped screen.</returns>
    /// <remarks>Note that this function does not pops the screen and instead works the same as <see cref="Peek()"/> when the screen count is 1.</remarks>
    public Screen Pop()
    {
        Screen s = Peek();
        if (_stack.Count == 1) return s;
        _stack.RemoveAt(_stack.Count - 1);
        return s;
    }

    /// <summary>
    /// Clears the screen stack.
    /// </summary>
    public void Clear()
    {
        Screen b = _stack[0];
        _stack.Clear();
        _stack.Add(b);
    }

    /// <summary>
    /// Updates the base screen.
    /// </summary>
    /// <param name="screen">New base screen.</param>
    public void UpdateBaseScreen(Screen screen)
    {
        _stack[0] = screen;
    }
}