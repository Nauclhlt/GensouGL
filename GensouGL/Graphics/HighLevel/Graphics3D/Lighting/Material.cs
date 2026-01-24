namespace GensouGL.Graphics;

/// <summary>
/// Represents a material.
/// </summary>
public sealed class Material
{
    /// <summary>
    /// Gets the default material.
    /// </summary>
    public static Material Default { get; } = new();

    private float _shininess = 32f;
    private float _metallic = 0.5f;
    private float _roughness = 0.5f;
    private Color _albedo = Color.White;
    private bool _castShadows = true;
    private bool _receiveShadows = true;
    
    /// <summary>
    /// Gets or sets the shininess.
    /// </summary>
    public float Shininess
    {
        get => _shininess;
        set => _shininess = value;
    }

    /// <summary>
    /// Gets or sets the metallic value.
    /// </summary>
    public float Metallic
    {
        get => _metallic;
        set => _metallic = float.Clamp(value, 0f, 1f);
    }

    /// <summary>
    /// Gets or sets the roughness.
    /// </summary>
    public float Roughness
    {
        get => _roughness;
        set => _roughness = float.Clamp(value, 0f, 1f);
    }

    /// <summary>
    /// Gets or sets the albedo color.
    /// </summary>
    public Color Albedo
    {
        get => _albedo;
        set => _albedo = value;
    }

    /// <summary>
    /// Gets or sets if the objects with this material should cast shadows.
    /// </summary>
    public bool CastShadows
    {
        get => _castShadows;
        set => _castShadows = value;
    }

    /// <summary>
    /// Gets or sets if the objects with this material should receive shadows.
    /// </summary>
    public bool ReceiveShadows
    {
        get => _receiveShadows;
        set => _receiveShadows = value;
    }

    internal GpuMaterial ToGpuMaterial()
    {
        return new GpuMaterial()
        {
            Shininess = _shininess,
            Metallic = _metallic,
            Roughness = _roughness,
            Albedo = (OpenTK.Mathematics.Vector4)_albedo.ToOpenTKColor4()
        };
    }
}

[StructLayout(LayoutKind.Sequential)]
internal struct GpuMaterial
{
    public float Shininess;
    public float Metallic;
    public float Roughness;
    private float _pad0;
    public OpenTK.Mathematics.Vector4 Albedo;
}