using Microsoft.Xna.Framework;

namespace Bawx.VoxelData
{
    internal interface IBounded
    {
        BoundingBox Bounds { get; }
    }
}