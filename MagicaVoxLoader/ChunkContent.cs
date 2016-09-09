using Bawx;
using Microsoft.Xna.Framework;

namespace MagicaVoxLoader
{
    public class ChunkContent
    {
        public readonly Vector3 Position;
        public readonly int SizeX;
        public readonly int SizeY;
        public readonly int SizeZ;
        public readonly BlockData[] Blocks;

        public ChunkContent(Vector3 position, int sizeX, int sizeY, int sizeZ, BlockData[] blocks)
        {
            Position = position;
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
            Blocks = blocks;
        }
    }
}