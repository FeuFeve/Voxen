#version 460

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

layout (location = 0) in vec3 vertexPosition;
layout (location = 1) in vec3 vertexColor;

out vec3 outColor;

void main()
{
    gl_Position = projection * view * model * vec4(vertexPosition, 1.0f);
    
    outColor = vertexColor;
}