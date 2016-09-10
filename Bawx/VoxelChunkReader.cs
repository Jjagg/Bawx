using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bawx
{
    public class VoxelChunkReader : ContentTypeReader<Chunk>
    {
        protected override Chunk Read(ContentReader reader, Chunk existingInstance)
        {
            var gds = (IGraphicsDeviceService) reader.ContentManager.ServiceProvider.GetService(typeof(IGraphicsDeviceService));
            var gd = gds.GraphicsDevice;

            var x = reader.ReadInt32();
            var y = reader.ReadInt32();
            var z = reader.ReadInt32();
            var pos = reader.ReadVector3();

            var chunk = new Chunk(gd, new VoxelEffect(gd), pos, x, y, z);

            var count = reader.ReadInt32();
            var blockData = new BlockData[count];

            for (var i = 0; i < count; i++)
            {
                var b = new BlockData();
                b.Position = reader.ReadVector3();
                b.Color = reader.ReadColor();

                blockData[i] = b;
            }

            chunk.BuildChunk(blockData);
            return chunk;
        }

    }
}