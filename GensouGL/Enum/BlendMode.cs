namespace GensouGL.Core;

/// <summary>
/// Represents blending modes.
/// </summary>
public enum BlendMode
{
    /// <summary>
    /// Normal(Alpha) blend.
    /// </summary>
    Normal = 0,
    /// <summary>
    /// Add blend.
    /// </summary>
    Add = 1,
    /// <summary>
    /// Add blend with no alpha.
    /// </summary>
    AddNoAlpha = 2,
    /// <summary>
    /// Glow blend.
    /// </summary>
    Glow = 3,
    /// <summary>
    /// Subtract blend.
    /// </summary>
    Subtract = 4,
    /// <summary>
    /// Multiplty blend.
    /// </summary>
    Multiply = 5,
    /// <summary>
    /// Max blend.
    /// </summary>
    Max = 6,
    /// <summary>
    /// Min blend.
    /// </summary>
    Min = 7,
    /// <summary>
    /// Screen blend.
    /// </summary>
    Screen = 8,
    /// <summary>
    /// No blend.
    /// </summary>
    NoBlend = 9
}

internal static class BlendModeData
{
    public static readonly BlendEquationMode[] BlendEquation =
    {
        BlendEquationMode.FuncAdd,
        BlendEquationMode.FuncAdd,
        BlendEquationMode.FuncAdd,
        BlendEquationMode.FuncAdd,
        BlendEquationMode.FuncSubtract,
        BlendEquationMode.FuncAdd,
        BlendEquationMode.Max,
        BlendEquationMode.Min,
        BlendEquationMode.FuncAdd
    };

    public static readonly (BlendingFactor src, BlendingFactor dst)[] BlendFactors =
    {
        (BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha),
        (BlendingFactor.SrcAlpha, BlendingFactor.One),
        (BlendingFactor.One, BlendingFactor.One),
        (BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha),
        (BlendingFactor.One, BlendingFactor.One),
        (BlendingFactor.DstColor, BlendingFactor.Zero),
        (BlendingFactor.One, BlendingFactor.One),
        (BlendingFactor.One, BlendingFactor.One),
        (BlendingFactor.One, BlendingFactor.OneMinusDstColor)
    };
}