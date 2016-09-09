using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bawx
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BlockData : IVertexType
    {
        // TODO try to use bytes to save lots of data transfer! Maybe need to pack the bytes though
        public Vector3 Position;
        public Color Color;
        public static readonly VertexDeclaration VertexDeclaration;

        public BlockData(Vector3 position, Color color)
        {
            Position = position;
            Color = color;
        }

        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;

        public override string ToString()
        {
            return "{{Position:" + Position + " Color:" + Color + "}}";
        }

        public bool Equals(BlockData other)
        {
            return Position.Equals(other.Position) && Color.Equals(other.Color);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is BlockData && Equals((BlockData) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Position.GetHashCode();
                hashCode = (hashCode*397) ^ Color.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(BlockData left, BlockData right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BlockData left, BlockData right)
        {
            return !(left == right);
        }

        static BlockData()
        {
            var elements = new [] 
            { 
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0), 
                new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0), 
            };
            VertexDeclaration = new VertexDeclaration(elements);
        }
    }
}