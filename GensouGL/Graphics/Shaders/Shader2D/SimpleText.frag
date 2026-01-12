#version 460

in vec4 vColor;
in vec2 vTexCoord;

uniform sampler2D pTexture;

out vec4 FragColor;

void main()
{
	FragColor = vColor * texture(pTexture, vTexCoord);
	//FragColor = vec4(vColor.rgb, vColor.a * texture(pTexture, vTexCoord).a);
}