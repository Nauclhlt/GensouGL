#version 460

layout ( location = 0 ) in vec2 position;
layout ( location = 1 ) in vec4 color;

layout ( location = 0 ) uniform mat4 vMatrix;

out vec4 vColor;

void main()
{
	gl_Position = vMatrix * vec4(position.x, position.y, 0, 1);
	vColor = color;
}