using Bawx.Rendering;
using Microsoft.Xna.Framework;
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

            var sizeX = reader.ReadInt32();
            var sizeY = reader.ReadInt32();
            var sizeZ = reader.ReadInt32();
            var pos = reader.ReadVector3();

            var count = reader.ReadInt32();
            var activeCount = reader.ReadInt32();
            var blockData = new BlockData[count];

            for (var i = 0; i < count; i++)
            {
                var x = reader.ReadByte();
                var y = reader.ReadByte();
                var z = reader.ReadByte();
                var index = reader.ReadByte();
                blockData[i] = new BlockData(x, y, z, index);
            }

            var palette = new Vector4[255];
            for (var i = 0; i < 255; i++)
                palette[i] = reader.ReadColor().ToVector4();

            var renderer = new InstancedChunkRenderer(gd, palette);
            var chunk = new Chunk(renderer, pos, sizeX, sizeY, sizeZ);

            chunk.BuildChunk(blockData, activeCount);
            return chunk;
        }

    }
}