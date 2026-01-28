namespace GensouGL.Core;

internal static class ListPool<T>
{
    private static Stack<List<T>> _pool;

    static ListPool()
    {
        _pool = new Stack<List<T>>(16);
    }

    public static List<T> Rent()
    {
        if (_pool.Count > 0)
        {
            return _pool.Pop();
        }
        else
        {
            return new(32);
        }
    }

    public static void Return(List<T> list)
    {
        if (list is null) return;
        list.Clear();
        _pool.Push(list);
    }
}