namespace GensouGL.Graphics;

internal static class TempScreenPool
{
    private static Dictionary<TempScreenParams, Stack<Screen>> _pools = new();

    public static Screen Rent(int width, int height)
    {
        return Rent(new TempScreenParams(width, height));
    }

    public static Screen Rent(TempScreenParams p)
    {
        if (_pools.TryGetValue(p, out Stack<Screen> stack))
        {
            if (stack.Count > 0)
                return stack.Pop();
        }
        else
        {
            _pools[p] = new();
        }

        Screen s = new(p.Width, p.Height);
        PersistentResourceService.Register(s);
        return s;
    }

    public static void Return(Screen screen)
    {
        _pools[new TempScreenParams(screen)].Push(screen);
    }
}

internal struct TempScreenParams(int w, int h) : IEquatable<TempScreenParams>
{
    public int Width = w;
    public int Height = h;

    public TempScreenParams(Screen source) : this(source.Width, source.Height)
    {
    }

    public bool Equals(TempScreenParams other)
    {
        return Width == other.Width && Height == other.Height;
    }

    public override bool Equals(object obj)
    {
        if (obj is TempScreenParams c)
        {
            return Equals(c);
        }
        else
        {
            return false;
        }
    }

    public override int GetHashCode()
    {
        return Width ^ Height;
    }
}