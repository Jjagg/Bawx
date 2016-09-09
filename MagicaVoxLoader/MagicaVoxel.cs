namespace MagicaVoxLoader
{
    public struct MagicaVoxel
    {
        public byte X;
        public byte Y;
        public byte Z;

        // 0 for empty voxels
        public byte ColorIndex;
        public bool IsEmpty => ColorIndex == 0;
    }
}