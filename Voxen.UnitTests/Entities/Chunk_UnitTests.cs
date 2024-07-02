using Voxen.Entities;

namespace Voxen.UnitTests;

public class Chunk_UnitTests
{
    [Fact]
    public void ChunkSizeShouldBeAFactorOf2()
    {
        //-----------------------------------------------------
        // Verify
        //-----------------------------------------------------
        
        Assert.True(Chunk.CHUNK_SIZE > 0);
        Assert.Equal(0, Chunk.CHUNK_SIZE & (Chunk.CHUNK_SIZE - 1));
    }
}