#version 460
in vec4 vColor;
in vec3 vNormal;
in vec3 vPosition;

out vec4 FragColor;

uniform mat4 pDirectionalLightMVP;
uniform sampler2D pDirectionalShadowMap;
uniform int pDirectionalShadowEnabled;

vec3 ProcessLighting();
vec3 ProcessColorGrading(vec3 color);

//ignore
uniform LightBlock
{
	vec4 ambientColorIntensity;

} lightBlock;
//ignore_end

//include<LightingBody>

float ShadowPCF(vec2 uv, float currentDepth)
{
    float shadow = 0.0;
    float bias = 0.004;
    vec2 texelSize = 1.0 / textureSize(pDirectionalShadowMap, 0);

    for (int x = -1; x <= 1; ++x)
    for (int y = -1; y <= 1; ++y)
    {
        float d = texture(pDirectionalShadowMap, uv + vec2(x, y) * texelSize).r;
        shadow += currentDepth - bias > d ? 0.0 : 1.0;
    }

    return shadow / 9.0;
}

void main()
{
	vec3 ambient = lightBlock.ambientColorIntensity.rgb * lightBlock.ambientColorIntensity.a;
	vec3 lit = vec3(0);

	lit += ProcessLighting();
	
	vec4 lightClip = pDirectionalLightMVP * vec4(vPosition, 1.0);
	vec3 ndc = lightClip.xyz / lightClip.w;
	vec2 uv = ndc.xy * 0.5 + 0.5;
	float currentDepth = ndc.z * 0.5 + 0.5;

	float shadow = ShadowPCF(uv, currentDepth);
	
    FragColor = vec4(ProcessColorGrading(vColor.rgb * (ambient + shadow * lit)), vColor.a);
}

//include<ProcessColorGrading>