using System;
using Microsoft.Xna.Framework;

namespace Bawx.VoxelData
{
    public class BlockGrid : BlockContainer
    {
        public Vector3 Position { get; set; }

        private BoundingBox _bounds;
        public override BoundingBox Bounds => _bounds;

        private BlockData[][][] _grid;

        /// <summary>
        /// Create a new BlockGrid.
        /// </summary>
        public BlockGrid(Chunk chunk) : base(chunk)
        {
            _bounds = new BoundingBox(Position, Position + new Vector3(chunk.SizeX, chunk.SizeY, chunk.SizeZ));
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