using OpenTK.Mathematics;
using Voxen.Entities;
using Voxen.World;

namespace Voxen.Components;

public struct ChunkData
{
    #region Properties

    public Vector3i ChunkCoordinates { get; set; }
    
    public VoxelType[] Voxels { get; set; } = [];

    #endregion
    
    #region Constructors

    public ChunkData(ref Vector3i chunkCoordinates)
    {
        ChunkCoordinates = chunkCoordinates;
    }

    #endregion
    
    #region Constants

    // The chunks are CHUNK_SIZE * CHUNK_SIZE * CHUNK_SIZE voxels
    public const ushort CHUNK_SIZE = 16;

    // The voxels are VOXEL_SIZE meters * VOXEL_SIZE meters * VOXEL_SIZE meters
    public const float VOXEL_SIZE = 1.0f;

    #endregion
}