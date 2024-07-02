using Voxen.Components;

namespace Voxen.Entities;

public class Chunk : IEntity
{
    #region IEntity Properties

    public Guid Guid { get; }
    
    #endregion
    
    #region Components

    public ChunkData Data { get; }
    
    public ChunkRenderInformation RenderInformation { get; }

    #endregion

    #region Constructors

    public Chunk(ChunkData chunkData)
    {
        Guid = Guid.NewGuid();

        Data = chunkData;
        RenderInformation = new ChunkRenderInformation(Data);
    }

    #endregion
}