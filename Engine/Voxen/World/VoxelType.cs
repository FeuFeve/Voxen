namespace Voxen.World;

public enum VoxelType : byte
{
    Air = 0,
    Stone = 1,
    Dirt = 2,
    Grass = 3,
    // 128-255 are reserved for partial blocks (used for LODs)
}