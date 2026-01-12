#version 460
#define MAX_LIGHTS 16
#define BRDF_PHONG 0
#define BRDF_PBR 1
#define TILE_SIZE 16
#define PI 3.1415926535897932

in vec4 vColor;
in vec3 vNormal;
in vec3 vPosition;

out vec4 FragColor;

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


vec3 EvaluatePhongBRDF( vec3 N, vec3 V, vec3 L, vec3 radiance )
{
    float NdotL = max(dot(N, L), 0.0);

    vec3 diffuse = radiance * NdotL;

    vec3 H = normalize(L + V);
    float spec = pow(max(dot(N, H), 0.0), material.shininess);

    vec3 specular = radiance * spec;

    return diffuse + specular;
}

vec3 FresnelSchlick(float cosTheta, vec3 F0)
{
    return F0 + (1.0 - F0) * pow(1.0 - cosTheta, 5.0);
}

float DistributionGGX(vec3 N, vec3 H, float roughness)
{
    float a  = roughness * roughness;
    float a2 = a * a;
    float NdotH = max(dot(N, H), 0.0);
    float NdotH2 = NdotH * NdotH;

    float denom = (NdotH2 * (a2 - 1.0) + 1.0);
    return a2 / (PI * denom * denom);
}

float GeometrySchlickGGX(float NdotV, float roughness)
{
    float r = roughness + 1.0;
    float k = (r * r) / 8.0;

    return NdotV / (NdotV * (1.0 - k) + k);
}

float GeometrySmith(vec3 N, vec3 V, vec3 L, float roughness)
{
    float NdotV = max(dot(N, V), 0.0);
    float NdotL = max(dot(N, L), 0.0);
    float ggx1 = GeometrySchlickGGX(NdotV, roughness);
    float ggx2 = GeometrySchlickGGX(NdotL, roughness);
    return ggx1 * ggx2;
}



vec3 EvaluatePBR(vec3 N, vec3 V, vec3 L, vec3 radiance)
{
    vec3 H = normalize(V + L);

    float NdotL = max(dot(N, L), 0.0);
    float NdotV = max(dot(N, V), 0.0);

    // base reflectivity
    vec3 F0 = vec3(0.04);
    F0 = mix(F0, material.albedoColor.rgb, material.metallic);

    // BRDF terms
    float D = DistributionGGX(N, H, material.roughness);
    float G = GeometrySmith(N, V, L, material.roughness);
    vec3  F = FresnelSchlick(max(dot(H, V), 0.0), F0);

    vec3 numerator = D * G * F;
    float denom = max(4.0 * NdotV * NdotL, 0.001);
    vec3 specular = numerator / denom;

    vec3 kS = F;
    vec3 kD = vec3(1.0) - kS;
    kD *= 1.0 - material.metallic;

    vec3 diffuse = kD * material.albedoColor.rgb / PI;

    return (diffuse + specular) * radiance * NdotL;
}


vec3 EvaluateBRDF( vec3 N, vec3 V, vec3 L, vec3 radiance )
{
    if (lightBlock.brdf == BRDF_PHONG)
        return EvaluatePhongBRDF(N, V, L, radiance);
    else
        return EvaluatePBR(N, V, L, radiance);
}



vec3 processLighting()
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

vec3 ACES(vec3 color)
{
    const float a = 2.51;
	const float b = 0.03;
	const float c = 2.43;
	const float d = 0.59;
	const float e = 0.14;
	return clamp((color * (a * color + b)) / (color * (c * color + d) + e), 0.0, 1.0);
}

void main()
{
	vec3 ambient = lightBlock.ambientColorIntensity.rgb * lightBlock.ambientColorIntensity.a;
	vec3 lit = vec3(0);

	lit += processLighting();

    FragColor = vec4(ACES(vColor.rgb * (ambient + lit)), vColor.a);
}