#version 460

out vec4 FragColor;

uniform sampler2D pTexture;

in vec4 vColor;
in vec2 vTexCoord;

void main()
{
	FragColor = vColor * texture(pTexture, vTexCoord);
}