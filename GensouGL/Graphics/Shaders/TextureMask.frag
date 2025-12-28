#version 460

uniform sampler2D pTexture;
uniform sampler2D pMask;

in vec2 vTexCoord;

out vec4 FragColor;

void main()
{
	vec4 texel = texture(pTexture, vTexCoord);
	vec4 mask = texture(pMask, vTexCoord);
	FragColor = vec4(texel.rgb, texel.a * mask.a);
}