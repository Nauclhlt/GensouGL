#version 460
in vec4 vColor;
in vec3 vNormal;
in vec3 vPosition;
in vec2 vTexCoord;
in vec4 vViewPos;

out vec4 FragColor;

uniform sampler2D pTexture;

#define CASCADE_COUNT 4

uniform mat4 pCascadeLightVP[CASCADE_COUNT];
uniform float pCascadeSplits[CASCADE_COUNT];
uniform sampler2D pShadowMaps[CASCADE_COUNT];
uniform vec3 pShadowLightDirection;

vec3 ProcessLighting();
vec3 ProcessColorGrading(vec3 color);
float ShadowPCF(sampler2D shadowMap, vec2 uv, float currentDepth, float bias, int size);
float ComputeBias(vec3 normal, vec3 lightDir);
float CalcShadow(vec4 viewPos);

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

    float shadow = CalcShadow(vViewPos);
	vec4 texel = texture(pTexture, vTexCoord);
    FragColor = vec4(ProcessColorGrading(texel.rgb * vColor.rgb * (ambient + shadow * lit)), texel.a * vColor.a);
}

//include<ProcessColorGrading>
//include<Shadow>