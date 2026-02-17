namespace GensouGL.Graphics;

/// <summary>
/// Represents an abstract base class for all renderers.
/// This class provides basic features of screen stack, and state management.
/// </summary>
public abstract class Renderer
{
    /// <summary>
    /// Screen stack.
    /// </summary>
    protected ScreenStack _screenStack;

    /// <summary>
    /// Rendering state.
    /// </summary>
    protected RenderState _state;

    internal ScreenStack ScreenStack => _screenStack;

    /// <summary>
    /// Gets or sets the rendering state.
    /// </summary>
    public RenderState State
    {
        get => _state;
        set => _state = value;
    }

    /// <summary>
    /// Gets or sets whether this renderer uses depth testing.
    /// </summary>
    protected bool DepthUsed
    {
        get; set;
    } = false;

    /// <summary>
    /// Initializes <see cref="Renderer"/> functions.
    /// </summary>
    /// <param name="screen">Target screen.</param>
    protected void Setup(Screen screen)
    {
        _screenStack = new(screen);
        if (DepthUsed)
        {
            screen.UseDepth();
        }
        _state = RenderState.Default;

        OnSetup(screen);
    }

    /// <summary>
    /// Begins post-processing.
    /// </summary>
    /// <param name="effect">Post-processing effect to apply.</param>
    /// <returns><see cref="IDisposable"/> instance to indicate the applying range of the effect.</returns>
    /// <remarks>This function only begins post-processing(prepares a new render target and push it on the screen stack), dispose of the returned object to process the contents rendered on the temporary render target and apply the effect.</remarks>
    public IDisposable BeginPostProc(PostProcessEffect effect)
    {
        Screen tempScreen = TempScreenPool.Rent((int)(_screenStack.BaseScreenWidth * effect.ActualSizeScale), (int)(_screenStack.BaseScreenHeight * effect.ActualSizeScale));
        if (DepthUsed) tempScreen.UseDepth();

        _screenStack.Push(tempScreen);
        tempScreen.Clear(Color.Transparent);

        return new PostProcessScope(this, effect);
    }

    /// <summary>
    /// Directly sets the blend mode in the rendering state.
    /// </summary>
    /// <param name="blendMode">New blend mode.</param>
    public void SetBlendMode(BlendMode blendMode)
    {
        _state.BlendMode = blendMode;
    }

    /// <summary>
    /// Set the target screen.
    /// </summary>
    /// <param name="screen">New target screen.</param>
    public void SetScreen(Screen screen)
    {
        if (_screenStack.Count >= 2)
        {
            DebugLog.Error($"{nameof(Renderer2D)}: Do not change the target screen while postprocessing.");
            return;
        }

        _screenStack.UpdateBaseScreen(screen);
    }

    /// <summary>
    /// Loads the current blend mode.
    /// </summary>
    protected void LoadBlendMode()
    {
        if (_state.BlendMode == BlendMode.NoBlend) GL.Disable(EnableCap.Blend);
        else
        {
            GL.Enable(EnableCap.Blend);
            _state.LoadBlendMode();
        }
    }

    /// <summary>
    /// When overridden, sets up the renderer.
    /// </summary>
    /// <param name="screen">Target screen.</param>
    protected abstract void OnSetup(Screen screen);

    /// <summary>
    /// When overridden, Loads the state.
    /// </summary>
    protected abstract void LoadState();

    /// <summary>
    /// When overridden, Restores the state.
    /// </summary>
    protected abstract void RestoreState();
}