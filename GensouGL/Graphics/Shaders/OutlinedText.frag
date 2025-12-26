#version 460

in vec4 vColor;
in vec2 vTexCoord;
in vec4 vOutlineColor;

uniform sampler2D pTexture;

out vec4 FragColor;

void main()
{
	vec4 texel = texture(pTexture, vTexCoord);
	float p = texel.r;
	FragColor = mix(vec4(vColor.rgb, vColor.a * texel.a), 
					vec4(vOutlineColor.rgb, vOutlineColor.a * texel.a), 
					p);
}