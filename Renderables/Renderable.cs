using Microsoft.Xna.Framework.Graphics;

namespace Renderables
{
    public class Renderable
    {
        public VertexBuffer Vertices { get; }
        public IndexBuffer Indices { get; }
        public PrimitiveType Type { get; }
        public int PrimitiveCount { get; }

        public Renderable(VertexBuffer vertices, IndexBuffer indices, PrimitiveType type, int primitiveCount)
        {
            Vertices = vertices;
            Indices = indices;
            Type = type;
            PrimitiveCount = primitiveCount;
        }

        public void Render(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.SetVertexBuffer(Vertices);
            graphicsDevice.Indices = Indices;
            graphicsDevice.DrawIndexedPrimitives(Type, 0, 0, PrimitiveCount);
        }

    }
}