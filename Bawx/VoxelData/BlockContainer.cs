using System.Collections.Generic;
using Bawx.VertexTypes;
using Microsoft.Xna.Framework;

namespace Bawx.VoxelData
{
    public abstract class BlockContainer
    {
        public abstract BoundingBox Bounds { get; }

        /// <summary>
        /// Get the block at {x, y, z} or <see cref="BlockData.Empty"/> if no block is present.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public BlockData this[byte x, byte y, byte z] => Get(x, y, z);

        /// <summary>
        /// Get the block at {x, y, z} or <see cref="BlockData.Empty"/> if no block is present.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public abstract BlockData Get(byte x, byte y, byte z);

        /// <summary>
        /// Adds a collection of blocks.
        /// </summary>
        /// <param name="blocks">The blocks to add.</param>
        public abstract void AddRange(List<Block> blocks);

        /// <summary>
        /// Add a block.
        /// </summary>
        /// <param name="block">The block to add.</param>
        public void Add(Block block)
        {
            Add(block.X, block.Y, block.Z, new BlockData(block.Index));
        }

        /// <summary>
        /// Add a block.
        /// </summary>
        /// <param name="x">X-coordinate of the block in the chunk.</param>
        /// <param name="y">Y-coordinate of the block in the chunk.</param>
        /// <param name="z">Z-coordinate of the block in the chunk.</param>
        /// <param name="block">Material data for the block.</param>
        public abstract void Add(byte x, byte y, byte z, BlockData block);

        /// <summary>
        /// Remove the given block. Note that this only takes the position of the 
        /// given block and ignores anything else.
        /// </summary>
        /// <param name="block"></param>
        public void Remove(Block block)
        {
            Remove(block.X, block.Y, block.Z);
        }

        /// <summary>
        /// Remove the block at the given coordinates within this container.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public abstract void Remove(byte x, byte y, byte z);

        public abstract void Clear();
        public abstract bool Intersect(Ray ray, out BlockIntersection intersection);
    }
}