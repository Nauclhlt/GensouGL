layout (std140, binding = 5) uniform ColorGrading
{
    float exposure;
    float brightness;
    float contrast;
    float saturation;

    float hueShift;
    int toneMap;
    vec2 _pad0;
} colorGrading;

vec3 ProcessColorGrading(vec3 color)
{
	color *= exp2(colorGrading.exposure);
	{
		float s = sin(colorGrading.hueShift);
		float c = cos(colorGrading.hueShift);
		mat3 hueRotation = mat3(
		vec3(0.213 + c * 0.787 - s * 0.213, 0.715 - c * 0.715 - s * 0.715, 0.072 - c * 0.072 + s * 0.928),
		vec3(0.213 - c * 0.213 + s * 0.143, 0.715 + c * 0.285 + s * 0.140, 0.072 - c * 0.072 - s * 0.283),
		vec3(0.213 - c * 0.213 - s * 0.787, 0.715 - c * 0.715 + s * 0.715, 0.072 + c * 0.928 + s * 0.072)
		);
		color = clamp(hueRotation * color, 0.0, 1.0);
	}
	color = (color - 0.5) * colorGrading.contrast + 0.5 + colorGrading.brightness;
	float luma = dot(color, vec3(0.2126, 0.7152, 0.0722));
	color = mix(vec3(luma), color, colorGrading.saturation);
	if (colorGrading.toneMap == 1)
		color = color / (color + 1.0);
	else if (colorGrading.toneMap == 2) {
		const float a = 2.51;const float b = 0.03;
		const float c = 2.43;const float d = 0.59;
		const float e = 0.14;
		color = clamp((color * (a * color + b)) / (color * (c * color + d) + e), 0.0, 1.0);
	}
	else if (colorGrading.toneMap == 3) {
		color = max(vec3(0.0), color - 0.004);
		color = (color * (6.2 * color + 0.5)) / (color * (6.2 * color + 1.7) + 0.06);
	}
	return color;
}