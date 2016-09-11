using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bawx.Rendering
{
    public class PolygonChunkRenderer : ChunkRenderer
    {
        public override int FreeBlocks { get; }

        public PolygonChunkRenderer(GraphicsDevice graphicsDevice, Vector4[] palette) : base(graphicsDevice, palette)
        {
        }

        protected override void InitializeInternal(BlockData[] blockData, int maxBlocks)
        {
            throw new System.NotImplementedException();
        }

        public override void SetBlock(BlockData block, int index)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveBlock(int index)
        {
            throw new System.NotImplementedException();
        }

        protected override void RebuildInternal(int maxBlocks)
        {
            throw new System.NotImplementedException();
        }

        protected override void DrawInternal()
        {
            throw new System.NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            throw new System.NotImplementedException();
        }
    }
}