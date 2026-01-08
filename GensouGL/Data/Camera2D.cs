namespace GensouGL.Core;

using Mat4 = OpenTK.Mathematics.Matrix4;

/// <summary>
/// Represents a 2D camera.
/// </summary>
public sealed class Camera2D
{
    /// <summary>
    /// Gets the default camera, with no translation, rotation, or scale modification.
    /// </summary>
    public static Camera2D Default { get; } = new(0, 0, 1, 1, 0);

    private float _translateX = 0f;
    private float _translateY = 0f;
    private float _scaleX = 1.0f;
    private float _scaleY = 1.0f;
    // Radians
    private float _rotation = 0f;
    
    /// <summary>
    /// Gets or sets the X translation.
    /// </summary>
    public float TranslateX
    {
        get => _translateX;
        set => _translateX = value;
    }

    /// <summary>
    /// Gets or sets the Y translation.
    /// </summary>
    public float TranslateY
    {
        get => _translateY;
        set => _translateY = value;
    }

    /// <summary>
    /// Gets or sets the translation.
    /// </summary>
    public Vec2 Translate
    {
        get => new(_translateX, _translateY);
        set
        {
            _translateX = value.X;
            _translateY = value.Y;
        }
    }

    /// <summary>
    /// Gets or sets the X scale.
    /// </summary>
    public float ScaleX
    {
        get => _scaleX;
        set => _scaleX = value;
    }

    /// <summary>
    /// Gets or sets the Y scale.
    /// </summary>
    public float ScaleY
    {
        get => _scaleY;
        set => _scaleY = value;
    }

    /// <summary>
    /// Gets or sets the scale.
    /// </summary>
    public Vec2 Scale
    {
        get => new(_scaleX, _scaleY);
        set
        {
            _scaleX = value.X;
            _scaleY = value.Y;
        }
    }

    /// <summary>
    /// Gets or sets the rotation angle in radians.
    /// </summary>
    public float Rotation
    {
        get => _rotation;
        set => _rotation = value;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Camera2D"/> using the specified parameters.
    /// </summary>
    /// <param name="translateX">X translation.</param>
    /// <param name="translateY">Y translation.</param>
    /// <param name="scaleX">X scale.</param>
    /// <param name="scaleY">Y scale.</param>
    /// <param name="rotation">Rotation angle in radians.</param>
    public Camera2D(float translateX, float translateY, float scaleX, float scaleY, float rotation)
    {
        TranslateX = translateX;
        TranslateY = translateY;
        ScaleX = scaleX;
        ScaleY = scaleY;
        Rotation = rotation;
    }

    internal Mat4 GetModelMatrix()
    {
        return Mat4.CreateScale(_scaleX, _scaleY, 1.0f) * Mat4.CreateRotationZ(_rotation) * Mat4.CreateTranslation(_translateX, _translateY, 0f);        
    }
}