#version 460

in vec4 vColor;
in vec2 vTexCoord;

uniform sampler2D pTexture;

void main()
{
	vec4 texel = texture(pTexture, vTexCoord);
	if (texel.a < 0.5)
	{
		discard;
	}
}
