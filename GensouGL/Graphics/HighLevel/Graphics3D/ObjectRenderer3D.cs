namespace GensouGL.Graphics;

using OpenTK.Mathematics;

/// <summary>
/// Provides the rendering pipeline to draw special 3D objects.
/// </summary>
public sealed class ObjectRenderer3D : Renderer3D
{
    /// <summary>
    /// Initializes a new instance of <see cref="ObjectRenderer3D"/>.
    /// </summary>
    /// <param name="screen">Screen(render target).</param>
    public ObjectRenderer3D(Screen screen)
    {
        DepthUsed = true;
        Setup(screen);
    }

    /// <summary>
    /// Draws the specified skybox.
    /// </summary>
    /// <param name="skybox">Cubemap skybox to render.</param>
    public void DrawSkybox(CubemapSkybox skybox)
    {
        LoadState();
        
        Shader shader = Shader.CubemapSkybox;

        shader.Bind();

        TextureService.Push();
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.TextureCubeMap, skybox.CubemapTextureId);

        GL.DepthMask(false);
        GL.DepthFunc(DepthFunction.Lequal);

        GL.Enable(EnableCap.Blend);
        GL.Enable(EnableCap.DepthTest);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        Matrix4 view = Matrix4.Identity;
        Matrix3 mat3 = new Matrix3(_camera.GetViewMatrix());
        view.Row0 = new Vector4(mat3.Row0, 0);
        view.Row1 = new Vector4(mat3.Row1, 0);
        view.Row2 = new Vector4(mat3.Row2, 0);

        Shader.SendMatrix012(Matrix4.Identity, view, _camera.GetProjectionMatrix());

        SharedBuffers.CubemapSkyboxBuffer.Draw();

        GL.DepthMask(true);
        GL.DepthFunc(DepthFunction.Less);

        GL.BindTexture(TextureTarget.TextureCubeMap, 0);
        TextureService.Pop();

        RestoreState();
    }
}