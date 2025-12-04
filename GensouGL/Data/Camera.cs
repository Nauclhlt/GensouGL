namespace GensouGL.Core;

using Mat4 = OpenTK.Mathematics.Matrix4;

public sealed class Camera
{
    public static Camera Default { get; } = new(0, 0, 1, 1, 0);

    private float _translateX = 0f;
    private float _translateY = 0f;
    private float _scaleX = 1.0f;
    private float _scaleY = 1.0f;
    // Radians
    private float _rotation = 0f;
    
    public float TranslateX
    {
        get => _translateX;
        set => _translateX = value;
    }

    public float TranslateY
    {
        get => _translateY;
        set => _translateY = value;
    }

    public float ScaleX
    {
        get => _scaleX;
        set => _scaleX = value;
    }

    public float ScaleY
    {
        get => _scaleY;
        set => _scaleY = value;
    }

    public float Rotation
    {
        get => _rotation;
        set => _rotation = value;
    }

    public Camera(float translateX, float translateY, float scaleX, float scaleY, float rotation)
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