namespace GensouGL.Graphics;

internal static class FramebufferService
{
    private static readonly Stack<int> _stack = new();
    private static int _boundFramebuffer = 0;

    public static int BoundFramebuffer => _boundFramebuffer;

    public static void Bind(int id)
    {
        _boundFramebuffer = id;
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, id);
    }

    public static void Unbind()
    {
        _boundFramebuffer = 0;
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }

    public static void Push()
    {
        _stack.Push(_boundFramebuffer);
    }

    public static void PushAndBind(int id)
    {
        Push();
        Bind(id);
    }

    public static void Pop()
    {
        _boundFramebuffer = _stack.Pop();
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _boundFramebuffer);
    }
}