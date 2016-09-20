using Bawx.Util;
using Bawx.VertexTypes;
using Bawx.VoxelData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bawx.Rendering.ChunkRenderers
{
    public class InstancedChunkRenderer : ChunkRenderer
    {
        private readonly Block[] _tmpBlock;
        private readonly VertexBufferBinding[] _bufferBindings;
        // the instance vertex buffer
        private VertexBuffer _vertexBuffer;
        public int ActiveCount { get; private set; }

        public override int FreeBlocks => _vertexBuffer == null ? 0 : _vertexBuffer.VertexCount - BlockCount;

        public InstancedChunkRenderer(GraphicsDevice graphicsDevice, Vector4[] palette) : base(graphicsDevice, palette)
        {
            _tmpBlock = new Block[1];
            _bufferBindings = new VertexBufferBinding[2];
            _bufferBindings[0] = new VertexBufferBinding(CubeBuilder.GetNormalBuffer(graphicsDevice), 0, 0);
        }

        protected override void InitializeInternal(Chunk chunk, int active, int maxBlocks)
        {
            _vertexBuffer = new VertexBuffer(GraphicsDevice, Block.VertexDeclaration, maxBlocks, BufferUsage.WriteOnly);
            _vertexBuffer.SetData(chunk.TmpBlocks);
            _bufferBindings[1] = new VertexBufferBinding(_vertexBuffer, 0, 1);

            ActiveCount = active;
        }

        public override void SetBlock(Block block, int index)
        {
            // TODO take active blocks into account
            if (index >= BlockCount) ActiveCount = index + 1;
            _tmpBlock[0] = block;
            _vertexBuffer.SetData(BlockDataSize * index, _tmpBlock, 0, 1, BlockDataSize);
        }

        public override void RemoveBlock(int index)
        {
            throw new System.NotImplementedException();
        }

        protected override void RebuildInternal(int maxBlocks)
        {
            throw new System.NotImplementedException();
        }

        protected override void PreDraw()
        {
            Effect.CurrentTechnique = Effect.InstancingTechnique;
        }

        protected override void DrawInternal()
        {
            GraphicsDevice.SetVertexBuffers(_bufferBindings);
            GraphicsDevice.Indices = CubeBuilder.GetShortIndexBuffer(GraphicsDevice);
            GraphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, 0, 0, 
                12, ActiveCount);
        }

        protected override void Dispose(bool disposing)
        {
            if (_vertexBuffer != null)
            {
                _vertexBuffer.Dispose();
                _vertexBuffer = null;
            }
        }
    }
}
