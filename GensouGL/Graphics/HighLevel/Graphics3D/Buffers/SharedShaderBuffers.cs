namespace GensouGL.Graphics;

internal static class SharedShaderBuffers
{
    public static void Initialize()
    {
        _lightBlockUBO = new();
        _materialUBO = new();
        _pointLightsSSBO = new();
        _lightIndicesSSBO = new();
        _tileRangesSSBO = new();
        _colorGradingUBO = new();
    }

    public static void Release()
    {
        _lightBlockUBO.Dispose();
        _materialUBO.Dispose();
        _pointLightsSSBO.Dispose();
        _lightIndicesSSBO.Dispose();
        _tileRangesSSBO.Dispose();
        _colorGradingUBO.Dispose();
    }

    private static StructureUniformBuffer<GpuLightBlock> _lightBlockUBO;
    private static StructureUniformBuffer<GpuMaterial> _materialUBO;
    private static ArrayShaderStorageBuffer<GpuPointLight> _pointLightsSSBO;
    private static ArrayShaderStorageBuffer<int> _lightIndicesSSBO;
    private static ArrayShaderStorageBuffer<int> _tileRangesSSBO;
    private static StructureUniformBuffer<GpuColorGrading> _colorGradingUBO;

    public static StructureUniformBuffer<GpuLightBlock> LightBlockUBO => _lightBlockUBO;
    public static StructureUniformBuffer<GpuMaterial> MaterialUBO => _materialUBO;
    public static ArrayShaderStorageBuffer<GpuPointLight> PointLightsSSBO => _pointLightsSSBO;
    public static ArrayShaderStorageBuffer<int> LightIndicesSSBO => _lightIndicesSSBO;
    public static ArrayShaderStorageBuffer<int> TileRangesSSBO => _tileRangesSSBO;
    public static StructureUniformBuffer<GpuColorGrading> ColorGradingUBO => _colorGradingUBO;
}