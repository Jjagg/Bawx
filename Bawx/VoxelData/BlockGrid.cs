using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Bawx.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bawx.VoxelData
{
    public class BlockGrid : BlockContainer
    {
        public Vector3 Position { get; set; }

        private BoundingBox _bounds;
        public override BoundingBox Bounds => _bounds;

        private BlockData[] _grid;

        // the outer faces of this chunk in chunk space
        private Plane[] _boundingPlanes;

        /// <summary>
        /// Create a new BlockGrid.
        /// </summary>
        public BlockGrid(Chunk chunk) : base(chunk)
        {
            _bounds = new BoundingBox(Position, Position + new Vector3(chunk.SizeX, chunk.SizeY, chunk.SizeZ));
            InitBoundingPlanes();
            InitializeGrid();
        }

        private void InitBoundingPlanes()
        {
            var size = Bounds.Max - Bounds.Min;
            _boundingPlanes = new Plane[6];
            _boundingPlanes[(int) CubeMapFace.PositiveX] = new Plane();
            _boundingPlanes[(int) CubeMapFace.NegativeX] = new Plane(-Vector3.UnitX, 0);
            _boundingPlanes[(int) CubeMapFace.PositiveY] = new Plane(Vector3.UnitY, size.Y);
            _boundingPlanes[(int) CubeMapFace.PositiveY] = new Plane(-Vector3.UnitY, 0);
            _boundingPlanes[(int) CubeMapFace.NegativeZ] = new Plane(Vector3.UnitZ, size.Z);
            _boundingPlanes[(int) CubeMapFace.NegativeZ] = new Plane(-Vector3.UnitZ, 0);
        }

        private void InitializeGrid()
        {
            _grid = new BlockData[SizeX*SizeY*SizeZ];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Offset(byte x, byte y, byte z)
        {
            return x + SizeX*y + SizeX*SizeY*z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte PosToVoxel(Vector3 pos, int axis)
        {
            var p = pos - Position;
            int val;
            switch (axis)
            {
                case 0:
                    val = (int) p.X;
                    if (val > SizeX - 1) val = SizeX - 1;
                    break;
                case 1:
                    val = (int) p.Y;
                    if (val > SizeY - 1) val = SizeY - 1;
                    break;
                case 2:
                    val = (int) p.Z;
                    if (val > SizeZ - 1) val = SizeZ - 1;
                    break;
                default:
                    throw new ArgumentException(nameof(axis));
            }
            if (val < 0) val = 0;
            return (byte) val;
        }

        public byte VoxelToPos(int axis)
        {
            return 0;
        }

        public override BlockData Get(byte x, byte y, byte z)
        {
            return _grid[Offset(x, y, z)];
        }

        public override void Add(byte x, byte y, byte z, BlockData block)
        {
            _grid[Offset(x, y, z)] = new BlockData(block.Index);
        }

        public override void Remove(byte x, byte y, byte z)
        {
            _grid[Offset(x, y, z)] = BlockData.Empty;
        }

        public override void Clear()
        {
            Array.Clear(_grid, 0, SizeX*SizeY*SizeZ);
        }

        public override bool Intersect(Ray ray, out BlockIntersection intersection)
        {
            /*float mint, maxt;
            if (Bounds.SlabIntersect(ray, out mint, out maxt) && maxt < 0f)
            {
                intersection = default(BlockIntersection);
                return false;
            }

            var pVec = ray.ValueAt(mint);

            var step = new int[3];
            var size = new[] {SizeX, SizeY, SizeZ};
            var deltaT = new[] {SizeX/ray.Direction.X, SizeY/ray.Direction.Y, SizeZ/ray.Direction.Z};

            
            BlockData current;
            while ((current = _grid[Offset(x, y, z)]).IsEmpty)
            {
                PosToVoxel(pos, out x, out y, out z);
                current = _grid[x][y][z];

                // Update bounds

            }*/
            intersection = new BlockIntersection();
            return false;
        }

    }
}