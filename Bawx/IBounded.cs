using Microsoft.Xna.Framework;

namespace Bawx
{
    internal interface IBounded
    {
        BoundingBox Bounds { get; }
    }
}