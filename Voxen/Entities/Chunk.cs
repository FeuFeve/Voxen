using Voxen.Components;

namespace Voxen.Entities;

public struct Chunk : IEntity
{
    #region IEntity Properties

    public Guid Guid { get; }
    
    #endregion
    
    #region Components

    public ChunkData Data { get; }
    
    public ChunkRenderInformation RenderInformation { get; }

    #endregion

    #region Constructors

    public Chunk()
    {
        Guid = Guid.NewGuid();
    }

    #endregion
}