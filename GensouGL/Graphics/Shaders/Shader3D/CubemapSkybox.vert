#version 460

layout ( location = 0 ) in vec3 position;

layout ( location = 0 ) uniform mat4 modelMatrix;
layout ( location = 1 ) uniform mat4 viewMatrix;
layout ( location = 2 ) uniform mat4 projMatrix;

out vec3 vTexCoord;

void main()
{
	vTexCoord = position;
	vec4 pos = projMatrix * viewMatrix * vec4(position, 1.0);
	gl_Position = pos.xyww;
}