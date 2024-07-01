namespace Voxen.World;

public struct Chunk
{
    #region Constants

    // The chunks are be CHUNK_SIZE * CHUNK_SIZE * CHUNK_SIZE voxels
    public const ushort CHUNK_SIZE = 16;

    // The voxels are VOXEL_SIZE meters * VOXEL_SIZE meters * VOXEL_SIZE meters
    public const float VOXEL_SIZE = 1.0f;

    #endregion
    
    public VoxelType[] Voxels;
}