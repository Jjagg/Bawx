using System;
using System.Linq;
using System.Runtime.InteropServices;
using Bawx.Util;
using Microsoft.Xna.Framework.Graphics;

namespace Bawx
{
    /// <summary>
    /// A buffer for rendering a group of blocks (a chunk)
    /// using instanced primitive rendering.
    /// </summary>
    public sealed class VoxelBuffer<TInstanceVertexType> : IDisposable where TInstanceVertexType : struct, IVertexType
    {
        private readonly GraphicsDevice _graphicsDevice;

        private readonly VertexBufferBinding[] _bufferBindings;

        public bool IsDisposed => VertexBuffer?.IsDisposed ?? true;

        /// <summary>
        /// Checks to see if this buffer has data.
        /// </summary>
        public bool HasData => VertexBuffer != null;

        /// <summary>
        /// Gets the underlying vertex buffer.
        /// </summary>
        public VertexBuffer VertexBuffer { get; private set; }

        public bool Dynamic { get; private set; }

        /// <summary>
        /// Creates a new synchronized voxel buffer.
        /// </summary>
        public VoxelBuffer(GraphicsDevice gd)
        {
            _graphicsDevice = gd;

            _bufferBindings = new VertexBufferBinding[2];
            _bufferBindings[0] = new VertexBufferBinding(GetNormalUnitCube(gd), 0, 0);
        }

        /// <summary>
        /// Ensures this voxel buffer is disposed.
        /// </summary>
        ~VoxelBuffer()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes of this voxel buffer
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Create a vertex buffer. Dispose of the existing vertex buffer if it exists.
        /// </summary>
        /// <param name="data">The data to set.</param>
        /// <param name="dynamic">If set to true, the created vertex buffer will be dynamic.</param>
        public void Create(TInstanceVertexType[] data, bool dynamic = false)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (!data.Any()) throw new ArgumentException("Data should not be empty", nameof(data));

            Dynamic = dynamic;

            VertexBuffer?.Dispose();

            VertexBuffer = dynamic
                ? new DynamicVertexBuffer(_graphicsDevice, data[0].VertexDeclaration, data.Length, BufferUsage.WriteOnly)
                : new VertexBuffer(_graphicsDevice, data[0].VertexDeclaration, data.Length, BufferUsage.WriteOnly);
            VertexBuffer.SetData(data, 0, data.Length);

            _bufferBindings[1] = new VertexBufferBinding(VertexBuffer, 0, 1);
        }

        // TODO document!!
        public void SetData(TInstanceVertexType[] src, int srcIndex, int dstIndex, int length)
        {
            SetData(src, srcIndex, dstIndex, length, Marshal.SizeOf(typeof(TInstanceVertexType)));
        }

        // TODO document!!
        public void SetData(TInstanceVertexType[] src, int srcIndex, int dstIndex, int length, int stride)
        {
            VertexBuffer.SetData(stride * srcIndex, src, dstIndex, length, stride);
        }

        /// <summary>
        /// Draws this voxel buffer, assuming that an effect is currently active.
        /// </summary>
        public void Draw()
        {
            if (VertexBuffer == null)
                return;

            _graphicsDevice.SetVertexBuffers(_bufferBindings);
            _graphicsDevice.Indices = GetCubeIndexBuffer(_graphicsDevice);
            _graphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, 0, 0, 
                12, VertexBuffer.VertexCount);
        }

        /// <summary>
        /// Disposes of this voxel buffer.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (VertexBuffer != null)
            {
                VertexBuffer.Dispose();
                VertexBuffer = null;
            }
        }

        private static VertexBuffer _normalColorUnitCube;
        private static VertexBuffer _normalUnitCube;
        private static IndexBuffer _unitCubeIndexBuffer;

        private static VertexBuffer GetNormalUnitCube(GraphicsDevice device)
        {
            if (_normalUnitCube == null)
            {
                _normalUnitCube = new VertexBuffer(device, VertexPositionNormal.VertexDeclaration, 6*4,
                    BufferUsage.WriteOnly);
                _normalUnitCube.SetData(CubeBuilder.GetNormal());
            }

            return _normalUnitCube;
        }

        private static IndexBuffer GetCubeIndexBuffer(GraphicsDevice device)
        {
            if (_unitCubeIndexBuffer == null)
            {
                _unitCubeIndexBuffer = new IndexBuffer(device, IndexElementSize.SixteenBits, 6*2*3, BufferUsage.None);
                _unitCubeIndexBuffer.SetData(CubeBuilder.GetShortIndices());
            }

            return _unitCubeIndexBuffer;
        }
    }
}