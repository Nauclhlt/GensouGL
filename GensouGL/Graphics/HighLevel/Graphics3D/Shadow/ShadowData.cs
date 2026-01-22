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
    private int _cascadeCount;
    private DepthFramebuffer[] _depthFramebuffers;

    /// <summary>
    /// Gets the X resolution of the directional shadow map.
    /// </summary>
    public int ResolutionWidth => _resolutionWidth;
    /// <summary>
    /// Gets the Y resolution of the directional shadow map.
    /// </summary>
    public int ResolutionHeight => _resolutionHeight;

    internal DepthFramebuffer[] DepthFramebuffers => _depthFramebuffers;

    /// <summary>
    /// Initializes a new instance of <see cref="ShadowData"/> with the specified shadow map resolution.
    /// </summary>
    /// <param name="resolutionWidth">X resolution.</param>
    /// <param name="resolutionHeight">Y resolution.</param>
    /// <param name="cascadeCount">Number of cascades.</param>
    public ShadowData(int resolutionWidth, int resolutionHeight, int cascadeCount)
    {
        _resolutionWidth = resolutionWidth;
        _resolutionHeight = resolutionHeight;
        _cascadeCount = cascadeCount;

        _depthFramebuffers = new DepthFramebuffer[_cascadeCount];
        for (int i = 0; i < _cascadeCount; i++)
            _depthFramebuffers[i] = new(_resolutionWidth, _resolutionHeight);
    }

    /// <summary>
    /// Clears the data. This function should be called before any rendering process.
    /// </summary>
    public void Clear()
    {
        for (int i = 0; i < _cascadeCount; i++)
            _depthFramebuffers[i].Clear();
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                for (int i = 0; i < _cascadeCount; i++)
                    _depthFramebuffers[i].Dispose();
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