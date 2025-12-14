namespace GensouGL.Graphics;

public sealed class ScreenStack
{
    private List<Screen> _stack;

    public Screen BaseScreen => _stack[0];
    public int BaseScreenWidth => _stack[0].Width;
    public int BaseScreenHeight => _stack[0].Height;
    public int Count => _stack.Count;


    public ScreenStack(Screen baseScreen)
    {
        _stack = [baseScreen];
    }

    public void Push(Screen screen)
    {
        _stack.Add(screen);
    }

    public Screen Peek()
    {
        return _stack[^1];
    }

    public Screen Pop()
    {
        Screen s = Peek();
        if (_stack.Count == 1) return s;
        _stack.RemoveAt(_stack.Count - 1);
        return s;
    }

    public void Clear()
    {
        Screen b = _stack[0];
        _stack.Clear();
        _stack.Add(b);
    }

    public void UpdateBaseScreen(Screen screen)
    {
        _stack[0] = screen;
    }
}