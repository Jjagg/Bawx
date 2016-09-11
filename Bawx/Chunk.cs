using System;
using Bawx.Rendering;
using Microsoft.Xna.Framework;

namespace Bawx
{
    public sealed class Chunk
    {
        public const int DefaultSize = 32;

        public ChunkRenderer Renderer { get; }

        private Vector3 _position;
        public Vector3 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                Renderer.Effect.ChunkPosition = value;
            }
        }

        public readonly int SizeX;
        public readonly int SizeY;
        public readonly int SizeZ;

        public readonly int TotalSize;

        public int BlockCount => Renderer.BlockCount;

        public Vector3 Center => Position + new Vector3(SizeX/2, SizeY/2, SizeZ/2);

        public Chunk(ChunkRenderer renderer, Vector3 position, 
            int sizeX = DefaultSize, int sizeY = DefaultSize, int sizeZ = DefaultSize)
        {
            Renderer = renderer;
            Position = position;
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
            TotalSize = sizeX*sizeY*sizeZ;

            Renderer.Assign(this);
        }

        public void SetBlockData(int index, BlockData data)
        {
            if (!Renderer.Initialized)
                throw new InvalidOperationException("Renderer must be initialized with <see cref='BuildChunk' /> first.");

            Renderer.SetBlock(data, index);
        }

        /// <summary>
        /// Add a single block from the given block data to this chunk. Do not use this when building a chunk, use <see cref='BuildChunk'/> instead.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="rebuildIfNeeded"></param>
        /// <returns></returns>
        public void AddBlock(BlockData data, bool rebuildIfNeeded = false)
        {
            if (BlockCount >= TotalSize)
                throw new InvalidOperationException("Chunk is full.");

            Renderer.AddBlock(data, rebuildIfNeeded);
            // TODO store the blocks in a more manageable format
        }

        public void BuildChunk(BlockData[] data, bool rebuild = false)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (data.Length > TotalSize)
                throw new ArgumentException("Too much data for this chunk.", nameof(data));
            if (Renderer.Initialized && !rebuild)
                throw new InvalidOperationException("Chunk is already built, to override set the rebuild flag.");

            Renderer.Initialize(data);
            // TODO store the blocks in a more manageable format (octree probably)
        }

        public void Draw()
        {
            Renderer.Draw();
        }
    }
}