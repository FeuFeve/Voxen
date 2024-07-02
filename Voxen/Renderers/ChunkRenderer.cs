using OpenTK.Graphics.OpenGL4;
using Voxen.Entities;

namespace Voxen.Renderers;

public struct ChunkRenderer
{
    
    public void Render(Chunk chunk, Shader shader)
    {
        shader.SetMatrix4("model", chunk.ChunkModelMatrix);
        
        GL.DrawElements(PrimitiveType.Lines, Chunk.BBOX_EDGE_INDICES, DrawElementsType.UnsignedByte, 0);
    }
}