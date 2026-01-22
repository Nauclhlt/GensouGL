namespace GensouGL.Graphics;

internal static class TextureService
{
    private const int TextureSlotCount = 10;

    private static readonly Stack<int>[] _stack;
    private static int[] _boundTexture;


    
    static TextureService()
    {
        _stack = new Stack<int>[TextureSlotCount];
        for (int i = 0; i < _stack.Length; i++)
        {
            _stack[i] = new();
        }
        _boundTexture = new int[TextureSlotCount];
    }

    public static void Bind(int id, int slot = 0)
    {
        _boundTexture[slot] = id;
        GL.ActiveTexture(TextureUnit.Texture0 + slot);
        GL.BindTexture(TextureTarget.Texture2D, id);
    }

    public static void Unbind(int slot = 0)
    {
        _boundTexture[slot] = 0;
        GL.ActiveTexture(TextureUnit.Texture0 + slot);
        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    public static void Push(int slot = 0)
    {
        _stack[slot].Push(_boundTexture[slot]);
    }

    public static void PushAndBind(int id, int slot = 0)
    {
        Push(slot);
        Bind(id, slot);
    }

    public static void Pop(int slot = 0)
    {
        _boundTexture[slot] = _stack[slot].Pop();
        GL.ActiveTexture(TextureUnit.Texture0 + slot);
        GL.BindTexture(TextureTarget.Texture2D, _boundTexture[slot]);
    }

    public static int GetBound(int slot)
    {
        return _boundTexture[slot];
    }
}