using OpenTK.Mathematics;

namespace Voxen.Components;

public struct ChunkRenderInformation
{
    #region Constructors

    public ChunkRenderInformation(ref ChunkData chunkData)
    {
        ChunkModelMatrix = Matrix4.CreateTranslation(
            x: chunkData.ChunkCoordinates.X * BBOX_MAX_VALUE,
            y: chunkData.ChunkCoordinates.Y * BBOX_MAX_VALUE,
            z: chunkData.ChunkCoordinates.Z * BBOX_MAX_VALUE
        );
    }

    #endregion
    
    #region Properties
    
    public Matrix4 ChunkModelMatrix { get; }

    #endregion
    
    #region Constants

    private const float BBOX_MAX_VALUE = ChunkData.CHUNK_SIZE * ChunkData.VOXEL_SIZE;

    public static readonly float[] BBOX_VERTICES =
    [
        0,              0,              0,
        0,              0,              BBOX_MAX_VALUE,
        BBOX_MAX_VALUE, 0,              BBOX_MAX_VALUE,
        BBOX_MAX_VALUE, 0,              0,
        BBOX_MAX_VALUE, BBOX_MAX_VALUE, 0,
        BBOX_MAX_VALUE, BBOX_MAX_VALUE, BBOX_MAX_VALUE,
        0,              BBOX_MAX_VALUE, BBOX_MAX_VALUE,
        0,              BBOX_MAX_VALUE, 0,
    ];

    public static readonly byte[] BBOX_EDGE_INDICES =
    [
        0, 1, 1, 2, 2, 3, 3, 0, // Bottom face
        4, 5, 5, 6, 6, 7, 7, 4, // Top face
        0, 4, 1, 5, 2, 6, 3, 7, // Vertical edges
    ];

    #endregion
}