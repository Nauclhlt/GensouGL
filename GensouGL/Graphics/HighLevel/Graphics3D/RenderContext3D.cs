namespace GensouGL.Graphics;

/// <summary>
/// Provides 3D rendering context.
/// </summary>
public sealed class RenderContext3D : IDisposable
{
    private const int ShadowResolution = 1024;

    // ----- state -----
    private LightingFrame _lightingFrame;
    private ShadowFrame _shadowFrame;
    private ShadowData _shadowData;
    private LightingMode _lightingMode;
    private BRDFMode _brdfMode;
    private Camera3D _camera;
    private GraphicsSettings3D _graphicsSettings;
    private ColorGradingSettings _colorGrading;
    private bool _disposed = false;
    // -----------------

    // ----- rendering -----
    private List<IDrawCommand3D> _commands;
    // ---------------------

    internal ShadowData ShadowData => _shadowData;
    internal ShadowFrame ShadowFrame => _shadowFrame;
    internal LightingFrame LightingFrame => _lightingFrame;


    #region Public Properties

    /// <summary>
    /// Gets or sets the lighting mode associated with the current context.
    /// </summary>
    public LightingMode LightingMode
    {
        get => _lightingMode;
        set => _lightingMode = value;
    }

    /// <summary>
    /// Gets or sets the BRDF mode associated with the current context.
    /// </summary>
    public BRDFMode BRDFMode
    {
        get => _brdfMode;
        set => _brdfMode = value;
    }

    /// <summary>
    /// Gets or sets the camera associated with the current context.
    /// </summary>
    public Camera3D Camera
    {
        get => _camera;
        set
        {
            if (value is null) return;
            _camera = value;
        }
    }

    /// <summary>
    /// Gets or sets the graphics settings associated with the current context.
    /// </summary>
    public GraphicsSettings3D GraphicsSettings
    {
        get => _graphicsSettings;
        set
        {
            if (value is null) return;
            _graphicsSettings = value;
        }
    }

    /// <summary>
    /// Gets or sets the color grading settings associated with the current context.
    /// </summary>
    public ColorGradingSettings ColorGrading
    {
        get => _colorGrading;
        set
        {
            if (value is null) return;
            _colorGrading = value;
        }
    }

    /// <summary>
    /// Determines if lighting is enabled.
    /// </summary>
    public bool LightingEnabled => _lightingMode != LightingMode.NoLighting;
    /// <summary>
    /// Determines if shadow is enabled.
    /// </summary>
    public bool ShadowEnabled => _lightingMode == LightingMode.LightingWithShadows;
    /// <summary>
    /// Determines if shadow map is available.
    /// </summary>
    public bool ShadowAvailable => _shadowData is not null;

    #endregion

    /// <summary>
    /// Initializes a new instance of <see cref="RenderContext3D"/>.
    /// </summary>
    public RenderContext3D()
    {
        _lightingFrame = LightingFrame.Default;
        _shadowFrame = ShadowFrame.Default;
        _shadowData = null;
        _camera = Camera3D.Default;
        _lightingMode = LightingMode.NoLighting;
        _brdfMode = BRDFMode.Phong;
        _graphicsSettings = new();
        _colorGrading = new();
        _commands = [];
    }

    /// <summary>
    /// Makes shadow maps.
    /// </summary>
    public void MakeShadowData()
    {
        _shadowData = new(ShadowResolution, ShadowResolution, SharedConstants.CascadeCount);
    }

    /// <summary>
    /// Sets the current lighting frame.
    /// </summary>
    /// <param name="lightingFrame">Lighting frame.</param>
    public void SetLightingFrame(LightingFrame lightingFrame)
    {
        if (lightingFrame is null)
        {
            DebugLog.Error($"{nameof(RenderContext3D)}: Lighting frame cannot be null.");
            return;
        }

        _lightingFrame = lightingFrame;
    }

    /// <summary>
    /// Sets the current shadow frame.
    /// </summary>
    /// <param name="shadowFrame">Shadow frame.</param>
    public void SetShadowFrame(ShadowFrame shadowFrame)
    {
        if (shadowFrame is null)
        {
            DebugLog.Error($"{nameof(RenderContext3D)}: Shadow frame cannot be null.");
            return;
        }

        _shadowFrame = shadowFrame;
    }

    /// <summary>
    /// Renders the shadow maps for debugging.
    /// </summary>
    public void DebugRenderShadowMaps()
    {
        if (!ShadowEnabled || !ShadowAvailable) return;

        QuickRenderer2D renderer = new(Screen.Default);
        float size = 120f;
        for (int i = 0; i < SharedConstants.CascadeCount; i++)
        {
            renderer.DrawTextureWithShader(new Rect(0f, size * i, size, size), _shadowData.DepthFramebuffers[i].Texture, Shader.DepthDebug);
        }
    }

    internal void Accumulate(IDrawCommand3D command) => _commands.Add(command);
    internal void Accumulate(IEnumerable<IDrawCommand3D> commands) => _commands.AddRange(commands);
    internal void ClearCommands() => _commands.Clear();
    internal List<IDrawCommand3D> GetCommands() => _commands;



    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _shadowData?.Dispose();
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// Releases all resources used by <see cref="RenderContext3D"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// <see cref="RenderContext3D"/> destructor.
    /// </summary>
    ~RenderContext3D()
    {
        Dispose(false);
    }
}