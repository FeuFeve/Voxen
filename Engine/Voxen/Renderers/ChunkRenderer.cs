using OpenTK.Graphics.OpenGL4;
using Voxen.Components;
using Voxen.Entities;

namespace Voxen.Renderers;

public struct ChunkRenderer : IRenderer<Chunk>
{
    #region Constructors

    public ChunkRenderer(Shader shader)
    {
        _shader = shader;
    }

    #endregion
    
    #region IRenderer implementation

    public void Render(Chunk chunk)
    {
        _shader.SetMatrix4(CommonConstants.ShaderModelMatrixName, chunk.RenderInformation.ChunkModelMatrix);
        
        GL.DrawElements(PrimitiveType.Lines, ChunkRenderInformation.BBOX_EDGE_INDICES.Length, DrawElementsType.UnsignedByte, 0);
    }

    #endregion

    #region Fields

    private readonly Shader _shader;

    #endregion
}