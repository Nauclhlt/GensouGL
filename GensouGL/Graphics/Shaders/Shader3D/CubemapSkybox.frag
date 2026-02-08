#version 460

uniform samplerCube pCubemapTexture;

in vec3 vTexCoord;

out vec4 FragColor;

void main()
{
	FragColor = texture(pCubemapTexture, vTexCoord);
}