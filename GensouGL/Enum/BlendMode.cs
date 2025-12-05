namespace GensouGL.Core;

public enum BlendMode
{
    Normal = 0,
    Add = 1,
    AddNoAlpha = 2,
    Glow = 3,
    Subtract = 4,
    Multiply = 5,
    Max = 6,
    Min = 7,
    Screen = 8
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