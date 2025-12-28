#version 460

uniform sampler2D pTexture;

uniform float pExposure;
uniform float pBrightness;
uniform float pContrast;
uniform float pSaturation;
uniform float pHueShift;
uniform int pToneMap;

in vec2 vTexCoord;

out vec4 FragColor;

vec3 hueShift(vec3 color, float shift);
vec3 linearToSRGB(vec3 color);

void main()
{
	vec4 texel = texture(pTexture, vTexCoord);
	vec3 color = texel.rgb;

	// Exposure
	color *= exp2(pExposure);

	// Hue Shift
	color = hueShift(color, pHueShift);

	// Brightness & Contrast
	color = (color - 0.5) * pContrast + 0.5 + pBrightness;

	// Saturation
	float luma = dot(color, vec3(0.2126, 0.7152, 0.0722));
	color = mix(vec3(luma), color, pSaturation);

	// Tone Mapping
	if (pToneMap == 1)
	{
		// Reinhard
		color = color / (color + 1.0);
	}
	else if (pToneMap == 2)
	{
		// ACES
		const float a = 2.51;
		const float b = 0.03;
		const float c = 2.43;
		const float d = 0.59;
		const float e = 0.14;
		color = clamp((color * (a * color + b)) / (color * (c * color + d) + e), 0.0, 1.0);
	}
	else if (pToneMap == 3)
	{
		// Filmic Uncharted 2
		color = max(vec3(0.0), color - 0.004);
		color = (color * (6.2 * color + 0.5)) / (color * (6.2 * color + 1.7) + 0.06);
	}

	// Linear to SRGB
	//color = linearToSRGB(color);

	FragColor = vec4(color, texel.a);
}

vec3 linearToSRGB(vec3 color)
{
	return mix(12.92 * color, 1.055 * pow(color, vec3(1.0 / 2.4)) - 0.055, step(0.0031308, color));
}

vec3 hueShift(vec3 color, float shift)
{
	float s = sin(shift);
	float c = cos(shift);
	mat3 hueRotation = mat3(
	vec3(0.213 + c * 0.787 - s * 0.213, 0.715 - c * 0.715 - s * 0.715, 0.072 - c * 0.072 + s * 0.928),
	vec3(0.213 - c * 0.213 + s * 0.143, 0.715 + c * 0.285 + s * 0.140, 0.072 - c * 0.072 - s * 0.283),
	vec3(0.213 - c * 0.213 - s * 0.787, 0.715 - c * 0.715 + s * 0.715, 0.072 + c * 0.928 + s * 0.072)
	);
	return clamp(hueRotation * color, 0.0, 1.0);
}