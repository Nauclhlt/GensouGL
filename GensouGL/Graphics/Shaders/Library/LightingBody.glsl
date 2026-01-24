//ignore
#version 460
vec3 vNormal, vPosition;
//ignore_end

#define MAX_LIGHTS 16
#define BRDF_PHONG 0
#define BRDF_PBR 1
#define TILE_SIZE 16
#define PI 3.1415926535897932

struct PointLight
{
	vec4 positionRange;
	vec4 colorIntensity;
};

struct DirectionalLight
{
	vec4 direction;
	vec4 colorIntensity;
};

layout (std140, binding = 0) uniform LightBlock
{
	vec3 viewPosition;
	int brdf;
	
	vec4 ambientColorIntensity;

	int pointLightCount;
	int directionalLightCount;
	ivec2 tileCount;

	DirectionalLight directionalLights[MAX_LIGHTS];
} lightBlock;

layout (std140, binding = 1) uniform Material
{
	float shininess;
	float metallic;
	float roughness;
	float _pad0;
    
    vec4 albedoColor;
} material;

layout (std430, binding = 2) readonly buffer PointLights
{
    PointLight pointLights[];
};

layout (std430, binding = 3) readonly buffer LightIndices
{
    int lightIndices[];
};

layout (std430, binding = 4) readonly buffer TileRanges
{
    ivec2 tileRanges[];
};


vec3 EvaluatePhongBRDF(vec3 N, vec3 V, vec3 L, vec3 radiance);
vec3 EvaluatePBR(vec3 N, vec3 V, vec3 L, vec3 radiance);

vec3 EvaluateBRDF( vec3 N, vec3 V, vec3 L, vec3 radiance )
{
    if (lightBlock.brdf == BRDF_PHONG)
        return EvaluatePhongBRDF(N, V, L, radiance);
    else
        return EvaluatePBR(N, V, L, radiance);
}



vec3 ProcessLighting()
{
	vec3 res = vec3(0);

    vec3 N = normalize(vNormal);
    vec3 V = normalize(lightBlock.viewPosition - vPosition);

    ivec2 tileCoord = ivec2(gl_FragCoord.xy) / TILE_SIZE;
    int tileId = tileCoord.y * lightBlock.tileCount.x + tileCoord.x;

    ivec2 range = tileRanges[tileId];

    for (int i = 0; i < range.y; i++)
    {
        int idx = lightIndices[range.x + i];

        vec3 lightPos = pointLights[idx].positionRange.xyz;
        float range   = pointLights[idx].positionRange.w;

        vec3 Lvec = lightPos - vPosition;
        float dist = length(Lvec);
        vec3 L = Lvec / dist;

        float att = clamp(1.0 - dist / range, 0.0, 1.0);
        att *= att;

        vec3 radiance =
            pointLights[idx].colorIntensity.rgb *
            pointLights[idx].colorIntensity.a *
            att;

        res += EvaluateBRDF(N, V, L, radiance);
    }

    for (int i = 0; i < lightBlock.directionalLightCount; i++)
    {
        vec3 L = normalize(-lightBlock.directionalLights[i].direction.xyz);

        vec3 radiance =
            lightBlock.directionalLights[i].colorIntensity.rgb *
            lightBlock.directionalLights[i].colorIntensity.a;

        res += EvaluateBRDF(N, V, L, radiance);
    }

    return res;
}

//include<EvaluatePBR>
//include<EvaluatePhongBRDF>