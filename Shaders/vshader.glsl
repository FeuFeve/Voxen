#version 460

layout(location=0) in vec3 vertices;
layout(location=1) in vec3 colors;

out vec3 fragColor;

void main()
{
    fragColor = colors;

    gl_Position = vec4(vertices, 1.0f);
}