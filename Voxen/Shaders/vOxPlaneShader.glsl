#version 460

uniform vec3 cameraPos;
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

layout (location = 0) in vec3 vertexPosition;

layout (location = 1) out vec4 fragPos;
layout (location = 3) out mat4 fragView;
layout (location = 7) out mat4 fragProj;

void main()
{
    fragView = view;
    fragProj = projection;
    
    // The y component of the camera is ignored: we only want to displace the vertices on the XZ axes
    vec3 cameraCenteredVertexPos = vec3(vertexPosition.x + cameraPos.x, vertexPosition.y, vertexPosition.z + cameraPos.z);
    
    fragPos = vec4(cameraCenteredVertexPos, 1.0f) * model;
    
    gl_Position = vec4(cameraCenteredVertexPos, 1.0f) * view * projection;
}