using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bawx.Rendering
{
    public abstract class ChunkRenderer : IDisposable
    {
        private int _currentIndex;

        protected readonly GraphicsDevice GraphicsDevice;
        protected Chunk Chunk;

        protected static int BlockDataSize = Marshal.SizeOf(typeof(BlockData));

        /// <summary>
        /// The number of active blocks.
        /// </summary>
        public int BlockCount => _currentIndex;

        /// <summary>
        /// The effect used for rendering.
        /// </summary>
        public readonly VoxelEffect Effect;

        /// <summary>
        /// The number of blocks that can be added to the buffer without rebuilding.
        /// </summary>
        public abstract int FreeBlocks { get; }

        /// <summary>
        /// True if this renderers buffer is full. This mean you have to rebuild the buffer to add blocks.
        /// </summary>
        public bool BufferFull => FreeBlocks == 0;

        #region Initialization

        protected ChunkRenderer(GraphicsDevice graphicsDevice, Vector4[] palette)
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException(nameof(graphicsDevice));
            if (graphicsDevice.GraphicsProfile != GraphicsProfile.HiDef)
                throw new ArgumentException("GraphicsDevice should have the HiDef profile!");
            if (palette == null)
                throw new ArgumentNullException(nameof(palette));
                
            GraphicsDevice = graphicsDevice;
            Effect = new VoxelEffect(graphicsDevice);
            Effect.Palette = palette;
        }

        /// <summary>
        /// True if <see cref="Assign"/> has been called on this renderer.
        /// </summary>
        public bool Assigned => Chunk != null;

        /// <summary>
        /// Assign this renderer to a chunk. A renderer is specific to one chunk, so this can only be called once.
        /// </summary>
        /// <param name="chunk">The chunk that this renderer should render.</param>
        public void Assign(Chunk chunk)
        {
            if (Assigned)
                throw new InvalidOperationException("This renderer is already assigned to a chunk!");

            Chunk = chunk;
        }

        /// <summary>
        /// True if <see cref="Initialize"/> has been called on this renderer.
        /// </summary>
        public bool Initialized { get; private set; }

        /// <summary>
        /// Initialize this renderer for the given chunk with the given block data.
        /// </summary>
        /// <param name="blockData"></param>
        /// <param name="maxBlocks"></param>
        public void Initialize(BlockData[] blockData, int? maxBlocks = null)
        {
            if (!Assigned)
                throw new InvalidOperationException("Renderer must be assigned to a chunk before calling Initialize!");

            if (Initialized)
                Dispose();

            InitializeInternal(blockData, maxBlocks ?? blockData.Length);

            _currentIndex += blockData.Length;

            Initialized = true;
        }

        protected abstract void InitializeInternal(BlockData[] blockData, int maxBlocks);

        #endregion Initialization

        #region Modification

        /// <summary>
        /// Set the block at the given index.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="index"></param>
        public abstract void SetBlock(BlockData block, int index);

        /// <summary>
        /// Add a block with the given data and returns the index of the created block.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the buffer is full and the <see cref="rebuildIfNeeded"/> flag is not set.</exception>
        /// <param name="block">Data of the block to add.</param>
        /// <param name="rebuildIfNeeded">
        ///   If true, this renderer will rebuild its buffer if it's full before adding the block. 
        ///   Fulness can be checked with <see cref="BufferFull"/> and <see cref="FreeBlocks"/>.
        /// </param>
        /// <returns>The index of the added block.</returns>
        public int AddBlock(BlockData block, bool rebuildIfNeeded = false)
        {
            // TODO overwrite inactive blocks if any exist.
            if (FreeBlocks == 0)
            {
                if (rebuildIfNeeded)
                    RebuildInternal(BlockCount + 1);
                else
                    throw new InvalidOperationException("The buffer cannot hold any more blocks, rebuild the buffer");
            }
            var index = _currentIndex;
            SetBlock(block, index);
            _currentIndex++;

            return index;
        }

        /// <summary>
        /// Remove the block at the given index. Makes the block inactive until a block is added to overwrite it. 
        /// To remove all inactive blocks and rebuild the buffer use <see cref="Rebuild"/>
        /// </summary>
        /// <param name="index">The index of the block to remove.</param>
        public abstract void RemoveBlock(int index);

        /// <summary>
        /// Rebuild the buffer to remove any empty blocks
        /// </summary>
        /// <param name="maxBlocks">The number of blocks that the buffer should be able to hold. If left at null maxBlocks will be set to <see cref="BlockCount"/>.</param>
        public void Rebuild(int? maxBlocks)
        {
            RebuildInternal(maxBlocks ?? BlockCount);
        }

        protected abstract void RebuildInternal(int maxBlocks);

        #endregion

        #region Rendering

        protected virtual void PreDraw()
        {
        }

        public void Draw()
        {
            PreDraw();

            foreach (var pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                DrawInternal();
            }
        }

        protected abstract void DrawInternal();

        #endregion

        #region IDisposable

        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Ensures this voxel buffer is disposed.
        /// </summary>
        ~ChunkRenderer()
        {
            Dispose(false);
            IsDisposed = true;
        }

        /// <summary>
        /// Disposes of this voxel buffer
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            IsDisposed = true;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Don't call this method directly! Use <see cref="Dispose"/>.
        /// </summary>
        protected abstract void Dispose(bool disposing);

        #endregion

    }
}