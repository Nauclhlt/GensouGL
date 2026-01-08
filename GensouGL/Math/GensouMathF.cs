namespace GensouGL.Helpers;

/// <summary>
/// Utility class for mathematics.
/// </summary>
public static class GensouMathF
{
    /// <summary>
    /// Returns the linear interpolation of the value from to to.
    /// </summary>
    /// <param name="from">From-value.</param>
    /// <param name="to">To-value.</param>
    /// <param name="pos">Position(0 to 1).</param>
    /// <returns>Interpolated value.</returns>
    public static float Lerp(float from, float to, float pos)
    {
        return from * (1 - pos) + to * pos;
    }

    /// <summary>
    /// Returns the linear interpolation of the value from to to.
    /// </summary>
    /// <param name="from">From-value.</param>
    /// <param name="to">To-value.</param>
    /// <param name="pos">Position(0 to 1).</param>
    /// <returns>Interpolated value.</returns>
    public static Color Lerp(Color from, Color to, float pos)
    {
        return new Color(Lerp(from.R, to.R, pos), Lerp(from.G, from.G, pos), Lerp(from.B, to.B, pos), Lerp(from.A, to.A, pos));
    }

    /// <summary>
    /// Converts the specified angle in degrees to in radians.
    /// </summary>
    /// <param name="degrees">Angle in degrees.</param>
    /// <returns>Angle in radians.</returns>
    public static float Rad(float degrees)
    {
        return degrees / 180f * MathF.PI;
    }

    /// <summary>
    /// Converts the specified angle in radians to in degrees.
    /// </summary>
    /// <param name="radians">Angle in radians.</param>
    /// <returns>Angle in degrees.</returns>
    public static float Deg(float radians)
    {
        return radians / MathF.PI * 180f;
    }
}