using OpenTK.Mathematics;

namespace GensouGL.Graphics;

/// <summary>
/// Provides the rendering pipeline to draw rendering contexts.
/// </summary>
public sealed class SceneRenderer3D : Renderer
{
    #region Constants

    private const int LightBlockBindingPoint = 0;
    private const int MaterialBindingPoint = 1;
    private const int PointLightsBindingPoint = 2;
    private const int LightIndicesBindingPoint = 3;
    private const int TileRangesBindingPoint = 4;
    private const int ColorGradingBindingPoint = 5;

    #endregion

    private TiledLightCache _tiledLightCache;

    /// <summary>
    /// Initializes a new instance of <see cref="SceneRenderer3D"/>.
    /// </summary>
    /// <param name="screen">Screen(render target).</param>
    public SceneRenderer3D(Screen screen)
    {
        Setup(screen);
        _tiledLightCache = new(screen.Width, screen.Height);
    }

    /// <summary>
    /// Consumes and executes all draw commands accumulated in the specified rendering contexts.
    /// </summary>
    /// <param name="context">Rendering context.</param>
    public void ConsumeAndRender(RenderContext3D context)
    {
        List<IDrawCommand3D> commands = context.GetCommands();
        if (commands.Count == 0) return;

        if (context.LightingMode == LightingMode.NoLighting)
        {
            RenderNoLighting(context);
        }
        else
        {
            ShadowDepthPass(context);
            LightingPass(context);
        }

        context.ClearCommands();
    }

    #region Rendering Passes

    private void ShadowDepthPass(RenderContext3D context)
    {
        if (!context.ShadowEnabled || !context.ShadowAvailable) return;

        List<IDrawCommand3D> commands = context.GetCommands();

        LoadShadowDepthState(context);
        SetShadowDepthMVP(context);

        for (int i = 0; i < commands.Count; i++)
        {
            commands[i].MakeDrawCall();
        }

        RestoreShadowDepthState(context);
    }

    private void LightingPass(RenderContext3D context)
    {
        if (!context.LightingEnabled) return;

        List<IDrawCommand3D> commands = context.GetCommands();

        LoadState(context);
        LoadLightingUniforms(context);

        for (int i = 0; i < commands.Count; i++)
        {
            if (!commands[i].Material.CastShadows) continue;

            Shader shader = commands[i].GetShader(context.LightingMode);
            shader.Bind();
            SetDefaultMVP(context);
            if (context.ShadowEnabled && context.ShadowAvailable)
                LoadShadowRenderingState(context, shader);

            LoadMaterial(commands[i].Material);

            commands[i].MakeDrawCall();

            if (context.ShadowEnabled && context.ShadowAvailable)
                RestoreShadowRenderingState();
        }

        RestoreLightingUniforms();
        RestoreState();
    }

    #endregion

    #region Lighting

