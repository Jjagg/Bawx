using Microsoft.Xna.Framework.Graphics;

namespace Renderables
{
    public abstract class RenderableContent
    {
        public RenderableContent(string vertexTypeName, int vertexCount, int indexCount, 
            PrimitiveType primitiveType, int primitiveCount, byte[] vertexData)
        {
            VertexTypeName = vertexTypeName;
            VertexCount = vertexCount;
            IndexCount = indexCount;
            PrimitiveType = primitiveType;
            PrimitiveCount = primitiveCount;
            VertexData = vertexData;
        }

        public string VertexTypeName { get; }
        internal int VertexCount { get; }
        public int IndexCount { get; }
        public PrimitiveType PrimitiveType { get; }
        public int PrimitiveCount { get; }
        public byte[] VertexData { get; }
        public int[] Indices { get; set; }
    }
}