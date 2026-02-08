using System.Reflection.Metadata.Ecma335;
using OpenTK.Mathematics;

namespace GensouGL.Graphics;

/// <summary>
/// Provides the rendering pipeline to draw rendering contexts.
/// </summary>
public sealed class SceneRenderer3D : Renderer3D
{
    #region Constants

    private const int ShadowMapTextureUnit = 2;

    #endregion

    #region Cached Frame shadow data
    private float[] c_cascadeSplits;
    private Matrix4[] c_cascadeProjection;
    private Matrix4 c_lightViewMatrix;
    #endregion

    private TiledLightCache _tiledLightCache;

    /// <summary>
    /// Initializes a new instance of <see cref="SceneRenderer3D"/>.
    /// </summary>
    /// <param name="screen">Screen(render target).</param>
    public SceneRenderer3D(Screen screen)
    {
        DepthUsed = true;
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

        ShadowFrame shadowFrame = context.ShadowFrame;
        float[] cascadeSplits = CreateCascadeSplits(context);
        Vec3[] frustumCorners = _camera.GetFrustumCorners();
        Vec3 frustumCenter = Vec3.Zero;
        for (int i = 0; i < frustumCorners.Length; i++) frustumCenter += frustumCorners[i] * (1.0f / frustumCorners.Length);
        Matrix4 lightViewMatrix = Matrix4.LookAt((frustumCenter - shadowFrame.DirectionalShadow.Direction * shadowFrame.DirectionalShadow.Distance).ToOpenTK(), frustumCenter.ToOpenTK(), Vector3.UnitY);
        Matrix4[] lightProjectionMatrices = CreateCascadeProjectionMatrices(context, lightViewMatrix, cascadeSplits, frustumCorners);



        c_cascadeSplits = cascadeSplits;
        c_cascadeProjection = lightProjectionMatrices;
        c_lightViewMatrix = lightViewMatrix;

        ShadowData shadowData = context.ShadowData;

        LoadShadowDepthState(context);

        FramebufferService.Push();

        for (int c = 0; c < SharedConstants.CascadeCount; c++)
        {
            FramebufferService.Bind(shadowData.DepthFramebuffers[c].Framebuffer);
            
            for (int i = 0; i < commands.Count; i++)
            {
                if (!commands[i].Material.CastShadows) continue;

                commands[i].GetDepthShader().Bind();
                Shader.SendMatrix012(commands[i].Transform.GetModelMatrix(), lightViewMatrix, lightProjectionMatrices[c]);

                commands[i].Prepare();
                commands[i].MakeDrawCall();
                commands[i].Finish();
            }
        }

        FramebufferService.Pop();
    }

    private void LightingPass(RenderContext3D context)
    {
        if (!context.LightingEnabled) return;

        List<IDrawCommand3D> opaque = ListPool<IDrawCommand3D>.Rent();
        List<IDrawCommand3D> transparent = ListPool<IDrawCommand3D>.Rent();

        PrioritizeCommands(context, opaque, transparent);

        LightingPassCommands(context, opaque);
        LightingPassCommands(context, transparent);

        ListPool<IDrawCommand3D>.Return(opaque);
        ListPool<IDrawCommand3D>.Return(transparent);
    }

