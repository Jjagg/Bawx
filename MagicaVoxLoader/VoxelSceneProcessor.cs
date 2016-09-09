using Bawx;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace MagicaVoxLoader
{
    [ContentProcessor(DisplayName = "Vox Processor - Bawx.VoxelScene")]
    public sealed class VoxelSceneProcessor : ContentProcessor<VoxContent, VoxelScene>
    {
        public override VoxelScene Process(VoxContent input, ContentProcessorContext context)
        {
            var voxels = new VoxelData[input.VoxelCount];

            for (var i = 0; i < input.Voxels.Length; i++)
            {
                voxels[i] = new VoxelData
                {
                    X = input.Voxels[i].X,
                    Y = input.Voxels[i].Y,
                    Z = input.Voxels[i].Z,
                    Color = input.Palette[input.Voxels[i].ColorIndex - 1]
                };
            }

            return new VoxelScene
            {
                SizeX = input.SizeX,
                SizeY = input.SizeY,
                SizeZ = input.SizeZ,
                Voxels = voxels
            };
        }
    }
}