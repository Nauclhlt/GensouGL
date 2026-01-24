#version 460

uniform sampler2D pTexture;
uniform vec2 pDirection;
uniform float pSpread;

in vec2 vTexCoord;

out vec4 FragColor;

void main()
{
	vec2 texelSize = 1.0 / textureSize(pTexture, 0);
	vec2 offset = texelSize * pDirection * pSpread;

	vec3 result = vec3(0.0);

	result += texture(pTexture, vTexCoord - 4.0 * offset).rgb * 0.016216;
    result += texture(pTexture, vTexCoord - 3.0 * offset).rgb * 0.054054;
    result += texture(pTexture, vTexCoord - 2.0 * offset).rgb * 0.1216216;
    result += texture(pTexture, vTexCoord - 1.0 * offset).rgb * 0.1945946;
    result += texture(pTexture, vTexCoord).rgb                * 0.227027;
    result += texture(pTexture, vTexCoord + 1.0 * offset).rgb * 0.1945946;
    result += texture(pTexture, vTexCoord + 2.0 * offset).rgb * 0.1216216;
    result += texture(pTexture, vTexCoord + 3.0 * offset).rgb * 0.054054;
    result += texture(pTexture, vTexCoord + 4.0 * offset).rgb * 0.016216;

	FragColor = vec4(result, 1.0);
}