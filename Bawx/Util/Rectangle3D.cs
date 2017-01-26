using Microsoft.Xna.Framework;

namespace Bawx.Util
{
    public struct Rectangle3D
    {
        public readonly Transformation Transformation;

        public Rectangle3D(Matrix matrix) : this(new Transformation(matrix))
        {
        }

        public Rectangle3D(Transformation transformation)
        {
            Transformation = transformation;
        }
    }
}