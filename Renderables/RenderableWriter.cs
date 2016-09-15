using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Graphics;

namespace Renderables
{
    [ContentTypeWriter]
    public class RenderableWriter : ContentTypeWriter<RenderableContent>
    {
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "Renderables.RenderableReader, Renderables";
        }

        protected override void Write(ContentWriter output, RenderableContent value)
        {
            output.Write(value.VertexTypeName);
            output.Write(value.VertexCount);
            output.Write(value.IndexCount);
            output.WriteRawObject(IndexElementSize.ThirtyTwoBits);
            output.WriteRawObject(value.PrimitiveType);
            output.Write(value.PrimitiveCount);

            foreach (var b in value.VertexData)
                output.Write(b);

            foreach (var ind in value.Indices)
                output.Write(ind);
        }
    }
}