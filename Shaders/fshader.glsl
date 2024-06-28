#version 460

layout(std430) buffer ByteArray
{
    double data[];
};

in vec3 outColor;

out vec4 fragColor;

void main()
{
    fragColor = vec4(outColor, 1.0f);
}

