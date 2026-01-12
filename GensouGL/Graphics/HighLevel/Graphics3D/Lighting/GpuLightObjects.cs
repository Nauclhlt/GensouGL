namespace GensouGL.Graphics;

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct GpuLightBlock
{
    public const int MaxLights = 16;
    public const int BRDF_PHONG = 0;
    public const int BRDF_PBR = 1;

    public OpenTK.Mathematics.Vector3 ViewPosition;
    public int Brdf;

    public OpenTK.Mathematics.Vector4 AmbientColorIntensity;

    // 4 bytes
    public int PointLightCount;
    // 4 bytes
    public int DirectionalLightCount;
    // 8 bytes
    public OpenTK.Mathematics.Vector2i TileCount;

    public fixed float DirectionalLights[MaxLights * 8];
}

[StructLayout(LayoutKind.Sequential)]
internal readonly struct GpuPointLight
{
    public readonly OpenTK.Mathematics.Vector4 PositionRange;
    public readonly OpenTK.Mathematics.Vector4 ColorIntensity;

    public GpuPointLight(PointLight source)
    {
        PositionRange = new(source.Position.ToOpenTK(), source.Range);
        ColorIntensity = new(source.Color.R, source.Color.G, source.Color.B, source.Intensity);
    }
}