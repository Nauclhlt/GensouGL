#version 460

layout ( location = 0 ) in vec3 position;
layout ( location = 1 ) in vec4 color;
layout ( location = 2 ) in vec3 normal;

layout ( location = 0 ) uniform mat4 modelMatrix;
layout ( location = 1 ) uniform mat4 viewMatrix;
layout ( location = 2 ) uniform mat4 projMatrix;

out vec4 vColor;
out vec3 vNormal;

void main()
{
	gl_Position = projMatrix * viewMatrix * modelMatrix * vec4(position, 1);
	//gl_Position = vec4(position, 1) * modelMatrix * viewMatrix * projMatrix;
	//gl_Position = vec4(position.xy, 0, 1);
	vColor = color;
	vNormal = normal;
}