    private unsafe void LoadLightingUniforms(RenderContext3D context)
    {
        LightingFrame lightingFrame = context.LightingFrame;


        if (lightingFrame is null) return;
        if (!context.LightingEnabled) return;

        _tiledLightCache.Cache(context.Camera, lightingFrame.PointLights, _screenStack.BaseScreenWidth, _screenStack.BaseScreenHeight);

        GpuLightBlock lightBlock = default;
        lightBlock.ViewPosition = context.Camera.Position.ToOpenTK();
        lightBlock.Brdf = (int)context.BRDFMode;
        lightBlock.TileCount = new(_tiledLightCache.TileCountX, _tiledLightCache.TileCountY);
        lightBlock.AmbientColorIntensity = new(lightingFrame.Ambient.Color.R, lightingFrame.Ambient.Color.G, lightingFrame.Ambient.Color.B, lightingFrame.Ambient.Intensity);

        lightBlock.PointLightCount = lightingFrame.PointLights.Count;

        lightBlock.DirectionalLightCount = int.Min(16, lightingFrame.DirectionalLights.Count);
        for (int i = 0; i < lightBlock.DirectionalLightCount; i++)
        {
            DirectionalLight light = lightingFrame.DirectionalLights[i];
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

        SharedShaderBuffers.LightBlockUBO.Update(lightBlock);
        SharedShaderBuffers.LightBlockUBO.Bind(LightBlockBindingPoint);

        GpuPointLight[] pointLights = new GpuPointLight[lightingFrame.PointLights.Count];
        for (int i = 0; i < lightingFrame.PointLights.Count; i++)
        {
            pointLights[i] = new(lightingFrame.PointLights[i]);
        }
        SharedShaderBuffers.PointLightsSSBO.Update(pointLights);
        SharedShaderBuffers.PointLightsSSBO.Bind(PointLightsBindingPoint);

        SharedShaderBuffers.LightIndicesSSBO.Update(CollectionsMarshal.AsSpan(_tiledLightCache.Indices));
        SharedShaderBuffers.LightIndicesSSBO.Bind(LightIndicesBindingPoint);

        SharedShaderBuffers.TileRangesSSBO.Update(_tiledLightCache.Ranges);
        SharedShaderBuffers.TileRangesSSBO.Bind(TileRangesBindingPoint);

        SharedShaderBuffers.ColorGradingUBO.Update(context.ColorGrading.ToGpuColorGrading());
        SharedShaderBuffers.ColorGradingUBO.Bind(ColorGradingBindingPoint);

        SharedShaderBuffers.MaterialUBO.Bind(MaterialBindingPoint);
    }

    private void RestoreLightingUniforms()
    {
        SharedShaderBuffers.PointLightsSSBO.Unbind(PointLightsBindingPoint);
        SharedShaderBuffers.LightIndicesSSBO.Unbind(LightIndicesBindingPoint);
        SharedShaderBuffers.TileRangesSSBO.Unbind(TileRangesBindingPoint);
        SharedShaderBuffers.ColorGradingUBO.Unbind(ColorGradingBindingPoint);
        SharedShaderBuffers.MaterialUBO.Unbind(MaterialBindingPoint);
    }

    private void LoadMaterial(Material material)
    {
        SharedShaderBuffers.MaterialUBO.Update(material.ToGpuMaterial());
    }

    #endregion

    #region Shadows

    private void LoadShadowDepthState(RenderContext3D context)
    {
        if (!context.ShadowAvailable) return;

        ShadowData shadowData = context.ShadowData;

        shadowData.Clear();

        FramebufferService.PushAndBind(shadowData.DepthFramebuffer.Framebuffer);
        GL.Viewport(0, 0, shadowData.ResolutionWidth, shadowData.ResolutionHeight);
        GL.Enable(EnableCap.DepthTest);
        GL.Disable(EnableCap.Blend);

        Shader.Depth3D.Bind();
    }

    private void RestoreShadowDepthState(RenderContext3D context)
    {
        if (!context.ShadowAvailable) return;

        FramebufferService.Pop();
    }

    private void LoadShadowRenderingState(RenderContext3D context, Shader shader)
    {
        ShadowFrame shadowFrame = context.ShadowFrame;
        ShadowData shadowData = context.ShadowData;

        Matrix4 view = Matrix4.LookAt((shadowFrame.DirectionalShadow.Center - shadowFrame.DirectionalShadow.Direction * 300f).ToOpenTK(),
            shadowFrame.DirectionalShadow.Center.ToOpenTK(),
            Vector3.UnitY);
        float size = 200f;
        Matrix4 projection = Matrix4.CreateOrthographicOffCenter(-size, size, -size, size, 0.1f, 700f);

        shader.SetUniformInt("pDirectionalShadowEnabled", 1);
        shader.SetUniformInt("pDirectionalShadowMap", 1);
        shader.SetUniformMatrix4("pDirectionalLightMVP", false, view * projection);
        TextureService.PushAndBind(shadowData.DepthFramebuffer.Texture.TextureId, 1);
    }

    private void RestoreShadowRenderingState()
    {
        TextureService.Pop(1);
    }

    private void SetShadowDepthMVP(RenderContext3D context)
    {
        ShadowFrame shadowFrame = context.ShadowFrame;
        Matrix4 view = Matrix4.LookAt((shadowFrame.DirectionalShadow.Center - shadowFrame.DirectionalShadow.Direction * 300f).ToOpenTK(),
            shadowFrame.DirectionalShadow.Center.ToOpenTK(),
            Vector3.UnitY);
        float size = 200f;
        Matrix4 projection = Matrix4.CreateOrthographicOffCenter(-size, size, -size, size, 0.1f, 700f);

        Shader.SendMatrix012(Matrix4.Identity, view, projection);
    }

    #endregion



    private void RenderNoLighting(RenderContext3D context)
    {
        List<IDrawCommand3D> commands = context.GetCommands();

        LoadState(context);

        for (int i = 0; i < commands.Count; i++)
        {
            Shader shader = commands[i].GetShader(LightingMode.NoLighting);
            shader.Bind();
            SetDefaultMVP(context);

            commands[i].MakeDrawCall();
        }

        RestoreState();
    }

    private void SetDefaultMVP(RenderContext3D context)
    {
        Shader.SendMatrix012(OpenTK.Mathematics.Matrix4.Identity, context.Camera.GetViewMatrix(), context.Camera.GetProjectionMatrix());
    }


    private void LoadState(RenderContext3D context)
    {
        Screen screen = _screenStack.Peek();
        LoadBlendMode();
        FramebufferService.PushAndBind(screen.ScreenId);
        GL.Viewport(0, 0, screen.Width, screen.Height);


        // 3D specific
        GL.Enable(EnableCap.DepthTest);
        if (context.GraphicsSettings.BackfaceCulling)
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
    /// Loads the current state.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override void LoadState()
    {
    }

    /// <summary>
    /// On set up.
    /// </summary>
    /// <param name="screen"></param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override void OnSetup(Screen screen)
    {
    }
}