using OpenTK.Mathematics;
using Voxen.Entities;

namespace Voxen.Renderers;

public struct ChunkRenderInformation
{
    #region Constructors

    public ChunkRenderInformation(ref Chunk chunk)
    {
        ChunkModelMatrix = Matrix4.CreateTranslation(
            x: chunk.ChunkCoordinates.X * Chunk.CHUNK_SIZE * Chunk.VOXEL_SIZE,
            y: chunk.ChunkCoordinates.Y * Chunk.CHUNK_SIZE * Chunk.VOXEL_SIZE,
            z: chunk.ChunkCoordinates.Z * Chunk.CHUNK_SIZE * Chunk.VOXEL_SIZE
        );
    }

    #endregion
    
    #region Properties
    
    public Matrix4 ChunkModelMatrix { get; }

    #endregion
    
    #region Constants

    public static readonly float[] BBOX_VERTICES =
    [
        0,                                   0,                                   0,
        0,                                   0,                                   Chunk.CHUNK_SIZE * Chunk.VOXEL_SIZE,
        Chunk.CHUNK_SIZE * Chunk.VOXEL_SIZE, 0,                                   Chunk.CHUNK_SIZE * Chunk.VOXEL_SIZE,
        Chunk.CHUNK_SIZE * Chunk.VOXEL_SIZE, 0,                                   0,
        Chunk.CHUNK_SIZE * Chunk.VOXEL_SIZE, Chunk.CHUNK_SIZE * Chunk.VOXEL_SIZE, 0,
        Chunk.CHUNK_SIZE * Chunk.VOXEL_SIZE, Chunk.CHUNK_SIZE * Chunk.VOXEL_SIZE, Chunk.CHUNK_SIZE * Chunk.VOXEL_SIZE,
        0,                                   Chunk.CHUNK_SIZE * Chunk.VOXEL_SIZE, Chunk.CHUNK_SIZE * Chunk.VOXEL_SIZE,
        0,                                   Chunk.CHUNK_SIZE * Chunk.VOXEL_SIZE, 0,
    ];

    public static readonly byte[] BBOX_EDGE_INDICES =
    [
        0, 1, 1, 2, 2, 3, 3, 0, // Bottom face
        4, 5, 5, 6, 6, 7, 7, 4, // Top face
        0, 4, 1, 5, 2, 6, 3, 7, // Vertical edges
    ];

    #endregion
}