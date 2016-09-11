using System.Collections.Generic;
using Bawx;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace MagicaVoxLoader
{
    [ContentProcessor(DisplayName = "Vox Processor - Bawx.ChunkContent")]
    public sealed class VoxelChunkProcessor : ContentProcessor<VoxContent, ChunkContent>
    {
        public override ChunkContent Process(VoxContent input, ContentProcessorContext context)
        {
            var data = new List<BlockData>();

            var voxels = input.Voxels;

            for (var i = 0; i < input.Voxels.Length; i++)
            {
                if (voxels[i].IsEmpty)
                    continue;

                // We reduce color index by one so the index matches with our palette array
                data.Add(new BlockData(voxels[i].X, voxels[i].Y, voxels[i].Z, (byte) (voxels[i].ColorIndex - 1)));
            }

            return new ChunkContent(Vector3.Zero, input.SizeX, input.SizeY, input.SizeZ, data.ToArray(), input.Palette);
        }
    }
}