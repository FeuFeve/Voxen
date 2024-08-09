// using OpenTK.Mathematics;
//
// namespace Voxen.World;
//
// public class ChunkGenerator
// {
//     #region Constructors
//
//     public ChunkGenerator()
//     {
//         _noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
//     }
//
//     #endregion
//
//     #region Public methods
//     
//     public ChunkData GenerateNewChunk(Vector3i chunkCoordinates)
//     {
//         // TODO: handle underground chunks
//         if (chunkCoordinates.Y < 0)
//         {
//             return new ChunkData
//             {
//                 Voxels = [VoxelType.Stone],
//             };
//         }
//         
//         // TODO: handle "sky" chunks
//         if (chunkCoordinates.Y > 0)
//         {
//             return new ChunkData
//             {
//                 Voxels = [VoxelType.Air],
//             };
//         }
//
//         float[,] noiseData = GenerateNoiseData(ref chunkCoordinates);
//         VoxelType[,,] lod0 = GenerateLod0FromNoiseData(ref noiseData);
//         
//     }
//
//     #endregion
//
//     #region Private methods
//
//     private float[,] GenerateNoiseData(ref Vector3i chunkCoordinates)
//     {
//         float[,] noiseData = new float[ChunkData.CHUNK_SIZE, ChunkData.CHUNK_SIZE];
//
//         int startingX = chunkCoordinates.X * ChunkData.CHUNK_SIZE;
//         int endingX = startingX + ChunkData.CHUNK_SIZE;
//         int startingZ = chunkCoordinates.Z + ChunkData.CHUNK_SIZE;
//         int endingZ = startingZ + ChunkData.CHUNK_SIZE;
//
//         for (int x = startingX; x < endingX; x++)
//         {
//             for (int z = startingZ; z < endingZ; z++)
//             {
//                 noiseData[x, z] = _noise.GetNoise(x, z);
//             }
//         }
//
//         return noiseData;
//     }
//
//     private static VoxelType[,,] GenerateLod0FromNoiseData(ref float[,] noiseData)
//     {
//         VoxelType[,,] lod0 = new VoxelType[ChunkData.CHUNK_SIZE, ChunkData.CHUNK_SIZE, ChunkData.CHUNK_SIZE];
//
//         for (int x = 0; x < ChunkData.CHUNK_SIZE; x++)
//         {
//             for (int z = 0; z < ChunkData.CHUNK_SIZE; z++)
//             {
//                 float terrainHeight = noiseData[x, z];
//                 short blocksToStone = 4;
//
//                 for (int y = ChunkData.CHUNK_SIZE - 1; y >= 0; y--)
//                 {
//                     float currentHeight = (float)y / ChunkData.CHUNK_SIZE;
//                     
//                     if (currentHeight > terrainHeight)
//                     {
//                         // Air (air should be 0, and the array should be already initialized to 0)
//                         continue;
//                     }
//                     else if (blocksToStone == 4)
//                     {
//                         blocksToStone--;
//                         lod0[x, y, z] = VoxelType.Grass;
//                     }
//                     else if (blocksToStone > 0)
//                     {
//                         blocksToStone--;
//                         lod0[x, y, z] = VoxelType.Dirt;
//                     }
//                     else
//                     {
//                         lod0[x, y, z] = VoxelType.Stone;
//                     }
//                 }
//             }
//         }
//
//         return lod0;
//     }
//
//     private static ChunkData GenerateChunkFromLod0(ref VoxelType[,,] lod0)
//     {
//         ushort lodCount
//     }
//
//     #endregion
//
//     #region Fields
//
//     private FastNoiseLite _noise = new();
//
//     #endregion
// }