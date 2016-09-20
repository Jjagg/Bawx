using System;
using System.Collections.Generic;
using Bawx.VertexTypes;
using Microsoft.Xna.Framework;

namespace Bawx.VoxelData
{
    public class BlockGrid : BlockContainer
    {
        public Vector3 Position { get; set; }
        public readonly byte SizeX;
        public readonly byte SizeY;
        public readonly byte SizeZ;

        private BoundingBox _bounds;
        public override BoundingBox Bounds => _bounds;

        private BlockData[][][] _grid;

        /// <summary>
        /// Create a new BlockGrid.
        /// </summary>
        /// <param name="position">The center of the first block in this grid. I.e. the block at {0, 0, 0}.</param>
        /// <param name="sizeX">The number of blocks in the grid along the x-axis.</param>
        /// <param name="sizeY">The number of blocks in the grid along the y-axis.</param>
        /// <param name="sizeZ">The number of blocks in the grid along the z-axis.</param>
        public BlockGrid(Vector3 position, byte sizeX, byte sizeY, byte sizeZ)
        {
            Position = position;
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
            _bounds = new BoundingBox(Position, Position + new Vector3(sizeX, sizeY, sizeZ));

            InitializeGrid();
        }

        private void InitializeGrid()
        {
            _grid = new BlockData[SizeX][][];
            for (var x = 0; x < SizeX; x++)
            {
                _grid[x] = new BlockData[SizeY][];

                for (var y = 0; y < SizeY; y++)
                    _grid[x][y] = new BlockData[SizeZ];
            }
        }

        public override BlockData Get(byte x, byte y, byte z)
        {
            return _grid[x][y][z];
        }

        public override void AddRange(List<Block> blocks)
        {
            foreach (var b in blocks)
                _grid[b.X][b.Y][b.Z] = new BlockData(b.Index);
        }

        public override void Add(byte x, byte y, byte z, BlockData block)
        {
            _grid[x][y][z] = new BlockData(block.Index);
        }

        public override void Remove(byte x, byte y, byte z)
        {
            _grid[x][y][z] = BlockData.Empty;
        }

        public override void Clear()
        {
            for (var x = 0; x < SizeX; x++)
            {
                for (var y = 0; y < SizeY; y++)
                    Array.Clear(_grid[x][y], 0, SizeZ);
            }
        }

        public override bool Intersect(Ray ray, out BlockIntersection intersection)
        {
            // TODO test MG implementation
            if (!Bounds.Intersects(ray).HasValue)
            {
                intersection = default(BlockIntersection);
                return false;
            }
            throw new NotImplementedException();
        }
    }
}