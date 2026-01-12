using OpenTK.Mathematics;

namespace GensouGL.Graphics;

/// <summary>
/// Represents an abstract base class for all 3D renderers.
/// This class provide sbasic features of screen stack, camera, and state management.
/// </summary>
/// <remarks>Classes derived from this class should call <see cref="Renderer.Setup(Screen)"/> in their constructor, call <see cref="LoadState()"/> before rendering, and <see cref="RestoreState()"/> after rendering.</remarks>
public abstract class Renderer3D : Renderer
{
    #region Static

    private static StructureUniformBuffer<GpuLightBlock> s_lightBlockUBO;
    private static StructureUniformBuffer<GpuMaterial> s_materialUBO;
    private static ArrayShaderStorageBuffer<GpuPointLight> s_pointLightsSSBO;
    private static ArrayShaderStorageBuffer<int> s_lightIndicesSSBO;
    private static ArrayShaderStorageBuffer<int> s_tileRangesSSBO;

    internal static void Initialize()
    {
        s_lightBlockUBO = new();
        s_materialUBO = new();
        s_pointLightsSSBO = new();
        s_lightIndicesSSBO = new();
        s_tileRangesSSBO = new();
    }

    internal static void Release()
    {
        s_lightBlockUBO.Dispose();
        s_materialUBO.Dispose();
        s_pointLightsSSBO.Dispose();
        s_lightIndicesSSBO.Dispose();
        s_tileRangesSSBO.Dispose();
    }

    #endregion


    /// <summary>
    /// Camera.
    /// </summary>
    protected Camera3D _camera;
    /// <summary>
    /// 3D graphics settings.
    /// </summary>
    protected GraphicsSettings3D _graphicsSettings;
    /// <summary>
    /// Current lighting frame.
    /// </summary>
    protected LightingFrame _lightingFrame;
    /// <summary>
    /// Current material.
    /// </summary>
    protected Material _material;
    /// <summary>
    /// Current lighting mode.
    /// </summary>
    protected LightingMode _lightingMode;
    /// <summary>
    /// Current BRDF mode.
    /// </summary>
    protected BRDFMode _brdfMode;
    /// <summary>
    /// Gets if lighting is enabled.
    /// </summary>
    protected bool LightingEnabled => _lightingMode != LightingMode.NoLighting;

    private TiledLightCache _tiledLightCache;

    /// <summary>
    /// Gets or sets the current camera.
    /// </summary>
    public Camera3D Camera
    {
        get => _camera;
        set
        {
            _camera = value is not null ? value : Camera3D.Default;
        }
    }

    /// <summary>
    /// Gets or sets the current 3D graphics settings.
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
    /// Gets or sets the current material.
    /// </summary>
    public Material Material
    {
        get => _material;
        set
        {
            if (value is null) return;
            _material = value;
        }
    }

    /// <summary>
    /// Gets or sets the current lighting mode.
    /// </summary>
    public LightingMode LightingMode
    {
        get => _lightingMode;
        set => _lightingMode = value;
    }

    /// <summary>
    /// Gets or sets the current BRDF mode.
    /// </summary>
    public BRDFMode BRDFMode
    {
        get => _brdfMode;
        set => _brdfMode = value;
    }

    /// <summary>
    /// Returns the current view matrix.
    /// </summary>
    protected OpenTK.Mathematics.Matrix4 ViewMatrix => _camera.GetViewMatrix();
    /// <summary>
    /// Returns the current projection matrix.
    /// </summary>
    protected OpenTK.Mathematics.Matrix4 ProjMatrix => _camera.GetProjectionMatrix();

    /// <summary>
    /// Sets the current lighting frame.
    /// </summary>
    /// <param name="lightingFrame">Lighting frame.</param>
    public void SetLightingFrame(LightingFrame lightingFrame)
    {
        if (lightingFrame is null)
        {
            DebugLog.Error($"{nameof(Renderer3D)}: Lighting frame cannot be null.");
            return;
        }

        _lightingFrame = lightingFrame;
    }

    /// <summary>
    /// Sets the default MVP matrices to the uniform variables.
    /// </summary>
    protected void SetDefaultMVP()
    {
        // TODO: Model matrix(temporarily identity)
        Shader.SendMatrix012(Matrix4.Identity, ViewMatrix, ProjMatrix);
    }

