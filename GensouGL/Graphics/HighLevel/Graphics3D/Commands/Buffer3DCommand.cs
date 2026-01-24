namespace GensouGL.Graphics;

internal sealed class Buffer3DCommand : IDrawCommand3D
{
    private PrimitiveType _primitiveType;
    private Buffer3D _buffer;
    private Material _material;
    private Transform3D _transform;
    private ITexture _texture;

    public Material Material => _material;
    public Transform3D Transform => _transform;

    public Buffer3DCommand(PrimitiveType primitiveType, Material material, Transform3D transform, Buffer3D buffer)
    {
        _primitiveType = primitiveType;
        _material = material;
        _transform = transform.Clone();
        _buffer = buffer;
    }

    public Buffer3DCommand(PrimitiveType primitiveType, Material material, Transform3D transform, Buffer3D buffer, ITexture texture)
    {
        _primitiveType = primitiveType;
        _material = material;
        _transform = transform;
        _buffer = buffer;
        _texture = texture;
    }

    public void Prepare()
    {
        if (_texture is not null)
        {
            TextureService.PushAndBind(_texture.TextureId);
        }
    }

    public void Finish()
    {
        if (_texture is not null)
        {
            TextureService.Pop();
        }
    }

    public void MakeDrawCall()
    {
        _buffer.Draw(_primitiveType);
    }

    public Shader GetDepthShader()
    {
        return _buffer.Textured ? Shader.DepthTexture3D : Shader.Depth3D;
    }

    public Shader GetShader(LightingMode mode)
    {
        if (_buffer.Textured)
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