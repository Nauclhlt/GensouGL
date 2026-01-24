#version 460
in vec4 vColor;
in vec3 vNormal;
in vec3 vPosition;
in vec2 vTexCoord;

uniform sampler2D pTexture;

out vec4 FragColor;

vec3 ProcessLighting();
vec3 ProcessColorGrading(vec3 color);

//ignore
uniform LightBlock
{
	vec4 ambientColorIntensity;

} lightBlock;
//ignore_end

//include<LightingBody>
//include<ColorGrading>

void main()
{
	vec4 texel = texture(pTexture, vTexCoord);
	
	if (texel.a == 0)
	{
		discard;
	}

	vec3 ambient = lightBlock.ambientColorIntensity.rgb * lightBlock.ambientColorIntensity.a;
	vec3 lit = vec3(0);

	lit += ProcessLighting();
	
	FragColor = vec4(ProcessColorGrading(texel.rgb * vColor.rgb * (ambient + lit)), texel.a * vColor.a);
}