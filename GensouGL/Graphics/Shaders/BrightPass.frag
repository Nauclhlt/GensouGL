#version 460

uniform sampler2D pTexture;
uniform float pThreshold;
uniform float pKnee;

in vec2 vTexCoord;

out vec4 FragColor;

void main()
{
	vec3 color = texture(pTexture, vTexCoord).rgb;
	float brightness = dot(color, vec3(0.2126, 0.7152, 0.0722));

	float soft = smoothstep(pThreshold, pThreshold + pKnee, brightness);

	FragColor = vec4(color * soft, 1.0);
}