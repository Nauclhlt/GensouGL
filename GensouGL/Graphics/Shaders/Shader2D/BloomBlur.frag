#version 460

uniform sampler2D pTexture;
uniform vec2 pDirection;
uniform float pSpread;
uniform float pIntensity;

in vec2 vTexCoord;

out vec4 FragColor;

vec3 ACES(vec3 color)
{
	const float a = 2.51;const float b = 0.03;
	const float c = 2.43;const float d = 0.59;
	const float e = 0.14;
	return clamp((color * (a * color + b)) / (color * (c * color + d) + e), 0.0, 1.0);
}

void main()
{
	vec2 texelSize = 1.0 / textureSize(pTexture, 0);
	vec2 offset = texelSize * pDirection * pSpread;

	vec3 result = vec3(0.0);

	result += texture(pTexture, vTexCoord - 2.0 * offset).rgb * 0.15;
    result += texture(pTexture, vTexCoord - 1.0 * offset).rgb * 0.25;
    result += texture(pTexture, vTexCoord).rgb                * 0.20;
    result += texture(pTexture, vTexCoord + 1.0 * offset).rgb * 0.25;
    result += texture(pTexture, vTexCoord + 2.0 * offset).rgb * 0.15;

	if(pDirection.y == 1)
		FragColor = vec4(result + pDirection.y * (pIntensity - 1) * result, 1.0);
	else
		FragColor = vec4(result, 1.0);
}