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
        /// Entry point of the ray.
        /// </summary>
        public readonly Vector3 Entry;
        
        /// <summary>
        /// Exit point of the ray.
        /// </summary>
        public readonly Vector3 Exit;

        public bool Empty => BlockData.Index == 0;

        public BlockIntersection(BlockData blockData, Vector3 entry, Vector3 exit)
        {
            BlockData = blockData;
            Entry = entry;
            Exit = exit;
        }
    }
}