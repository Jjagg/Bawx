using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Renderables
{
    public class RenderableReader : ContentTypeReader<Renderable>
    {
        protected override Renderable Read(ContentReader input, Renderable existingInstance)
        {
            var gd = ((IGraphicsDeviceService) input.ContentManager.ServiceProvider.GetService(typeof(IGraphicsDeviceService))).GraphicsDevice;

            var vertexTypeName = input.ReadString();
            var vertexCount = input.ReadInt32();
            var indexCount = input.ReadInt32();
            var indexSize = input.ReadRawObject<IndexElementSize>();
            var primitiveType = input.ReadRawObject<PrimitiveType>();
            var primitiveCount = input.ReadInt32();

            var vertexType = Type.GetType(vertexTypeName);
            if (vertexType == null)
                throw new ContentLoadException($"Could not find vertex type {vertexTypeName}");

            // Vertex Buffer
            var vertexByteCount = Marshal.SizeOf(vertexType)*vertexCount;
            var vertexData = new byte[vertexByteCount];
            for (var i = 0; i < vertexByteCount; i++)
                vertexData[i] = input.ReadByte();

            var vertBuffer = new VertexBuffer(gd, vertexType, vertexCount, BufferUsage.WriteOnly);
            vertBuffer.SetData(vertexData);

            // Index Buffer
            var indBuffer = new IndexBuffer(gd, indexSize, indexCount, BufferUsage.WriteOnly);
            if (indexSize == IndexElementSize.SixteenBits)
            {
                var indices = new short[indexCount];
                for (var i = 0; i < indexCount; i++)
                    indices[i] = input.ReadInt16();

                indBuffer.SetData(indices);
            }
            else
            {
                var indices = new int[indexCount];
                for (var i = 0; i < indexCount; i++)
                    indices[i] = input.ReadInt32();

                indBuffer.SetData(indices);
            }

            return new Renderable(vertBuffer, indBuffer, primitiveType, primitiveCount);
        }
    }
}