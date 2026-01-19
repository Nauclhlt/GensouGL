using System.Numerics;

namespace GensouGL.Graphics;

/// <summary>
/// Contains data necessary to map shadows.
/// </summary>
public sealed class ShadowData : IDisposable
{
    private bool _disposed = false;
    private int _resolutionWidth;
    private int _resolutionHeight;
    private DepthFramebuffer _depthFramebuffer;

    /// <summary>
    /// Gets the X resolution of the directional shadow map.
    /// </summary>
    public int ResolutionWidth => _resolutionWidth;
    /// <summary>
    /// Gets the Y resolution of the directional shadow map.
    /// </summary>
    public int ResolutionHeight => _resolutionHeight;

    internal DepthFramebuffer DepthFramebuffer => _depthFramebuffer;

    /// <summary>
    /// Initializes a new instance of <see cref="ShadowData"/> with the specified shadow map resolution.
    /// </summary>
    /// <param name="resolutionWidth">X resolution.</param>
    /// <param name="resolutionHeight">Y resolution.</param>
    public ShadowData(int resolutionWidth, int resolutionHeight)
    {
        _resolutionWidth = resolutionWidth;
        _resolutionHeight = resolutionHeight;

        _depthFramebuffer = new(_resolutionWidth, _resolutionHeight);
    }

    /// <summary>
    /// Clears the data. This function should be called before any rendering process.
    /// </summary>
    public void Clear()
    {
        _depthFramebuffer.Clear();
    }

    public void SavePng(string filename)
    {
        _depthFramebuffer.Texture.SavePng(filename);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _depthFramebuffer.Dispose();
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// Releases all resources used by <see cref="ShadowData"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// <see cref="ShadowData"/> destructor.
    /// </summary>
    ~ShadowData()
    {
        Dispose(false);
    }
}