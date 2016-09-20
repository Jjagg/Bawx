using Bawx.VertexTypes;

namespace Bawx.VoxelData
{
    /// <summary>
    /// Contains all data that is specific to a single <see cref="Block"/>. Note that the
    /// difference between this and a <see cref="Block"/> is that a Block also contains its
    /// position within its chunk.
    /// </summary>
    public struct BlockData
    {
        public byte Index;

        public bool IsEmpty => Index == 0;

        public BlockData(byte index)
        {
            Index = index;
        }

        public static BlockData Empty => default(BlockData);
    }
}