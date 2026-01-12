#version 460

uniform sampler2D pTexture;
uniform vec4 pColor;

in vec2 vTexCoord;

out vec4 FragColor;

void main()
{
	vec4 texel = texture(pTexture, vTexCoord);
	FragColor = vec4(pColor.rgb, pColor.a * (1.0 - texel.a));
}