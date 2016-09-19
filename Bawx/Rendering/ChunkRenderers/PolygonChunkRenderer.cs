﻿using System.Collections.Generic;
using Bawx.VertexTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bawx.Rendering.ChunkRenderers
{
    public class PolygonChunkRenderer : ChunkRenderer
    {
        public override int FreeBlocks => 0;

        private QuadData[] _vertices;
        private int[] _indices;

        #region Initialization

        public PolygonChunkRenderer(GraphicsDevice graphicsDevice, Vector4[] palette) : base(graphicsDevice, palette)
        {
        }

        protected override void InitializeInternal(Chunk chunk, int active, int maxBlocks)
        {
            var grid = BuildGrid(chunk);

            GreedyMesh.Generate(grid, CreateQuad, out _vertices, out _indices);
        }

        private IEnumerable<QuadData> CreateQuad(int[] p, int[] du, int[] dv, int normal, bool normalPos, GreedyMesh.VoxelFace voxelFace, bool backFace)
        {
            // normal is perpendicular to width and height
            var norm = DirectionToFace(normal, normalPos);
            var index = (byte) (voxelFace.Index - 1);

            // TODO -0.5 in world space before rendering in shader
            return new []
            {
                new QuadData((byte) p[0], (byte) p[1], (byte) p[2], index, norm),
                new QuadData((byte) (p[0] + du[0]), (byte) (p[1] + du[1]), (byte) (p[2] + du[2]), index, norm),
                new QuadData((byte) (p[0] + du[0] + dv[0]), (byte) (p[1] + du[1] + dv[1]), (byte) (p[2] + du[2] + dv[2]), index, norm),
                new QuadData((byte) (p[0] + dv[0]), (byte) (p[1] + dv[1]), (byte) (p[2] + dv[2]), index, norm),
            };
        }

        private byte DirectionToFace(int direction, bool positive)
        {
            return (byte) (direction*2 + (positive ? 0 : 1));
        }

        private static byte[][][] BuildGrid(Chunk chunk)
        {
            var grid = new byte[chunk.SizeX][][];
            for (var x = 0; x < chunk.SizeX; x++)
            {
                grid[x] = new byte[chunk.SizeY][];

                for (var y = 0; y < chunk.SizeY; y++)
                {
                    grid[x][y] = new byte[chunk.SizeZ];
                }
            }

	    // Index is zero-based, but GreedyMesh expects index 0 only for empty voxels!
            foreach (var block in chunk.BlockData)
                grid[block.X][block.Y][block.Z] = (byte) (block.Index + 1);

            return grid;
        }

        #endregion

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

        protected override void PreDraw()
        {
            Effect.CurrentTechnique = Effect.MeshTechnique;
        }

        protected override void DrawInternal()
        {
            GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _vertices, 
                0, _vertices.Length, _indices, 0, _indices.Length / 3, QuadData.VertexDeclaration);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _vertices = null;
                _indices = null;
            }
        }
    }
}