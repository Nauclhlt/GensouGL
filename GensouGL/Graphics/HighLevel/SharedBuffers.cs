namespace GensouGL.Graphics;

internal static class SharedBuffers
{
    public static void Initialize()
    {
        _simpleBuffer3D = new();
        _simpleBuffer2D = new();
        _cubemapSkyboxBuffer = new();
    }

    public static void Release()
    {
        _simpleBuffer3D?.Dispose();
        _simpleBuffer2D?.Dispose();
        _cubemapSkyboxBuffer?.Dispose();
    }

    private static SimpleBuffer3D _simpleBuffer3D;
    private static SimpleBuffer2D _simpleBuffer2D;
    private static CubemapSkyboxBuffer _cubemapSkyboxBuffer;

    public static SimpleBuffer3D SimpleBuffer3D => _simpleBuffer3D;
    public static SimpleBuffer2D SimpleBuffer2D => _simpleBuffer2D;
    public static CubemapSkyboxBuffer CubemapSkyboxBuffer => _cubemapSkyboxBuffer;
}