#version 460
in vec4 vColor;
in vec3 vNormal;
in vec3 vPosition;

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

void main()
{
	vec3 ambient = lightBlock.ambientColorIntensity.rgb * lightBlock.ambientColorIntensity.a;
	vec3 lit = vec3(0);

	lit += ProcessLighting();
	
    FragColor = vec4(ProcessColorGrading(vColor.rgb * (ambient + lit)), vColor.a);
}

//include<ProcessColorGrading>