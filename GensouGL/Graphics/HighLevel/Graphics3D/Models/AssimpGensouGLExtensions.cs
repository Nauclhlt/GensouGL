namespace GensouGL.Graphics.Models;

internal static class AssimpGensouGLExtensions
{
    public static Color ToGensouColor(this Ai.Color4D color)
    {
        return new Color(color.R, color.G, color.B, color.A);
    }
    public static Vec3 ToGensouVec3(this Ai.Vector3D vector)
    {
        return new Vec3(vector.X, vector.Y, vector.Z);
    }
    public static Vec2 ToGensouVec2(this Ai.Vector2D vector)
    {
        return new Vec2(vector.X, vector.Y);
    }
}