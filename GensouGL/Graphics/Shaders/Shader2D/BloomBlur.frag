#version 460

uniform sampler2D pTexture;
uniform vec2 pDirection;
uniform float pSpread;
uniform float pIntensity;

in vec2 vTexCoord;

out vec4 FragColor;

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

	FragColor = vec4(result + pDirection.y * (pIntensity - 1) * result, 1.0);
}