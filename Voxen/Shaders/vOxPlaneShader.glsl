#version 460

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

layout (location = 0) in vec3 vertexPosition;

layout(location = 1) out vec3 nearPoint;
layout(location = 2) out vec3 farPoint;

layout(location = 3) out mat4 fragView;
layout(location = 7) out mat4 fragProj;

vec3 UnprojectPoint(float x, float y, float z, mat4 view, mat4 projection)
{
    mat4 viewInv = inverse(view);
    mat4 projInv = inverse(projection);
    vec4 unprojectedPoint = vec4(x, y, z, 1.0) * projInv * viewInv;
    return unprojectedPoint.xyz / unprojectedPoint.w;
}

void main()
{
    nearPoint = UnprojectPoint(vertexPosition.x, vertexPosition.y, 0.0f, view, projection).xyz;
    farPoint = UnprojectPoint(vertexPosition.x, vertexPosition.y, 1.0f, view, projection).xyz;
    
    fragView = view;
    fragProj = projection;
    
    gl_Position = vec4(vertexPosition, 1.0f);
}