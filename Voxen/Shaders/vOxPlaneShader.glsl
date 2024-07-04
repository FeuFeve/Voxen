#version 460

uniform vec3 cameraPos;
uniform mat4 view;
uniform mat4 projection;

layout (location = 0) in vec3 vertexPosition;

layout (location = 1) out vec3 fragPos;

void main()
{
    // The y component of the camera is ignored: we only want to displace the vertices on the XZ axes
    vec3 cameraCenteredVertexPos = vec3(vertexPosition.x + cameraPos.x, vertexPosition.y, vertexPosition.z + cameraPos.z);
    
    fragPos = cameraCenteredVertexPos;
    
    gl_Position = vec4(cameraCenteredVertexPos, 1.0f) * view * projection;
}