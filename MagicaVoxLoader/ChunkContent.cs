using Bawx;
using Bawx.Rendering;
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
        public readonly Color[] Palette;
        public readonly int ActiveBlocks;
        public int BlockCount => Blocks.Length;
        public ChunkRendererType RendererType;

        public ChunkContent(Vector3 position, int sizeX, int sizeY, int sizeZ, 
            BlockData[] blocks, Color[] palette, int activeBlocks, ChunkRendererType rendererType)
        {
            Position = position;
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
            Blocks = blocks;
            Palette = palette;
            ActiveBlocks = activeBlocks;
            RendererType = rendererType;
        }
    }
}