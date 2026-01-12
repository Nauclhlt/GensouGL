namespace GensouGL.Graphics;

/// <summary>
/// Represents a lighting frame.
/// </summary>
public sealed class LightingFrame
{
    /// <summary>
    /// Gets the default lighting frame.
    /// </summary>
    public static LightingFrame Default { get; } = new(new AmbientLight(Color.White, 0.2f));

    private AmbientLight _ambientLight;
    private IReadOnlyList<DirectionalLight> _directionalLights;
    private IReadOnlyList<PointLight> _pointLights;

    /// <summary>
    /// Gets or sets the ambient light.
    /// </summary>
    public AmbientLight Ambient
    {
        get => _ambientLight;
        set => _ambientLight = value;
    }

    /// <summary>
    /// Gets or sets the list of the directional lights.
    /// </summary>
    public IReadOnlyList<DirectionalLight> DirectionalLights
    {
        get => _directionalLights; 
        set
        {
            if (value is null) return;
            _directionalLights = value;
        }
    }

    /// <summary>
    /// Gets or sets the light of the point lights.
    /// </summary>
    public IReadOnlyList<PointLight> PointLights
    {
        get => _pointLights;
        set
        {
            if (value is null) return;
            _pointLights = value;
        }
    }

    /// <summary>
    /// Initializes a new instance of <see cref="LightingFrame"/> with the specified ambient.
    /// </summary>
    /// <param name="ambient">Ambient light.</param>
    public LightingFrame(AmbientLight ambient)
    {
        _ambientLight = ambient;
        _directionalLights = [];
        _pointLights = [];
    }
}