namespace GensouGL.Graphics;

internal static class SharedBuffers
{
    public static void Initialize()
    {
        _simpleBuffer3D = new();
    }

    public static void Release()
    {
        _simpleBuffer3D?.Dispose();
    }

    private static SimpleBuffer3D _simpleBuffer3D;

    public static SimpleBuffer3D SimpleBuffer3D => _simpleBuffer3D;
}