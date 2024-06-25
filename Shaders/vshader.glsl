#version 460

layout (location = 0) in vec3 vertex;
layout (location = 1) in vec3 color;

out vec3 fragColor;

void main()
{
    fragColor = color;

    gl_Position = vec4(vertex, 1.0f);
}