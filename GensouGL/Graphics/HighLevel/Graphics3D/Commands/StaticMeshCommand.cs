namespace GensouGL.Graphics.Models;

internal sealed class StaticMeshCommand : IDrawCommand3D
{
    private StaticMesh3D _mesh;
    private Transform3D _transform;
    private RenderKey _key;

    public Material Material => _mesh.Material;
    public Transform3D Transform => _transform;
    public RenderKey Key => _key;

    public StaticMeshCommand(StaticMesh3D mesh, Transform3D transform, RenderKey key)
    {
        _mesh = mesh;
        _transform = transform.Clone();
        _key = key;
    }

    public void Prepare()
    {
        if (_mesh.HasTexture)
        {
            TextureService.PushAndBind(_mesh.Texture.TextureId);
        }
    }

    public void Finish()
    {
        if (_mesh.HasTexture)
        {
            TextureService.Pop();
        }
    }

    public void MakeDrawCall()
    {
        _mesh.Buffer.Draw(PrimitiveType.Triangles);
    }

    public Shader GetDepthShader()
    {
        return _mesh.HasTexture ? Shader.DepthTexture3D : Shader.Depth3D;
    }

    public Shader GetShader(LightingMode mode)
    {
        if (_mesh.HasTexture)
        {
            return mode switch
            {
                LightingMode.NoLighting => Shader.Texture3D,
                LightingMode.Lighting => Shader.Texture3DLighting,
                LightingMode.LightingWithShadows => Shader.Texture3DLightingShadow,
                _ => Shader.SimpleShape3D
            };
        }
        else
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
}