namespace GensouGL.Graphics;

internal static class TextureService
{
    private static readonly Stack<int> _stack = new();
    private static int _boundTexture = 0;

    public static int BoundTexture => _boundTexture;

    public static void Bind(int id)
    {
        _boundTexture = id;
        GL.BindTexture(TextureTarget.Texture2D, id);
    }

    public static void Unbind()
    {
        _boundTexture = 0;
        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    public static void Push()
    {
        _stack.Push(_boundTexture);
    }

    public static void PushAndBind(int id)
    {
        Push();
        Bind(id);
    }

    public static void Pop()
    {
        _boundTexture = _stack.Pop();
        GL.BindTexture(TextureTarget.Texture2D, _boundTexture);
    }
}