namespace GensouGL.Graphics;

internal sealed class SimpleShapeCommand : IDrawCommand3D
{
    private PrimitiveType _primitiveType;
    private float[] _vertices;
    private float[] _colors;
    private float[] _normals;
    private Material _material;
    private Transform3D _transform;
    private RenderKey _key;

    public Material Material => _material;
    public Transform3D Transform => _transform;
    public RenderKey Key => _key;

    public SimpleShapeCommand(PrimitiveType primitiveType, Material material, Transform3D transform, RenderKey key, float[] va, float[] ca, float[] na)
    {
        _primitiveType = primitiveType;
        _material = material;
        _transform = transform.Clone();
        _key = key;
        _vertices = va;
        _colors = ca;
        _normals = na;
    }

    public void Prepare()
    {

    }

    public void Finish()
    {

    }

    public void MakeDrawCall()
    {
        SharedBuffers.SimpleBuffer3D.Draw(_primitiveType, _vertices, _colors, _normals);
    }

    public Shader GetDepthShader() => Shader.Depth3D;

    public Shader GetShader(LightingMode mode)
    {
        return mode switch
        {
            LightingMode.NoLighting => Shader.SimpleShape3D,
            LightingMode.Lighting => Shader.SimpleShape3DLighting,
            LightingMode.LightingWithShadows => Shader.SimpleShape3DLightingShadow,
            _ => Shader.SimpleShape3D
        };
    }
}