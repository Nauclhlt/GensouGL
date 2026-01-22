#version 460

layout ( location = 0 ) in vec3 position;
layout ( location = 1 ) in vec4 color;
layout ( location = 2 ) in vec3 normal;

layout ( location = 0 ) uniform mat4 modelMatrix;
layout ( location = 1 ) uniform mat4 viewMatrix;
layout ( location = 2 ) uniform mat4 projMatrix;

out vec4 vColor;
out vec3 vNormal;
out vec3 vPosition;
out vec4 vViewPos;

void main()
{
	gl_Position = projMatrix * viewMatrix * modelMatrix * vec4(position, 1);
	vPosition = (modelMatrix * vec4(position, 1)).xyz;
	vColor = color;
	vNormal = normalize(mat3(transpose(inverse(modelMatrix))) * normal);
	vViewPos = viewMatrix * modelMatrix * vec4(position, 1);
}