#version 460

layout ( location = 0 ) in vec2 vertPosition;
layout ( location = 1 ) in vec4 color;
layout ( location = 2) in vec2 texCoord;
layout ( location = 3 ) in vec2 position;
layout ( location = 4 ) in float rotation;
layout ( location = 5 ) in vec2 scale;

layout ( location = 0 ) uniform mat4 vMatrix;

out vec4 vColor;
out vec2 vTexCoord;

// ( cos, sin )
// ( -sin, cos )

void main()
{
	vec2 sv = vertPosition * scale;
	vec2 rot = vec2(sv.x * cos(rotation) + sv.y * sin(rotation), -sv.x * sin(rotation) + sv.y * cos(rotation));
	gl_Position = vMatrix * vec4(position + rot, 0, 1);

	vColor = color;
	vTexCoord = texCoord;
}