namespace GensouGL.Core;

// TODO: Add input.
public class Overlay : Scene
{
    public new static Overlay Empty { get; } = new Overlay();

    internal bool Closing { get; set; } = false;

    protected internal void Close()
    {
        Closing = true;
    }
}