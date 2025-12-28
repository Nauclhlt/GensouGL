using System.Runtime;

namespace GensouGL.Core;

public sealed class ColorGradingSettings
{
    private float _exposure;
    private float _brightness;
    private float _contrast;
    private float _saturation;
    private float _hueShiftRadians;
    private ToneMap _toneMap;

    public float Exposure
    {
        get => _exposure;
        set => _exposure = float.Clamp(value, -5f, 5f);
    }

    public float Brightness
    {
        get => _brightness;
        set => _brightness = float.Clamp(value, -0.2f, 0.2f);
    }

    public float Contrast
    {
        get => _contrast;
        set => _contrast = float.Clamp(value, 0.5f, 1.5f);
    }

    public float Saturation
    {
        get => _saturation;
        set => _saturation = float.Clamp(value, 0f, 2f);
    }

    public float HueShiftRadians
    {
        get => _hueShiftRadians;
        set => _hueShiftRadians = float.Clamp(value, -MathF.PI, MathF.PI);
    }

    public ToneMap ToneMap
    {
        get => _toneMap;
        set => _toneMap = value;
    }

    public ColorGradingSettings(float exposure, float brightness, float contrast, float saturation, float hueShiftRadians, ToneMap toneMap)
    {
        _exposure = exposure;
        _brightness = brightness;
        _contrast = contrast;
        _saturation = saturation;
        _hueShiftRadians = hueShiftRadians;
        _toneMap = toneMap;
    }

    public ColorGradingSettings() : this(0f, 0f, 1f, 1f, 0f, ToneMap.ACES)
    {
    }

    public void SetUniform(Shader shader)
    {
        shader.SetUniformFloat("pExposure", Exposure);
        shader.SetUniformFloat("pBrightness", Brightness);
        shader.SetUniformFloat("pContrast", Contrast);
        shader.SetUniformFloat("pSaturation", Saturation);
        shader.SetUniformFloat("pHueShift", HueShiftRadians);
        shader.SetUniformInt("pToneMap", (int)ToneMap);
    }
}