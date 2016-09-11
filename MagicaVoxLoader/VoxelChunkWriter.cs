using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace MagicaVoxLoader
{
    [ContentTypeWriter]
    public class VoxelChunkWriter : ContentTypeWriter<ChunkContent>
    {
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "Bawx.VoxelChunkReader, Bawx";
        }

        protected override void Write(ContentWriter output, ChunkContent value)
        {
            output.Write(value.SizeX);
            output.Write(value.SizeY);
            output.Write(value.SizeZ);
            output.Write(value.Position);
            var count = value.Blocks.Length;
            output.Write(count);

            for (var i = 0; i < count; i++)
            {
                var b = value.Blocks[i];
                output.Write(b.PackedValue);
            }

            // write the palette
            foreach (var c in value.Palette)
                output.Write(c);
        }
    }
}