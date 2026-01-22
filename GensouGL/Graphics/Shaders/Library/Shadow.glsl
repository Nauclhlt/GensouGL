float ShadowPCF(sampler2D shadowMap, vec2 uv, float currentDepth, float bias, int size)
{
    float shadow = 0.0;
    vec2 texelSize = 1.0 / textureSize(shadowMap, 0);

    int offset = (size - 1) / 2;
    for (int x = -offset; x <= offset; ++x)
    for (int y = -offset; y <= offset; ++y)
    {
        float d = texture(shadowMap, uv + vec2(x, y) * texelSize).r;
        shadow += currentDepth - bias > d ? 0.0 : 1.0;
    }

    return shadow / (size * size);
}

float ComputeBias(vec3 normal, vec3 lightDir)
{
    float ndotl = max(dot(normal, lightDir), 0.0);

    float slopeBias = 0.005 * (1.0 - ndotl);
    float constantBias = 0.004;

    return constantBias;
}

float CalcShadow(vec4 viewPos)
{
	float depth = -viewPos.z;
    int cascade = 0;
    for (int i = 0; i < CASCADE_COUNT - 1; i++)
    {
        if (depth >= pCascadeSplits[i + 1])
            cascade = i + 1;
    }

    vec4 lightClip = pCascadeLightVP[cascade] * vec4(vPosition, 1.0);
	vec3 ndc = lightClip.xyz / lightClip.w;
	vec2 uv = ndc.xy * 0.5 + 0.5;
	float currentDepth = ndc.z * 0.5 + 0.5;

	return ShadowPCF(pShadowMaps[cascade], uv, currentDepth, ComputeBias(vNormal, pShadowLightDirection), cascade == 0 ? 5 : 3);
}