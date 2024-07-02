using OpenTK.Mathematics;
using Voxen.World;

namespace Voxen.Entities;

public struct Chunk : IEntity
{
    #region Constructor

    public Chunk()
    {
        Guid = Guid.NewGuid();
    }

    #endregion

    #region Properties

    public Guid Guid { get; }

    public Vector3i ChunkCoordinates { get; set; }
    
    public VoxelType[] Voxels { get; set; } = [];

    #endregion
    
    #region Constants

    // The chunks are CHUNK_SIZE * CHUNK_SIZE * CHUNK_SIZE voxels
    public const ushort CHUNK_SIZE = 16;

    // The voxels are VOXEL_SIZE meters * VOXEL_SIZE meters * VOXEL_SIZE meters
    public const float VOXEL_SIZE = 1.0f;

    #endregion
}