namespace GensouGL.Graphics.Models;

/// <summary>
/// Provides functionality to encode 3D models for rendering using a specified rendering context.
/// </summary>
public sealed class ModelEncoder3D : GraphicsEncoder3D
{
    /// <summary>
    /// Initializes a new instance of <see cref="ModelEncoder3D"/> using the specified context.
    /// </summary>
    /// <param name="context">Rendering context.</param>
    public ModelEncoder3D(RenderContext3D context) : base(context)
    {
    }

    /// <summary>
    /// Encodes the specified 3D model for rendering.
    /// </summary>
    /// <param name="model">3D model to encode.</param>
    public void Encode(StaticModel3D model)
    {
        for (int i = 0; i < model.MeshCount; i++)
        {
            StaticMesh3D mesh = model.Meshes[i];
            Material previousMaterial = _material;
            if (mesh.Material is not null)
            {
                Material = mesh.Material;
            }
            Accumulate(new StaticMeshCommand(mesh, _transform));
            _material = previousMaterial;
        }
    }
}