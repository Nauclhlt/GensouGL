using GensouGL.Graphics.Models;

namespace GensouGL.Graphics;

internal sealed class StaticMesh3D : IDisposable
{
    private Buffer3D _buffer;
    private Material _material;
    private Texture _texture;

    private bool _disposed = false;

    public Buffer3D Buffer => _buffer;
    public Material Material => _material;
    public Texture Texture => _texture;
    public bool HasTexture => _texture is not null;

    private StaticMesh3D()
    {
    }

    internal StaticMesh3D(string parentDirectory, Ai.Scene scene, Ai.Mesh mesh)
    {
        Ai.Material meshMaterial = scene.Materials[mesh.MaterialIndex];
        _material = new()
        {
            Albedo = meshMaterial.ColorDiffuse.ToGensouColor(),
            CastShadows = true,
            Shininess = meshMaterial.Shininess,
        };

        Ai.TextureSlot diffuseTexture = meshMaterial.TextureDiffuse;

        int[] indices = mesh.GetIndices();
        _buffer = new(meshMaterial.HasTextureDiffuse, indices.Length + 10);
        for (int j = 0; j < indices.Length; j++)
        {
            int idx = indices[j];
            if (_buffer.Textured)
                _buffer.AddVertex(mesh.Vertices[idx].ToGensouVec3(), _material.Albedo, mesh.Normals[idx].ToGensouVec3(), mesh.TextureCoordinateChannels[diffuseTexture.UVIndex][idx].ToGensouVec3().XY);
            else
                _buffer.AddVertex(mesh.Vertices[idx].ToGensouVec3(), _material.Albedo, mesh.Normals[idx].ToGensouVec3());
        }
        _buffer.CreateBuffer();

        // load texture
        if (meshMaterial.HasTextureDiffuse)
        {
            string source = diffuseTexture.FilePath;
            if (source.StartsWith('*'))
            {
                // embedded
                int index = int.Parse(source[1..]);
                Ai.EmbeddedTexture embTexture = scene.Textures[index];
                if (embTexture.IsCompressed)
                    _texture = new Texture(new TextureSource(embTexture.CompressedData));
                else
                {
                    Ai.Texel[] data = embTexture.NonCompressedData;
                    byte[] pixels = new byte[embTexture.Width * embTexture.Height * 4];
                    for (int i = 0; i < data.Length; i++)
                    {
                        pixels[(i << 2)] = data[i].R;
                        pixels[(i << 2) + 1] = data[i].G;
                        pixels[(i << 2) + 2] = data[i].B;
                        pixels[(i << 2) + 3] = data[i].A;
                    }
                    _texture = Texture.LoadFromPixelArray(pixels, embTexture.Width, embTexture.Height);
                }
            }
            else
            {
                // from file
                string filePath = source;
                if (!File.Exists(filePath)) filePath = Path.Combine(parentDirectory, source.Replace('/', '\\'));
                DebugLog.Info(filePath);
                if (!File.Exists(filePath))
                {
                    DebugLog.Error($"{nameof(StaticMesh3D)}: Failed to load texture");
                }
                else
                {
                    _texture = new(filePath);
                }
            }
        }
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _buffer?.Dispose();
                _texture?.Dispose();
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~StaticMesh3D()
    {
        Dispose(false);
    }
}