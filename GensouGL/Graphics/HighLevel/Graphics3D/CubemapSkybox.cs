namespace GensouGL.Graphics;

using System.Diagnostics.Tracing;
using Assimp.Configs;
using OpenTK.Mathematics;
using OpenTK.Platform.Windows;
using SixLabors.ImageSharp.Processing;
using IMS = SixLabors.ImageSharp;
using IMSP = SixLabors.ImageSharp.PixelFormats;

/// <summary>
/// Represents a skybox using cubemap textures.
/// </summary>
public sealed class CubemapSkybox : IDisposable
{
    private bool _disposed = false;
    private int _cubemapTexture;

    private TextureResizeMode _resizeMode = TextureResizeMode.Bilinear;

    /// <summary>
    /// Gets or sets the texture resize mode.
    /// </summary>
    public TextureResizeMode ResizeMode
    {
        get => _resizeMode;
        set
        {
            if (_disposed) return;
            GL.BindTexture(TextureTarget.TextureCubeMap, _cubemapTexture);

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)_resizeMode);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)_resizeMode);

            GL.BindTexture(TextureTarget.TextureCubeMap, 0);
        }
    }

    internal int CubemapTextureId => _cubemapTexture;

    /// <summary>
    /// Initializes a new instance of <see cref="CubemapSkybox"/> using the specified texture sources.
    /// </summary>
    /// <remarks>Texture order: (right, left, top, bottom, front, back)</remarks>
    /// <param name="sources">Texture sources in the order: (right, left, top, bottom, front, back)</param>
    public CubemapSkybox(TextureSource[] sources)
    {
        if (sources.Length != 6)
        {
            throw new ArgumentException("Length of the source array must be 6.");
        }

        IMS.Image<IMSP.Rgba32>[] images = new IMS.Image<IMSP.Rgba32>[sources.Length];
        for (int i = 0; i < sources.Length; i++)
        {
            images[i] = sources[i].LoadImageSharpImage();
        }

        try
        {
            int size = 0;
            for (int i = 0; i < images.Length; i++)
            {
                if (size == 0)
                {
                    size = images[i].Width;
                }

                if (images[i].Width != size || images[i].Height != size)
                {
                    throw new ArgumentException("Each texture must have the uniform size.");
                }
            }

            _cubemapTexture = GL.GenTexture();

            GL.BindTexture(TextureTarget.TextureCubeMap, _cubemapTexture);

            for (int i = 0; i < images.Length; i++)
            {
                byte[] data = new byte[images[i].Width * images[i].Height * 4];
                images[i].CopyPixelDataTo(data);
                GL.TexImage2D((TextureTarget)(TextureTarget.TextureCubeMapPositiveX + i), 0, PixelInternalFormat.Rgba, images[i].Width, images[i].Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
            }

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);

            GL.BindTexture(TextureTarget.TextureCubeMap, 0);
        }
        finally
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i]?.Dispose();
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of <see cref="CubemapSkybox"/> using the specified texture source and rectangles.
    /// </summary>
    /// <remarks>Texture order: (right, left, top, bottom, front, back)</remarks>
    /// <param name="source">Texture source.</param>
    /// <param name="points">Top left point of texture rectangles in the order: (right, left, top, bottom, front, back)</param>
    /// <param name="size">Size of each image.</param>
    public CubemapSkybox(TextureSource source, Point[] points, int size)
    {
        if (size <= 0)
        {
            throw new ArgumentException($"{nameof(size)} must be a positive integer.");
        }

        if (points.Length != 6)
        {
            throw new ArgumentException("Length of the source array must be 6.");
        }

        using (var testImage = source.LoadImageSharpImage())
        {
            for (int i = 0; i < points.Length; i++)
            {
                int x = (int)points[i].X;
                int y = (int)points[i].Y;
                if (x < 0 || y < 0 || x + size > testImage.Width || y + size > testImage.Height)
                {
                    throw new ArgumentException("Specified ractangles must be within the bounds of the source image.");
                }
            }
        }

        _cubemapTexture = GL.GenTexture();

        GL.BindTexture(TextureTarget.TextureCubeMap, _cubemapTexture);

        for (int i = 0; i < points.Length; i++)
        {
            int x = (int)points[i].X;
            int y = (int)points[i].Y;
            using var image = source.LoadImageSharpImage();
            image.Mutate(ctx => ctx.Crop(new(x, y, size, size)));
            byte[] data = new byte[image.Width * image.Height * 4];
            image.CopyPixelDataTo(data);

            GL.TexImage2D((TextureTarget)(TextureTarget.TextureCubeMapPositiveX + i), 0, PixelInternalFormat.Rgba, size, size, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
        }

        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);

        GL.BindTexture(TextureTarget.TextureCubeMap, 0);
    }

    /// <summary>
    /// Initializes a new instance of <see cref="CubemapSkybox"/> using the specified source textures.
    /// </summary>
    /// <remarks>Texture order: (right, left, top, bottom, front, back)</remarks>
    /// <param name="sources">Texture sources in the order: (right, left, top, bottom, front, back)</param>
    public CubemapSkybox(Texture[] sources)
    {
        if (sources.Length != 6)
        {
            throw new ArgumentException("Length of the source array must be 6.");
        }

        int size = 0;
        for (int i = 0; i < sources.Length; i++)
        {
            if (size == 0)
            {
                size = sources[i].BaseWidth;
            }

            if (sources[i].BaseWidth != size || sources[i].BaseHeight != size)
            {
                throw new ArgumentException("Each texture must have the uniform size.");
            }
        }

        _cubemapTexture = GL.GenTexture();

        
        for (int i = 0; i < sources.Length; i++)
        {
            GL.CopyImageSubData(sources[i].TextureId, ImageTarget.Texture2D, 0, 0, 0, 0,
                                _cubemapTexture, ImageTarget.TextureCubeMap, 0, 0, 0, i,
                                sources[i].BaseWidth, sources[i].BaseHeight, 1);
        }

        GL.BindTexture(TextureTarget.TextureCubeMap, _cubemapTexture);

        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);

        GL.BindTexture(TextureTarget.TextureCubeMap, 0);
    }

    /// <summary>
    /// Renders the current cubemap sky box.
    /// </summary>
    //public void Render(Camera3D camera)
    //{
    //    if (s_buffer is null)
    //    {
    //        s_buffer = new();
    //        PersistentResourceService.Register(s_buffer);
    //    }

    //    Shader shader = Shader.CubemapSkybox;

    //    shader.Bind();

    //    GL.ActiveTexture(TextureUnit.Texture0);
    //    GL.BindTexture(TextureTarget.TextureCubeMap, _cubemapTexture);

    //    GL.DepthMask(false);
    //    GL.DepthFunc(DepthFunction.Lequal);

    //    GL.Enable(EnableCap.Blend);
    //    GL.Enable(EnableCap.DepthTest);
    //    GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

    //    Matrix4 view = Matrix4.Identity;
    //    Matrix3 mat3 = new Matrix3(camera.GetViewMatrix());
    //    view.Row0 = new Vector4(mat3.Row0, 0);
    //    view.Row1 = new Vector4(mat3.Row1, 0);
    //    view.Row2 = new Vector4(mat3.Row2, 0);

    //    Shader.SendMatrix012(Matrix4.Identity, view, camera.GetProjectionMatrix());

    //    s_buffer.Draw();

    //    GL.DepthMask(true);
    //    GL.DepthFunc(DepthFunction.Less);

    //    GL.BindTexture(TextureTarget.TextureCubeMap, 0);
    //}

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {

            }

            if (_cubemapTexture != 0)
            {
                GL.DeleteTexture(_cubemapTexture);
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// Releases all resources used by <see cref="CubemapSkybox"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// <see cref="CubemapSkybox"/> destructor.
    /// </summary>
    ~CubemapSkybox()
    {
        Dispose(false);
    }
}