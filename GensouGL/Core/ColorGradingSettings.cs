using System.Runtime;

namespace GensouGL.Core;

/// <summary>
/// Represents settings related to color grading.
/// </summary>
public sealed class ColorGradingSettings
{
    private float _exposure;
    private float _brightness;
    private float _contrast;
    private float _saturation;
    private float _hueShiftRadians;
    private ToneMap _toneMap;

    /// <summary>
    /// Gets or sets the exposure value. Value is clamped to the range [-5, 5]
    /// </summary>
    public float Exposure
    {
        get => _exposure;
        set => _exposure = float.Clamp(value, -5f, 5f);
    }

    /// <summary>
    /// Gets or sets the brightness value. Value is clamped to the range [-0.2, 0.2]
    /// </summary>
    public float Brightness
    {
        get => _brightness;
        set => _brightness = float.Clamp(value, -0.2f, 0.2f);
    }

    /// <summary>
    /// Gets or sets the contrast value. Value is clamped to the range [0.5, 1.5]
    /// </summary>
    public float Contrast
    {
        get => _contrast;
        set => _contrast = float.Clamp(value, 0.5f, 1.5f);
    }

    /// <summary>
    /// Gets or sets the saturation value. Value is clamped to the range [0, 2]
    /// </summary>
    public float Saturation
    {
        get => _saturation;
        set => _saturation = float.Clamp(value, 0f, 2f);
    }

    /// <summary>
    /// Gets or sets the hue shift value in radians. Value is clamped to the range [-3.14..., 3.14...]
    /// </summary>
    public float HueShiftRadians
    {
        get => _hueShiftRadians;
        set => _hueShiftRadians = float.Clamp(value, -MathF.PI, MathF.PI);
    }

    /// <summary>
    /// Gets or sets the tone map.
    /// </summary>
    public ToneMap ToneMap
    {
        get => _toneMap;
        set => _toneMap = value;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="ColorGradingSettings"/> using the specified parameters.
    /// </summary>
    /// <param name="exposure">Exposure value.</param>
    /// <param name="brightness">Brightness value.</param>
    /// <param name="contrast">Contrast value.</param>
    /// <param name="saturation">Saturation value.</param>
    /// <param name="hueShiftRadians">Hue shift value in radians.</param>
    /// <param name="toneMap">Tone map.</param>
    public ColorGradingSettings(float exposure, float brightness, float contrast, float saturation, float hueShiftRadians, ToneMap toneMap)
    {
        _exposure = exposure;
        _brightness = brightness;
        _contrast = contrast;
        _saturation = saturation;
        _hueShiftRadians = hueShiftRadians;
        _toneMap = toneMap;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="ColorGradingSettings"/> using default values.
    /// </summary>
    public ColorGradingSettings() : this(0f, 0f, 1f, 1f, 0f, ToneMap.ACES)
    {
    }

    internal void SetUniform(Shader shader)
    {
        shader.SetUniformFloat("pExposure", Exposure);
        shader.SetUniformFloat("pBrightness", Brightness);
        shader.SetUniformFloat("pContrast", Contrast);
        shader.SetUniformFloat("pSaturation", Saturation);
        shader.SetUniformFloat("pHueShift", HueShiftRadians);
        shader.SetUniformInt("pToneMap", (int)ToneMap);
    }
}