    private void LightingPassCommands(RenderContext3D context, List<IDrawCommand3D> commands)
    {
        LoadState();
        LoadLightingUniforms(context);

        for (int i = 0; i < commands.Count; i++)
        {
            bool useShadow = context.ShadowEnabled && context.ShadowAvailable && commands[i].Material.ReceiveShadows;
            Shader shader = commands[i].GetShader(useShadow ? LightingMode.LightingWithShadows : LightingMode.Lighting);
            shader.Bind();
            SetDefaultMVP(context, commands[i]);
            if (useShadow)
                LoadShadowRenderingState(context, shader);

            LoadMaterial(commands[i].Material);

            commands[i].Prepare();

            commands[i].MakeDrawCall();

            commands[i].Finish();

            if (useShadow)
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

        _tiledLightCache.Cache(_camera, lightingFrame.PointLights, _screenStack.BaseScreenWidth, _screenStack.BaseScreenHeight);

        GpuLightBlock lightBlock = default;
        lightBlock.ViewPosition = _camera.Position.ToOpenTK();
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
        SharedShaderBuffers.LightBlockUBO.Bind(SharedConstants.LightBlockBindingPoint);

        GpuPointLight[] pointLights = new GpuPointLight[lightingFrame.PointLights.Count];
        for (int i = 0; i < lightingFrame.PointLights.Count; i++)
        {
            pointLights[i] = new(lightingFrame.PointLights[i]);
        }
        SharedShaderBuffers.PointLightsSSBO.Update(pointLights);
        SharedShaderBuffers.PointLightsSSBO.Bind(SharedConstants.PointLightsBindingPoint);

        SharedShaderBuffers.LightIndicesSSBO.Update(CollectionsMarshal.AsSpan(_tiledLightCache.Indices));
        SharedShaderBuffers.LightIndicesSSBO.Bind(SharedConstants.LightIndicesBindingPoint);

        SharedShaderBuffers.TileRangesSSBO.Update(_tiledLightCache.Ranges);
        SharedShaderBuffers.TileRangesSSBO.Bind(SharedConstants.TileRangesBindingPoint);

        SharedShaderBuffers.ColorGradingUBO.Update(context.ColorGrading.ToGpuColorGrading());
        SharedShaderBuffers.ColorGradingUBO.Bind(SharedConstants.ColorGradingBindingPoint);

        SharedShaderBuffers.MaterialUBO.Bind(SharedConstants.MaterialBindingPoint);
    }

    private void RestoreLightingUniforms()
    {
        SharedShaderBuffers.PointLightsSSBO.Unbind(SharedConstants.PointLightsBindingPoint);
        SharedShaderBuffers.LightIndicesSSBO.Unbind(SharedConstants.LightIndicesBindingPoint);
        SharedShaderBuffers.TileRangesSSBO.Unbind(SharedConstants.TileRangesBindingPoint);
        SharedShaderBuffers.ColorGradingUBO.Unbind(SharedConstants.ColorGradingBindingPoint);
        SharedShaderBuffers.MaterialUBO.Unbind(SharedConstants.MaterialBindingPoint);
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
        
        GL.Viewport(0, 0, shadowData.ResolutionWidth, shadowData.ResolutionHeight);
        GL.Enable(EnableCap.DepthTest);
        GL.Disable(EnableCap.Blend);

        if (context.ShadowFrame.FrontFaceCulling)
        {
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(TriangleFace.Front);
        }
    }

    private void LoadShadowRenderingState(RenderContext3D context, Shader shader)
    {
        ShadowData shadowData = context.ShadowData;

        Vec3 dir = context.ShadowFrame.DirectionalShadow.Direction;
        shader.SetUniformVector3("pShadowLightDirection", dir.X, dir.Y, dir.Z);
        for (int i = 0; i < SharedConstants.CascadeCount; i++)
        {
            TextureService.PushAndBind(shadowData.DepthFramebuffers[i].Texture.TextureId, ShadowMapTextureUnit + i);
            shader.SetUniformMatrix4($"pCascadeLightVP[{i}]", false, c_lightViewMatrix * c_cascadeProjection[i]);
            shader.SetUniformFloat($"pCascadeSplits[{i}]", c_cascadeSplits[i]);
            shader.SetUniformInt($"pShadowMaps[{i}]", ShadowMapTextureUnit + i);
        }
    }

    private void RestoreShadowRenderingState()
    {
        for (int i = 0; i < SharedConstants.CascadeCount; i++)
        {
            TextureService.Pop(ShadowMapTextureUnit + i);
        }
    }

    private float[] CreateCascadeSplits(RenderContext3D context)
    {
        float lambda = 0.5f;

        float[] splits = new float[SharedConstants.CascadeCount];
        splits[0] = _camera.ZNear;

        for (int i = 1; i < SharedConstants.CascadeCount; i++)
        {
            float linearSplit = _camera.ZNear + (_camera.ZFar - _camera.ZNear) * (i / (float)SharedConstants.CascadeCount);
            float logSplit = _camera.ZNear * MathF.Pow(_camera.ZFar / _camera.ZNear, i / (float)SharedConstants.CascadeCount);
            splits[i] = GensouMathF.Lerp(linearSplit, logSplit, lambda);
        }

        return splits;
    }

    private Matrix4[] CreateCascadeProjectionMatrices(RenderContext3D context, Matrix4 lightViewMatrix, float[] cascadeSplits, Vec3[] frustumCorners)
    {
        int cascadeCount = SharedConstants.CascadeCount;
        Matrix4[] cascadeProjections = new Matrix4[cascadeCount];

        float cameraNear = _camera.ZNear;
        float cameraFar = _camera.ZFar;

        for (int i = 0; i < cascadeCount; i++)
        {
            float splitNear = cascadeSplits[i];
            float splitFar = (i == cascadeCount - 1)
                ? cameraFar
                : cascadeSplits[i + 1];

            float nearRatio = (splitNear - cameraNear) / (cameraFar - cameraNear);
            float farRatio = (splitFar - cameraNear) / (cameraFar - cameraNear);

            Vec3[] cascadeCorners = new Vec3[8];

            for (int c = 0; c < 4; c++)
            {
                Vec3 nearCorner = frustumCorners[c];
                Vec3 farCorner = frustumCorners[c + 4];

                Vec3 cornerRay = farCorner - nearCorner;

                cascadeCorners[c] = nearCorner + cornerRay * nearRatio;
                cascadeCorners[c + 4] = nearCorner + cornerRay * farRatio;
            }

            Vector3 min = new Vector3(float.MaxValue);
            Vector3 max = new Vector3(float.MinValue);

            for (int c = 0; c < 8; c++)
            {
                Vector4 ls = new Vector4(
                    cascadeCorners[c].X,
                    cascadeCorners[c].Y,
                    cascadeCorners[c].Z,
                    1.0f
                ) * lightViewMatrix;

                Vector3 p = ls.Xyz;
                min = Vector3.ComponentMin(min, p);
                max = Vector3.ComponentMax(max, p);
            }



            // Texel Snapping
            {
                float width = max.X - min.X;
                float height = max.Y - min.Y;

                float texelSizeX = width / SharedConstants.ShadowResolution;
                float texelSizeY = height / SharedConstants.ShadowResolution;

                float centerX = (min.X + max.X) * 0.5f;
                float centerY = (min.Y + max.Y) * 0.5f;

                centerX = MathF.Floor(centerX / texelSizeX) * texelSizeX;
                centerY = MathF.Floor(centerY / texelSizeY) * texelSizeY;

                min.X = centerX - width * 0.5f;
                max.X = centerX + width * 0.5f;
                min.Y = centerY - height * 0.5f;
                max.Y = centerY + height * 0.5f;
            }



            const float zPadding = 10f;

            float zNear = -max.Z - context.ShadowFrame.DirectionalShadow.CasterExtension;
            float zFar = -min.Z + zPadding;
            

            cascadeProjections[i] =
                Matrix4.CreateOrthographicOffCenter(
                    min.X, max.X,
                    min.Y, max.Y,
                    zNear, zFar
                );
        }

        return cascadeProjections;
    }

    #endregion

    private void PrioritizeCommands(RenderContext3D context, List<IDrawCommand3D> opaqueBuffer, List<IDrawCommand3D> transparentBuffer)
    {
        opaqueBuffer.Clear();
        transparentBuffer.Clear();

        List<IDrawCommand3D> commands = context.GetCommands();

        for (int i = 0; i < commands.Count; i++)
        {
            if (commands[i].Key.Priority == RenderPriority.Opaque)
            {
                opaqueBuffer.Add(commands[i]);
            }
            else if (commands[i].Key.Priority == RenderPriority.Transparent)
            {
                transparentBuffer.Add(commands[i]);
            }
        }

        if (transparentBuffer.Count == 0) return;

        // Sort transparent
        float calcViewSpaceDepth(IDrawCommand3D c)
        {
            Vector3 rp = c.Key.RepresentativePoint.ToOpenTK();
            Vector4 world = new Vector4(rp, 1f) * c.Transform.GetModelMatrix();
            Vector4 view = world * _camera.GetViewMatrix();
            return -view.Z;
        }

        transparentBuffer.Sort((x, y) => calcViewSpaceDepth(y).CompareTo(calcViewSpaceDepth(x)));
    }

    private void RenderNoLighting(RenderContext3D context)
    {
        List<IDrawCommand3D> opaque = ListPool<IDrawCommand3D>.Rent();
        List<IDrawCommand3D> transparent = ListPool<IDrawCommand3D>.Rent();

        PrioritizeCommands(context, opaque, transparent);

        NoLightingPassCommands(context, opaque);
        NoLightingPassCommands(context, transparent);

        ListPool<IDrawCommand3D>.Return(opaque);
        ListPool<IDrawCommand3D>.Return(transparent);
    }

    private void NoLightingPassCommands(RenderContext3D context, List<IDrawCommand3D> commands)
    {
        LoadState();

        for (int i = 0; i < commands.Count; i++)
        {
            Shader shader = commands[i].GetShader(LightingMode.NoLighting);
            shader.Bind();
            SetDefaultMVP(context, commands[i]);

            commands[i].Prepare();

            commands[i].MakeDrawCall();

            commands[i].Finish();
        }

        RestoreState();
    }

    private void SetDefaultMVP(RenderContext3D context, IDrawCommand3D command)
    {
        Shader.SendMatrix012(command.Transform.GetModelMatrix(), _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
    }
}