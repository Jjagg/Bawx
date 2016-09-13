using Bawx.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bawx.Rendering
{
    public class InstancedChunkRenderer : ChunkRenderer
    {
        private readonly BlockData[] _tmpBlockData;
        private readonly VertexBufferBinding[] _bufferBindings;
        // the instance vertex buffer
        private VertexBuffer _vertexBuffer;
        private int _activeCount;

        public override int FreeBlocks => _vertexBuffer == null ? 0 : _vertexBuffer.VertexCount - BlockCount;

        public InstancedChunkRenderer(GraphicsDevice graphicsDevice, Vector4[] palette) : base(graphicsDevice, palette)
        {
            _tmpBlockData = new BlockData[1];
            _bufferBindings = new VertexBufferBinding[2];
            _bufferBindings[0] = new VertexBufferBinding(CubeBuilder.GetNormalBuffer(graphicsDevice), 0, 0);
        }

        protected override void InitializeInternal(BlockData[] data, int active, int maxBlocks)
        {
            _vertexBuffer = new VertexBuffer(GraphicsDevice, BlockData.VertexDeclaration, maxBlocks, BufferUsage.WriteOnly);
            _vertexBuffer.SetData(data);
            _bufferBindings[1] = new VertexBufferBinding(_vertexBuffer, 0, 1);

            _activeCount = active;
        }

        public override void SetBlock(BlockData block, int index)
        {
            // TODO take active blocks into account
            if (index >= BlockCount) _activeCount = index + 1;
            _tmpBlockData[0] = block;
            _vertexBuffer.SetData(BlockDataSize * index, _tmpBlockData, 0, 1, BlockDataSize);
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
        }

        protected override void DrawInternal()
        {
            GraphicsDevice.SetVertexBuffers(_bufferBindings);
            GraphicsDevice.Indices = CubeBuilder.GetShortIndexBuffer(GraphicsDevice);
            GraphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, 0, 0, 
                12, _activeCount);
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
