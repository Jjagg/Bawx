using Microsoft.Xna.Framework;

namespace Bawx.Util
{
    public struct Transformation
    {
        public readonly Matrix Matrix;
        public readonly Matrix Inverse;

        public Transformation(Matrix matrix)
        {
            Matrix = matrix;
            Matrix.Invert(ref matrix, out Inverse);
        }

        public Transformation(Matrix matrix, Matrix inverse)
        {
            Matrix = matrix;
            Inverse = inverse;
        }

        #region Factory Methods

        public static Transformation CreateTranslation(Vector3 translation)
        {
            var m = Matrix.CreateTranslation(translation);
            var inv = Matrix.CreateTranslation(-translation);
            return new Transformation(m, inv);
        }

        public static Transformation CreateScale(Vector3 scale)
        {
            var m = Matrix.CreateScale(scale);
            var unscale = new Vector3(1f/scale.X, 1f/scale.Y, 1f/scale.Z);
            var inv = Matrix.CreateScale(unscale);
            return new Transformation(m, inv);
        }

        public static Transformation CreateRotationX(float radians)
        {
            var m = Matrix.CreateRotationX(radians);
            var inv = Matrix.CreateRotationX(-radians);
            return new Transformation(m, inv);
        }

        public static Transformation CreateRotationY(float radians)
        {
            var m = Matrix.CreateRotationY(radians);
            var inv = Matrix.CreateRotationY(-radians);
            return new Transformation(m, inv);
        }

        public static Transformation CreateRotationZ(float radians)
        {
            var m = Matrix.CreateRotationZ(radians);
            var inv = Matrix.CreateRotationZ(-radians);
            return new Transformation(m, inv);
        }

        #endregion

    }
}