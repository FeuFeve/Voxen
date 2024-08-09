using Voxen.Components;

namespace Voxen.UnitTests.Components;

public class ChunkData_UnitTests
{
    [Fact]
    public void ChunkSizeShouldBeAFactorOf2()
    {
        //-----------------------------------------------------
        // Verify
        //-----------------------------------------------------
        
        Assert.True(ChunkData.CHUNK_SIZE > 0);
        Assert.Equal(0, ChunkData.CHUNK_SIZE & (ChunkData.CHUNK_SIZE - 1));
    }
}