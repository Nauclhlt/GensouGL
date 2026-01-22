namespace GensouGL.Core;

/// <summary>
/// Represents a 3D camera.
/// </summary>
public sealed class Camera3D
{
    /// <summary>
    /// Gets the default camera.
    /// </summary>
    public static Camera3D Default { get; } = new(new Vec3(0, 0, 0), new Vec3(0, 0, 1), new Vec3(0, 1, 0));

    private Vec3 _position;
    private Vec3 _target;
    private Vec3 _up;
    private float _fovy = MathF.PI / 2;
    private float _aspect = 16f / 9f;
    private float _zNear = 0.5f;
    private float _zFar = 1000f;

    /// <summary>
    /// Gets or sets the position of the camera.
    /// </summary>
    public Vec3 Position
    {
        get => _position;
        set => _position = value;
    }

    /// <summary>
    /// Gets or sets the target of the camera.
    /// </summary>
    public Vec3 Target
    {
        get => _target;
        set => _target = value;
    }

    /// <summary>
    /// Gets or sets the up vector of the camera.
    /// </summary>
    public Vec3 Up
    {
        get => _up;
        set => _up = value;
    }

    /// <summary>
    /// Gets or sets the y-direction fov. Value is clamped to the range[0, 3.14...].
    /// </summary>
    public float FovY
    {
        get => _fovy;
        set
        {
            _fovy = float.Clamp(value, 0, MathF.PI);
        }
    }

    /// <summary>
    /// Gets or sets the aspect ratio (width/height). Value must be positive.
    /// </summary>
    public float Aspect
    {
        get => _aspect;
        set
        {
            if (value <= 0f) return;
            _aspect = value;
        }
    }

    /// <summary>
    /// Gets or sets the Z near value. Value must be positive.
    /// </summary>
    public float ZNear
    {
        get => _zNear;
        set
        {
            if (value <= 0f) return;
            _zNear = value;
        }
    }

    /// <summary>
    /// Gets or sets the z far value. Value must be positive.
    /// </summary>
    public float ZFar
    {
        get => _zFar;
        set
        {
            if (value <= 0f) return;
            _zFar = value;
        }
    }

    /// <summary>
    /// Gets the forward vector of the camera.
    /// </summary>
    public Vec3 Forward => (_target - _position).Normalize();
    /// <summary>
    /// Gets the right vector of the camera.
    /// </summary>
    public Vec3 Right => Vec3.Cross(Forward, _up).Normalize();

    /// <summary>
    /// Initializes a new instance of <see cref="Camera3D"/> with the specified parameters.
    /// </summary>
    /// <param name="position">Camera position.</param>
    /// <param name="target">Camera target.</param>
    /// <param name="up">Camera up vector.</param>
    public Camera3D(Vec3 position, Vec3 target, Vec3 up)
    {
        _position = position;
        _target = target;
        _up = up;
    }

    internal Vec3[] GetFrustumCorners()
    {
        Vec3[] corners = new Vec3[8];

        Vec3 forward = (_target - _position).Normalize();
        Vec3 right = Vec3.Cross(forward, _up).Normalize();
        Vec3 up = Vec3.Cross(right, forward).Normalize();

        float tanHalfFovy = MathF.Tan(_fovy / 2f);

        float nearHeight = 2f * tanHalfFovy * _zNear;
        float nearWidth = nearHeight * _aspect;

        float farHeight = 2f * tanHalfFovy * _zFar;
        float farWidth = farHeight * _aspect;

        Vec3 nearCenter = _position + forward * _zNear;
        Vec3 farCenter = _position + forward * _zFar;

        Vec3 nearUpVec = up * (nearHeight / 2f);
        Vec3 nearRightVec = right * (nearWidth / 2f);
        Vec3 farUpVec = up * (farHeight / 2f);
        Vec3 farRightVec = right * (farWidth / 2f);

        corners[0] = nearCenter + nearUpVec - nearRightVec; // Top-Left
        corners[1] = nearCenter + nearUpVec + nearRightVec; // Top-Right
        corners[2] = nearCenter - nearUpVec + nearRightVec; // Bottom-Right
        corners[3] = nearCenter - nearUpVec - nearRightVec; // Bottom-Left

        corners[4] = farCenter + farUpVec - farRightVec;    // Top-Left
        corners[5] = farCenter + farUpVec + farRightVec;    // Top-Right
        corners[6] = farCenter - farUpVec + farRightVec;    // Bottom-Right
        corners[7] = farCenter - farUpVec - farRightVec;    // Bottom-Left

        return corners;
    }

    internal OpenTK.Mathematics.Matrix4 GetViewMatrix()
    {
        return OpenTK.Mathematics.Matrix4.LookAt(_position.ToOpenTK(), _target.ToOpenTK(), _up.ToOpenTK());
    }

    internal OpenTK.Mathematics.Matrix4 GetProjectionMatrix()
    {
        return OpenTK.Mathematics.Matrix4.CreatePerspectiveFieldOfView(_fovy, _aspect, _zNear, _zFar);
    }
}