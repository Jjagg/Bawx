using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Bawx.Util
{
    public class GraphicsStateStack
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly Stack<GraphicsState> _states;

        public GraphicsStateStack(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _states = new Stack<GraphicsState>();
        }

        /// <summary>
        /// Pushes the current graphics state to an internal stack.
        /// </summary>
        public void Push()
        {
            lock (_states)
            {
                _states.Push(new GraphicsState(_graphicsDevice));
            }
        }

        /// <summary>
        /// Removes the state from the top of the stack and restores it.
        /// </summary>
        public void Pop()
        {
            lock ( _states )
            {
                if (_states.Count <= 0) return;

                var state = _states.Pop();
                state.Restore(_graphicsDevice);
            }
        }

        /// <summary>
        /// Peeks at the last pushed state.
        /// </summary>
        /// <returns></returns>
        public GraphicsState Peek()
        {
            lock (_states)
            {
                return _states.Peek();
            }
        }

        public void Clear()
        {
            
        }

        /// <summary>
        /// Contains a way to save and restore the state of the graphics device.
        /// </summary>
        public sealed class GraphicsState
        {
            /// <summary>
            /// Gets the blend state.
            /// </summary>
            public BlendState BlendState { get; }

            /// <summary>
            /// Gets the depth-stencil state.
            /// </summary>
            public DepthStencilState DepthStencilState { get; }

            /// <summary>
            /// Gets the rasterizer state.
            /// </summary>
            public RasterizerState RasterizerState { get; }

            /// <summary>
            /// Gets the number of sampler states stored.
            /// </summary>
            public int SamplerStateCount => SamplerStates.Length;

            /// <summary>
            /// Gets the sampler states.
            /// </summary>
            public SamplerState[] SamplerStates { get; }

            /// <summary>
            /// Creates a graphics state representing the current state of the graphics device.
            /// </summary>
            public GraphicsState(GraphicsDevice graphicsDevice, int samplerStateCount = 1)
            {
                if (graphicsDevice == null)
                    throw new ArgumentNullException(nameof(graphicsDevice));

                BlendState = graphicsDevice.BlendState;
                DepthStencilState = graphicsDevice.DepthStencilState;
                RasterizerState = graphicsDevice.RasterizerState;

                SamplerStates = new SamplerState[samplerStateCount];
                for (var i = 0; i < samplerStateCount; i++)
                    SamplerStates[i] = graphicsDevice.SamplerStates[i];
            }

            /// <summary>
            /// Restores this state's information to the given graphics device.
            /// </summary>
            internal void Restore(GraphicsDevice graphicsDevice)
            {
                if (graphicsDevice == null)
                    throw new ArgumentNullException(nameof(graphicsDevice));

                graphicsDevice.BlendState = BlendState;
                graphicsDevice.DepthStencilState = DepthStencilState;
                graphicsDevice.RasterizerState = RasterizerState;

                for (var i = 0; i < SamplerStateCount; i++)
                graphicsDevice.SamplerStates[i] = SamplerStates[i];
            }
        }

    }
}