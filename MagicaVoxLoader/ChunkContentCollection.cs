using System.Collections.Generic;

namespace MagicaVoxLoader
{
    public class ChunkContentCollection
    {
        public readonly List<ChunkContent> Chunks = new List<ChunkContent>();

        public void Add(ChunkContent chunk)
        {
            Chunks.Add(chunk);
        }

        public ChunkContent this[int i]
        {
            get { return Chunks[i]; }
        } 
    }
}