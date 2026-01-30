namespace GensouGL.Graphics.Models;

/// <summary>
/// Represents a static 3D model that can be loaded, rendered, and managed within a 3D graphics environment.
/// </summary>
/// <remarks>A static 3D model is a non-animated mesh used for rendering objects that do not require skeletal
/// animation or dynamic geometry. This class implements <see cref="IDisposable"/>, and instances should be disposed
/// when no longer needed to release any unmanaged resources associated with the model.</remarks>
public sealed class StaticModel3D : IDisposable
{
    private bool _disposed = false;

    private int _meshCount;
    private StaticMesh3D[] _meshes;

    /// <summary>
    /// Gets number of meshes in the model.
    /// </summary>
    public int MeshCount => _meshCount;
    internal StaticMesh3D[] Meshes => _meshes;

    private StaticModel3D()
    {

    }

    private StaticModel3D(int meshCount, StaticMesh3D[] meshes)
    {
        _meshCount = meshCount;
        _meshes = meshes;
    }

    /// <summary>
    /// Loads the static 3D model from the specified file.
    /// </summary>
    /// <param name="filename">Source file name.</param>
    /// <returns>Loaded 3D model.</returns>
    public static StaticModel3D Load(string filename)
    {
        string parentDirectory = Path.GetDirectoryName(Path.GetFullPath(filename));


        using var fileStream = GensouFileSystem.OpenReadFile(filename);
        using Ai.AssimpContext importer = new();
        Ai.Scene scene = importer.ImportFileFromStream(fileStream, Ai.PostProcessSteps.Triangulate |
            Ai.PostProcessSteps.GenerateSmoothNormals |
            Ai.PostProcessSteps.GenerateUVCoords |
            Ai.PostProcessSteps.FindDegenerates |
            Ai.PostProcessSteps.RemoveRedundantMaterials);

        int meshCount = scene.MeshCount;
        StaticMesh3D[] meshes = new StaticMesh3D[meshCount];
        for (int i = 0; i < scene.MeshCount; i++)
        {
            Ai.Mesh mesh = scene.Meshes[i];
            meshes[i] = new(parentDirectory, scene, mesh);
        }

        return new(meshCount, meshes);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                for (int i = 0; i < _meshes.Length; i++)
                {
                    _meshes[i]?.Dispose();
                }
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// Releases all resources used by <see cref="StaticModel3D"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// <see cref="StaticModel3D"/> destructor.
    /// </summary>
    ~StaticModel3D()
    {
        Dispose(false);
    }
}