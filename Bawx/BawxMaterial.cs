using Microsoft.Xna.Framework;

namespace Bawx
{
    public struct BawxMaterial
    {

        public Color Color;

        // Default
        public static readonly BawxMaterial Default;
        static BawxMaterial()
        {
            Default = new BawxMaterial
            {
                Color = Color.White,
            };
        }
    }
}