namespace GensouGL.Core;

internal static class PersistentResourceService
{
    private static readonly List<IDisposable> _resources;

    static PersistentResourceService()
    {
        _resources = new List<IDisposable>();
    }

    public static void Register(IDisposable resource)
    {
        _resources.Add(resource);
    }

    public static void ReleaseAll()
    {
        for (int i = 0; i < _resources.Count; i++)
        {
            _resources[i].Dispose();
        }

        _resources.Clear();
    }
}