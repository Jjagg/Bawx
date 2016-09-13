using Microsoft.Xna.Framework;

namespace Bawx
{
    public class Camera
    {
        public Matrix View;
        public Matrix Projection;

        public BoundingFrustum BoundingFrustrum => new BoundingFrustum(View*Projection);
    }
}