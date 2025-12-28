namespace GensouGL.Helpers;

public static class GensouMathF
{
    public static float Lerp(float from, float to, float pos)
    {
        return from * (1 - pos) + to * pos;
    }

    public static Color Lerp(Color from, Color to, float pos)
    {
        return Color.FromFloat(Lerp(from.R, to.R, pos), Lerp(from.G, from.G, pos), Lerp(from.B, to.B, pos), Lerp(from.A, to.A, pos));
    }

    public static float Rad(float degrees)
    {
        return degrees / 180f * MathF.PI;
    }

    public static float Deg(float radians)
    {
        return radians / MathF.PI * 180f;
    }
}