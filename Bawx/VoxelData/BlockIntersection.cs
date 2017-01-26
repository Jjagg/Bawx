using Microsoft.Xna.Framework;

namespace Bawx.VoxelData
{
    public struct BlockIntersection
    {

        /// <summary>
        /// Data of the block that was hit.
        /// </summary>
        public readonly BlockData BlockData;

        /// <summary>
        /// UV coordinates on the block where the intersection happened.
        /// </summary>
        public readonly Vector2 Uv;

        public bool Empty => BlockData.Index == 0;

        public BlockIntersection(BlockData blockData, Vector2 uv)
        {
            Uv = uv;
            BlockData = blockData;
        }
    }
}