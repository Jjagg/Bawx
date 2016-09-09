namespace Bawx
{
    public struct Block
    {
        public readonly  BlockData Data;

        // index in the chunk
        public readonly int Index;

        public Block(BlockData data, int index)
        {
            Data = data;
            Index = index;
        }
    }
}