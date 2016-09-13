using System.Collections.Generic;

namespace Bawx
{
    public class ChunkCollection
    {
        public readonly List<Chunk> Chunks;

        public ChunkCollection(List<Chunk> chunks)
        {
            Chunks = chunks;
        }

        public void Draw()
        {
            foreach (var chunk in Chunks)
                chunk.Draw();
        }
    }
}