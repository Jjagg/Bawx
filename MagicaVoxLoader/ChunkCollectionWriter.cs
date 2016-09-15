using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace MagicaVoxLoader
{
    [ContentTypeWriter]
    public class ChunkCollectionWriter : ContentTypeWriter<ChunkContentCollection>
    {
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "Bawx.ChunkCollectionReader, Bawx";
        }

        protected override void Write(ContentWriter output, ChunkContentCollection value)
        {
            var chunkWriter = new ChunkWriter();

            output.Write(value.Chunks.Count);

            foreach (var chunk in value.Chunks)
                output.WriteObject(chunk, chunkWriter);
        }
    }
}