namespace GensouGL.Helpers;

public static class GensouMathF
{
    public static float Lerp(float from, float to, float pos)
    {
        return from * (1 - pos) + to * pos;
    }

    public static Color Lerp(Color from, Color to, float pos)
    {
        return Color.FromFloat(Lerp(from.Rf, to.Rf, pos), Lerp(from.Gf, from.Gf, pos), Lerp(from.Bf, to.Bf, pos), Lerp(from.Af, to.Af, pos));
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