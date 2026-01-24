namespace GensouGL.Core;

/// <summary>
/// Represents 3D transformation data.
/// </summary>
public sealed class Transform3D
{
    /// <summary>
    /// Returns the identity transform.
    /// </summary>
    public static Transform3D Identity => new Transform3D
    {
        _translate = Vec3.Zero,
        _scale = new Vec3(1f, 1f, 1f)
    };

    private Vec3 _translate;
    private Vec3 _scale;

    /// <summary>
    /// Gets or sets the translation vector.
    /// </summary>
    public Vec3 Translate
    {
        get => _translate;
        set => _translate = value;
    }

    /// <summary>
    /// Gets or sets the scale vector.
    /// </summary>
    public Vec3 Scale
    {
        get => _scale;
        set => _scale = value;
    }

    internal OpenTK.Mathematics.Matrix4 GetModelMatrix()
    {
        OpenTK.Mathematics.Matrix4 translation = OpenTK.Mathematics.Matrix4.CreateTranslation(_translate.X, _translate.Y, _translate.Z);
        OpenTK.Mathematics.Matrix4 scale = OpenTK.Mathematics.Matrix4.CreateScale(_scale.X, _scale.Y, _scale.Z);

        return scale * translation;
    }

    internal Transform3D Clone()
    {
        return new Transform3D
        {
            _translate = this._translate,
            _scale = this._scale
        };
    }
}