using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bawx.Rendering.Lighting
{
    public class DirectionalShadowMap
    {

        public readonly RenderTarget2D RenderTarget;
        private readonly GraphicsDevice _graphicsDevice;

        private static readonly BlendState BlendState = BlendState.Opaque;

        public DirectionalShadowMap(GraphicsDevice graphicsDevice)
            : this(
                  graphicsDevice, 
                  graphicsDevice.PresentationParameters.BackBufferWidth, 
                  graphicsDevice.PresentationParameters.BackBufferHeight)
        {
        }

        public DirectionalShadowMap(GraphicsDevice graphicsDevice, int width, int height)
        {
            RenderTarget = new RenderTarget2D(graphicsDevice, width, height, false, SurfaceFormat.Single, DepthFormat.Depth24);
            _graphicsDevice = graphicsDevice;
        }

        /// <summary>
        /// Clear <see cref="RenderTarget"/> and set the graphicsdevice state. Call this before
        /// drawing the occluders with an effect with a shadow mapping technique.
        /// </summary>
        public void Prepare()
        {
            _graphicsDevice.SetRenderTarget(RenderTarget);
            _graphicsDevice.Clear(Color.White);
            _graphicsDevice.BlendState = BlendState;
            _graphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
    }
}