    /// <summary>
    /// Sets up the <see cref="Renderer2D"/>.
    /// </summary>
    /// <param name="screen">Target screen.</param>
    protected override void OnSetup(Screen screen)
    {
        _camera = Camera3D.Default;
        _graphicsSettings = new();
        _lightingFrame = LightingFrame.Default;
        _material = Material.Default;
        _lightingMode = LightingMode.NoLighting;
    }

    /// <summary>
    /// Loads the current state.
    /// </summary>
    protected override void LoadState()
    {
        Screen screen = _screenStack.Peek();
        _state.LoadBlendMode();
        FramebufferService.PushAndBind(screen.ScreenId);
        GL.Viewport(0, 0, screen.Width, screen.Height);

        // 3D specific
        GL.Enable(EnableCap.DepthTest);
        if (_graphicsSettings.BackfaceCulling)
        {
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(TriangleFace.Back);
        }
        else
        {
            GL.Disable(EnableCap.CullFace);
        }
    }

    /// <summary>
    /// Restores the state.
    /// </summary>
    protected override void RestoreState()
    {
        FramebufferService.Pop();
    }

    /// <summary>
    /// Prepares for lighting.
    /// </summary>
    protected unsafe void PrepareLighting()
    {
        if (_lightingFrame is null) return;
        if (_lightingMode == LightingMode.NoLighting) return;

        _tiledLightCache = new(_camera, _lightingFrame.PointLights, _screenStack.BaseScreenWidth, _screenStack.BaseScreenHeight);

        GpuLightBlock lightBlock = default;
        lightBlock.ViewPosition = _camera.Position.ToOpenTK();
        lightBlock.Brdf = (int)_brdfMode;
        lightBlock.TileCount = new(_tiledLightCache.TileCountX, _tiledLightCache.TileCountY);
        lightBlock.AmbientColorIntensity = new(_lightingFrame.Ambient.Color.R, _lightingFrame.Ambient.Color.G, _lightingFrame.Ambient.Color.B, _lightingFrame.Ambient.Intensity);

        lightBlock.PointLightCount = _lightingFrame.PointLights.Count;

        lightBlock.DirectionalLightCount = int.Min(16, _lightingFrame.DirectionalLights.Count);
        for (int i = 0; i < lightBlock.DirectionalLightCount; i++)
        {
            DirectionalLight light = _lightingFrame.DirectionalLights[i];
            int pos = i << 3;
            lightBlock.DirectionalLights[pos] = light.Direction.X;
            lightBlock.DirectionalLights[pos + 1] = light.Direction.Y;
            lightBlock.DirectionalLights[pos + 2] = light.Direction.Z;
            lightBlock.DirectionalLights[pos + 3] = 0f;  // no data
            lightBlock.DirectionalLights[pos + 4] = light.Color.R;
            lightBlock.DirectionalLights[pos + 5] = light.Color.G;
            lightBlock.DirectionalLights[pos + 6] = light.Color.B;
            lightBlock.DirectionalLights[pos + 7] = light.Intensity;
        }

        s_lightBlockUBO.Update(lightBlock);
        s_lightBlockUBO.Bind(0);

        s_materialUBO.Update(_material.ToGpuMaterial());
        s_materialUBO.Bind(1);

        GpuPointLight[] pointLights = new GpuPointLight[_lightingFrame.PointLights.Count];
        for (int i = 0; i < _lightingFrame.PointLights.Count; i++)
        {
            pointLights[i] = new(_lightingFrame.PointLights[i]);
        }
        s_pointLightsSSBO.Update(pointLights);
        s_pointLightsSSBO.Bind(2);

        s_lightIndicesSSBO.Update(CollectionsMarshal.AsSpan(_tiledLightCache.Indices));
        s_lightIndicesSSBO.Bind(3);

        s_tileRangesSSBO.Update(_tiledLightCache.Ranges);
        s_tileRangesSSBO.Bind(4);
    }

    /// <summary>
    /// Finishes lighting.
    /// </summary>
    protected void FinishLighting()
    {
        if (_lightingMode == LightingMode.NoLighting) return;

        s_lightBlockUBO.Unbind(0);
        s_materialUBO.Unbind(1);
        s_pointLightsSSBO.Unbind(2);
        s_lightIndicesSSBO.Unbind(3);
        s_tileRangesSSBO.Unbind(4);
    }
}