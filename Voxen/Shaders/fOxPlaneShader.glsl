#version 460

float near = 0.01f;
float far = 5.0f;

layout(location = 0) out vec4 outColor;

layout(location = 1) in vec3 nearPoint; // nearPoint calculated in vertex shader
layout(location = 2) in vec3 farPoint; // farPoint calculated in vertex shader

layout(location = 3) in mat4 fragView;
layout(location = 7) in mat4 fragProj;

vec4 Grid(vec3 fragPos3D, float scale, bool drawAxis) {
    vec2 coord = fragPos3D.xz * scale; // use the scale variable to set the distance between the lines
    vec2 derivative = fwidth(coord);

    vec2 grid = abs(fract(coord - 0.5f) - 0.5f) / derivative;
    float line = min(grid.x, grid.y);

    vec4 color = vec4(0.2f, 0.2f, 0.2f, 1.0f - min(line, 1.0f));

    if (drawAxis)
    {
        // z axis
        float minimumX = min(derivative.x, 1.0f);
        if (fragPos3D.x > -1.0f * minimumX && fragPos3D.x < 1.0f * minimumX)
        {
            color = vec4(0.0f, 0.0f, 1.0f, 1.0f);
        }

        // x axis
        float minimumZ = min(derivative.y, 1.0f);
        if (fragPos3D.z > -1.0f * minimumZ && fragPos3D.z < 1.0f * minimumZ)
        {
            color = vec4(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    return color;
}

float ComputeDepth(vec3 pos) {
    vec4 clipSpacePos = vec4(pos, 1.0f) * fragView * fragProj;
    return (clipSpacePos.z / clipSpacePos.w);
}

float ComputeLinearDepth(vec3 pos) {
    vec4 clipSpacePos = vec4(pos, 1.0f) * fragView * fragProj;
    float clipSpaceDepth = (clipSpacePos.z / clipSpacePos.w) * 2.0 - 1.0; // put back between -1 and 1
    float linearDepth = (2.0f * near * far) / (far + near - clipSpaceDepth * (far - near)); // get linear value between near and far
    return linearDepth / far; // normalize
}

void main() {
    float t = -nearPoint.y / (farPoint.y - nearPoint.y);
    vec3 fragPos3D = nearPoint + t * (farPoint - nearPoint);

    gl_FragDepth = ComputeDepth(fragPos3D);
    
    float linearDepth = ComputeLinearDepth(fragPos3D);
    float fading = max(0.0f, 0.5f - linearDepth);
    
    outColor = (Grid(fragPos3D, 10, false) + Grid(fragPos3D, 1.0f, true)) * float(t > 0.0f);
    outColor.a *= fading;
}