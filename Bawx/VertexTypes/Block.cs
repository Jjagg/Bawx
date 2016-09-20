using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bawx.VertexTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Block : IVertexType
    {
        public byte X;
        public byte Y;
        public byte Z;
        public byte Index;

        public static readonly VertexDeclaration VertexDeclaration;
        public bool IsEmpty => Index == 0;

        public Block(byte x, byte y, byte z, byte index)
        {
            X = x;
            Y = y;
            Z = z;
            Index = index;
        }

        public Block(byte[] bytes)
        {
            X = bytes[0];
            Y = bytes[1];
            Z = bytes[2];
            Index = bytes[3];
        }

        public Block(uint packedValue) : this(
                (byte) (packedValue & 0xFF),
                (byte) ((packedValue >> 8) & 0xFF),
                (byte) ((packedValue >> 16) & 0xFF),
                (byte) ((packedValue >> 24) & 0xFF)) {
        }

        public Vector3 Position
        {
            get { return new Vector3(X, Y, Z); }
            set
            {
                X = (byte) value.X;
                Y = (byte) value.Y;
                Z = (byte) value.Z;
            }
        }


        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;

        public override string ToString()
        {
            return $"Position: ({X}, {Y}, {Z}), Index: {Index}";
        }

        public bool Equals(Block other)
        {
            return X == other.X && Y == other.Y && Z == other.Z && Index == other.Index;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Block && Equals((Block) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode*397) ^ Y.GetHashCode();
                hashCode = (hashCode*397) ^ Z.GetHashCode();
                hashCode = (hashCode*397) ^ Index.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Block a, Block b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Block a, Block b)
        {
            return !(a == b);
        }

        static Block()
        {
            var elements = new [] 
            { 
                new VertexElement(0, VertexElementFormat.Byte4, VertexElementUsage.Position, 1) 
            };
            VertexDeclaration = new VertexDeclaration(elements);
        }
    }
}