#version 460

in vec4 vColor;
in vec2 vTexCoord;

uniform sampler2D pTexture;

out vec4 FragColor;

void main()
{
	vec4 texel = texture(pTexture, vTexCoord);
	if (texel.a == 0.0)
	{
		discard;
	}
	FragColor = vec4(texel.rgb * vColor.rgb, texel.a * vColor.a);
}