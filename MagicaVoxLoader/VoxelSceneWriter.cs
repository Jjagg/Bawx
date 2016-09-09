using Bawx;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace MagicaVoxLoader
{
    [ContentTypeWriter]
    public class VoxelSceneWriter : ContentTypeWriter<VoxelScene>
    {
        protected override void Write(ContentWriter output, VoxelScene value)
        {

            var voxels = value.Voxels;
            var count = value.Voxels.Length;
            output.Write(count);
            output.Write(value.SizeX);
            output.Write(value.SizeY);
            output.Write(value.SizeZ);


            for (var i = 0; i < count; i++)
            {
                var data = voxels[i];
                output.Write(data.X);
                output.Write(data.Y);
                output.Write(data.Z);
                output.Write(data.Color);
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "VoxViewer.VoxelSceneReader, VoxViewer";
        }
    }
}