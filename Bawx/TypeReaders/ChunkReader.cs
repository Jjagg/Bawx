using Bawx.Rendering.ChunkRenderers;
using Bawx.VertexTypes;
using Bawx.VoxelData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bawx.TypeReaders
{
    public class ChunkReader : ContentTypeReader<Chunk>
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
            var blockData = new Block[count];

            for (var i = 0; i < count; i++)
            {
                var x = reader.ReadByte();
                var y = reader.ReadByte();
                var z = reader.ReadByte();
                var index = reader.ReadByte();
                blockData[i] = new Block(x, y, z, index);
            }

            var palette = new Vector4[255];
            for (var i = 0; i < 255; i++)
                palette[i] = reader.ReadColor().ToVector4();

            var renderer = new PolygonChunkRenderer(gd, palette);
            var chunk = new Chunk(renderer, pos, (byte) sizeX, (byte) sizeY, (byte) sizeZ);

            chunk.BuildChunk(blockData, activeCount);
            return chunk;
        }

    }
}