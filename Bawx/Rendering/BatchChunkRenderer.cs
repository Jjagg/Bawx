using System;
using Bawx.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bawx.Rendering
{
    /// <summary>
    /// Keep blocks of a chunk in an array to batch them in a single draw call.
    /// This is the most straightforward way of doing the rendering, but far from optimal since a lot of duplicate information is sent.
    /// <see cref="InstancedChunkRenderer"/> and <see cref="PolygonChunkRenderer"/> perform better than this.
    /// </summary>
    public class BatchChunkRenderer : ChunkRenderer
    {
        private VertexPositionNormalColor[] _vertices;
        private short[] _indices;
        private int _vertexCount;
        private int _indexCount;

        public override int FreeBlocks => _vertices.Length - BlockCount;

        public BatchChunkRenderer(GraphicsDevice graphicsDevice, Vector4[] palette) : base(graphicsDevice, palette)
        {
        }

        protected override void InitializeInternal(BlockData[] blockData, int active, int maxBlocks)
        {
            _vertexCount = blockData.Length*24;
            _indexCount = blockData.Length*36;
            _vertices = new VertexPositionNormalColor[maxBlocks*24];
            _indices = new short[maxBlocks * 36];

            var inds = CubeBuilder.GetShortIndices();

            for (var i = 0; i < blockData.Length; i++)
            {
                var d = blockData[i];
                // lots of garbage here maybe modfiy CubeBuilder to insert values into the array
                var v = CubeBuilder.GetNormalColor(d.Position, new Color(Effect.Palette[d.Index]));

                Array.Copy(v, 0, _vertices, i * v.Length, v.Length);
                Array.Copy(inds, 0, _indices, i * inds.Length, inds.Length);
            }
        }

        public override void SetBlock(BlockData block, int index)
        {
            throw new NotImplementedException();
        }

        public override void RemoveBlock(int index)
        {
            throw new NotImplementedException();
        }

        protected override void RebuildInternal(int maxBlocks)
        {
            throw new NotImplementedException();
        }

        protected override void PreDraw()
        {
            Effect.CurrentTechnique = Effect.BatchTechnique;
        }

        protected override void DrawInternal()
        {
            GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _vertices, 0, _vertexCount, _indices, 0, 
                _vertexCount / 2, VertexPositionNormalColor.VertexDeclaration);
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}
