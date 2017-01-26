using Bawx.Rendering.ChunkRenderers;
using Bawx.VertexTypes;
using Microsoft.Xna.Framework;

namespace MagicaVoxLoader
{
    public class ChunkContent
    {
        public readonly Vector3 Position;
        public readonly int SizeX;
        public readonly int SizeY;
        public readonly int SizeZ;
        public readonly Block[] Blocks;
        public readonly Color[] Palette;
        public readonly int ActiveBlocks;
        public int BlockCount => Blocks.Length;

        public ChunkContent(Vector3 position, int sizeX, int sizeY, int sizeZ, 
            Block[] blocks, Color[] palette, int activeBlocks)
        {
            Position = position;
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
            Blocks = blocks;
            Palette = palette;
            ActiveBlocks = activeBlocks;
        }
    }
}