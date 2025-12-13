namespace GensouGL.Graphics;

public struct AnimationFrame(float duration, Rect source)
{
    public float Duration = duration;
    public Rect Source = source;
}