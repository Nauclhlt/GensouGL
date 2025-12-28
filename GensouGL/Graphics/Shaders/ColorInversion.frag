#version 460

uniform sampler2D pTexture;
in vec2 vTexCoord;

out vec4 FragColor;

void main()
{
	vec4 texel = texture(pTexture, vTexCoord);
	FragColor = vec4(vec3(1.0) - texel.rgb, texel.a);
}