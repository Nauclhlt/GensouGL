namespace GensouGL.Graphics;

/// <summary>
/// Represents an abstract base class for all 2D renderers.
/// This class provides basic features of screen stack, camera, and state management.
/// </summary>
/// <remarks>Classes derived from this class should call <see cref="Renderer.Setup(Screen)"/> in their constructor, call <see cref="LoadState()"/> before rendering, and <see cref="RestoreState()"/> after rendering.</remarks>
public abstract class Renderer2D : Renderer
{
    /// <summary>
    /// Camera.
    /// </summary>
    protected Camera2D _camera;

    /// <summary>
    /// Gets or sets the current camera.
    /// </summary>
    public Camera2D Camera
    {
        get => _camera;
        set
        {
            _camera = value is not null ? value : Camera2D.Default;
        }
    }

    /// <summary>
    /// Returns the default vertex modification matrix.
    /// </summary>
    /// <returns></returns>
    protected OpenTK.Mathematics.Matrix4 GetDefaultVMatrix()
    {
        OpenTK.Mathematics.Matrix4 projMatrix = OpenTK.Mathematics.Matrix4.CreateOrthographicOffCenter(0, _screenStack.BaseScreenWidth, _screenStack.BaseScreenHeight, 0, -1, 1);
        return projMatrix * _camera.GetModelMatrix();
    }

    /// <summary>
    /// Sets up the <see cref="Renderer2D"/>.
    /// </summary>
    /// <param name="screen">Target screen.</param>
    protected override void OnSetup(Screen screen)
    {
        _camera = Camera2D.Default;
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

        // 2D specific
        GL.Disable(EnableCap.DepthTest);
        GL.Disable(EnableCap.CullFace);
    }

    /// <summary>
    /// Restores the state.
    /// </summary>
    protected override void RestoreState()
    {
        FramebufferService.Pop();
    }
}