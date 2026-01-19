vec3 EvaluatePhongBRDF( vec3 N, vec3 V, vec3 L, vec3 radiance )
{
    float NdotL = max(dot(N, L), 0.0);

    vec3 diffuse = radiance * NdotL;

    vec3 H = normalize(L + V);
    float spec = pow(max(dot(N, H), 0.0), material.shininess);

    vec3 specular = radiance * spec;

    return diffuse + specular;
}