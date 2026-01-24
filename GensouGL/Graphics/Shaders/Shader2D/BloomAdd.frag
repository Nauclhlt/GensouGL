#version 460

uniform sampler2D pBaseTexture;
uniform sampler2D pBlurTexture;

uniform float pIntensity;

in vec2 vTexCoord;

out vec4 FragColor;

vec3 ProcessColorGrading(vec3 color);
//include<ColorGrading>

vec3 ACES(vec3 color)
{
	const float a = 2.51;const float b = 0.03;
	const float c = 2.43;const float d = 0.59;
	const float e = 0.14;
	return clamp((color * (a * color + b)) / (color * (c * color + d) + e), 0.0, 1.0);	
}

void main()
{
	vec4 baseColor	= texture(pBaseTexture, vTexCoord);
	vec3 blur		= texture(pBlurTexture, vTexCoord).rgb * pIntensity;
	vec3 result = baseColor.rgb + blur.rgb;
	
	float alpha = dot(blur.rgb, vec3(0.299, 0.587, 0.114));
	alpha = clamp(baseColor.a + alpha, 0.0, 1.0);
	FragColor = vec4(ProcessColorGrading(result), alpha);
}