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

                data.Add(new BlockData(
                    new Vector3(voxels[i].X, voxels[i].Y, voxels[i].Z),
                    input.Palette[voxels[i].ColorIndex - 1]));
            }

            // ReSharper disable once PossibleLossOfFraction
            var pos = new Vector3(-input.SizeX / 2, -input.SizeY / 2, -input.SizeZ / 2);
            return new ChunkContent(pos, input.SizeX, input.SizeY, input.SizeZ, data.ToArray());
        }
    }
}