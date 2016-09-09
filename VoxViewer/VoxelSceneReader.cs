using Bawx;
using Microsoft.Xna.Framework.Content;

namespace VoxViewer
{
    public class VoxelSceneReader : ContentTypeReader<VoxelScene>
    {
        protected override VoxelScene Read(ContentReader input, VoxelScene existingInstance)
        {
            var result = new VoxelScene();

            var count = input.ReadInt32();
            result.Voxels = new VoxelData[count];
            result.SizeX = input.ReadInt32();
            result.SizeY = input.ReadInt32();
            result.SizeZ = input.ReadInt32();

            for (var i = 0; i < count; i++)
            {
                result.Voxels[i] = new VoxelData();
                result.Voxels[i].X = input.ReadByte();
                result.Voxels[i].Y = input.ReadByte();
                result.Voxels[i].Z = input.ReadByte();
                result.Voxels[i].Color = input.ReadColor();
            }

            return result;
        }
    }
}