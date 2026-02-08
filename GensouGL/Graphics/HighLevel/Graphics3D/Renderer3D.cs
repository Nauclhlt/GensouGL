using OpenTK.Mathematics;

namespace GensouGL.Graphics;

// TODO:
// move the lighting and shadow mapping features to SceneRenderer3D.
// Renderer3D should be removed(if possible)

/// <summary>
/// Represents an abstract base class for all 3D renderers.
/// This class provide sbasic features of screen stack, camera, and state management.
/// </summary>
/// <remarks>Classes derived from this class should call <see cref="Renderer.Setup(Screen)"/> in their constructor, call <see cref="LoadState()"/> before rendering, and <see cref="RestoreState()"/> after rendering.</remarks>
public abstract class Renderer3D : Renderer
{
    /// <summary>
    /// Camera.
    /// </summary>
    protected Camera3D _camera;
    /// <summary>
    /// 3D graphics settings.
    /// </summary>
    protected GraphicsSettings3D _graphicsSettings;

    #region Public properties

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

    #endregion

    /// <summary>
    /// Returns the current view matrix.
    /// </summary>
    protected OpenTK.Mathematics.Matrix4 ViewMatrix => _camera.GetViewMatrix();
    /// <summary>
    /// Returns the current projection matrix.
    /// </summary>
    protected OpenTK.Mathematics.Matrix4 ProjMatrix => _camera.GetProjectionMatrix();

    /// <summary>
    /// Sets up the <see cref="Renderer2D"/>.
    /// </summary>
    /// <param name="screen">Target screen.</param>
    protected override void OnSetup(Screen screen)
    {
        _camera = Camera3D.Default;
        _graphicsSettings = new();
    }

    /// <summary>
    /// Loads the current state.
    /// </summary>
    protected override void LoadState()
    {
        Screen screen = _screenStack.Peek();
        LoadBlendMode();
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
}