namespace GensouGL.IO;

/// <summary>
/// An intermediate class that manages files including resources.
/// </summary>
public static class GensouFileSystem
{
    private static List<string> _localUserRoots = new(16);

    /// <summary>
    /// Adds the specified root path as a local user root.
    /// </summary>
    /// <param name="rootPath">Root path.</param>
    public static void AddLocalUserRoot(string rootPath)
    {
        if (!Directory.Exists(rootPath))
        {
            DebugLog.Error($"{nameof(GensouFileSystem)}: Specified directory path '{rootPath}' was not found. User root has not been added.");
            return;
        }

        if (_localUserRoots.Contains(rootPath))
        {
            return;
        }

        _localUserRoots.Add(rootPath);
    }

    /// <summary>
    /// Removes the specified root path from the current local user roots.
    /// </summary>
    /// <param name="rootPath">Root path.</param>
    public static void RemoveLocalUserRoot(string rootPath)
    {
        _localUserRoots.Remove(rootPath);
    }

    //public static void AddArchive(Stream archive)


    internal static Stream OpenReadFile(string path)
    {
        if (File.Exists(path))
        {
            return File.OpenRead(path);
        }

        for (int i = 0; i < _localUserRoots.Count; i++)
        {
            string combined = Path.Combine(_localUserRoots[i], path);
            if (File.Exists(combined))
            {
                return File.OpenRead(combined);
            }
        }

        throw new FileNotFoundException(path);
    }

    internal static async Task<byte[]> ReadAllBytesAsync(string filename)
    {
        using var stream = OpenReadFile(filename);
        byte[] output = new byte[stream.Length];
        await stream.ReadExactlyAsync(output);
        return output;
    }
}