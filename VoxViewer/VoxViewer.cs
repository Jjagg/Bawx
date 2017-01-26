using System;
using System.Collections.Generic;
using Bawx.Rendering.Effects;
using Bawx.Rendering.Lighting;
using Bawx.Util;
using Bawx.VoxelData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VoxViewerDx;

namespace VoxViewer
{
    internal partial class VoxViewer : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private DirectionalShadowMap _shadowMap;

        private SpriteBatch _spriteBatch;
        private Texture2D _background;

        private SpriteFont _font;

        private GraphicsStateStack _deviceState;

        public VoxViewerState State;

        private Chunk CurrentChunk => State.CurrentChunk;
        private VoxelEffect Effect => CurrentChunk.Renderer.Effect;

        private GraphicsMetrics _drawMetrics;

        public VoxViewer()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            IsFixedTimeStep = false;
            _graphics.SynchronizeWithVerticalRetrace = false;
        }

        protected override void Initialize()
        {
            Components.Add(new Input(this));
            base.Initialize();
        }

        private static float Remap(float val, float min1, float max1, float min2, float max2)
        {
            return (val - min1) / (max1 - min1) * (max2 - min2) + min2;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _deviceState = new GraphicsStateStack(GraphicsDevice);

            _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);
            _font = Content.Load<SpriteFont>("font");

            // nice gradient background
            const int resolution = 30;
            _background = new Texture2D(_graphics.GraphicsDevice, 1, resolution);
            var gradient = new Color[resolution];
            for (var i = 0; i < resolution; i++)
            {
                var val = Remap((float)i/resolution, 0f, 1f, 0.3f, 0.75f);
                gradient[i] = new Color(val, val, val, 1f);
            }
            _background.SetData(gradient);

            var modelNames = new List<string>();
            modelNames.Add("3x3x3");
            modelNames.Add("8x8x8");
            modelNames.Add("castle");
            modelNames.Add("chr_knight");
            modelNames.Add("chr_old");
            modelNames.Add("chr_rain");
            modelNames.Add("chr_sword");
            modelNames.Add("doom");
            modelNames.Add("ephtracy");
            modelNames.Add("menger");
            modelNames.Add("monu1");
            modelNames.Add("monu9");
            modelNames.Add("nature");
            modelNames.Add("shelf");
            modelNames.Add("teapot");

            var modelChunks = new List<Chunk>();
            foreach (var model in modelNames)
            {
                var chunk = Content.Load<Chunk>(model); 
                modelChunks.Add(chunk);
                chunk.Position = -chunk.Center;
            }

            _shadowMap = new DirectionalShadowMap(GraphicsDevice);

            State = new VoxViewerState(this, modelNames, modelChunks);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            HandleInput();

            State.Update(gameTime);

            Effect.View = State.ViewMatrix;
            Effect.Projection = State.ProjectionMatrix;
        }

        protected override void Draw(GameTime gameTime)
        {
            if (State.RenderState == RenderState.ShadowMap)
                UpdateShadowMap();

            GraphicsDevice.Clear(Color.LightGray);
            _deviceState.Push();
            RenderBackGround();
            _deviceState.Pop();

            _drawMetrics = GraphicsDevice.Metrics;

            switch (State.RenderState)
            {
                case RenderState.ShadowMap:
                    RenderModel();

                    RenderShadowMap(new Rectangle(1920-320, 0, 320, 180));
                    break;
                case RenderState.Simple:
                    RenderModelNoShadow();
                    break;
                case RenderState.Depth:
                    RenderModel();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _drawMetrics = GraphicsDevice.Metrics - _drawMetrics;

            _deviceState.Push();

            RenderUI();

            // Render components
            base.Draw(gameTime);
            _deviceState.Pop();
        }

        private void UpdateShadowMap()
        {
            _deviceState.Push();

            _shadowMap.Prepare();
            State.CurrentChunk.Draw();

            _deviceState.Pop();

            GraphicsDevice.SetRenderTarget(null);
        }

        private void RenderShadowMap(Rectangle? rect = null)
        {
            _deviceState.Push();

            _spriteBatch.Begin();
            if (rect == null)
                _spriteBatch.Draw(_shadowMap.RenderTarget, Vector2.Zero);
            else
                _spriteBatch.Draw(_shadowMap.RenderTarget, null, rect);

            _spriteBatch.End();

            _deviceState.Pop();
        }

        private void RenderModel()
        {
            _deviceState.Push();

            GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            Effect.ShadowMap = _shadowMap.RenderTarget;
            CurrentChunk.Draw();

            _deviceState.Pop();
        }

        private void RenderModelNoShadow()
        {
            _deviceState.Push();

            CurrentChunk.Draw();

            _deviceState.Pop();
        }

        private void RenderBackGround()
        {
            _deviceState.Push();

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(_background, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
            _spriteBatch.End();
            _deviceState.Pop();

            _deviceState.Pop();
        }

        private void RenderUI()
        {
            _deviceState.Push();

            _spriteBatch.Begin();

            var h = GraphicsDevice.PresentationParameters.BackBufferHeight;
            var w = GraphicsDevice.PresentationParameters.BackBufferWidth;

            var primitivesStr = $"Total primitives: {_drawMetrics.PrimitiveCount}";
            RenderString(primitivesStr, new Vector2(6f, h - 121));

            var modelIndexStr = $"Index: {State.ChunkIndex}";
            RenderString(modelIndexStr, new Vector2(6f, h - 96));

            var modelStr = $"Model: {State.CurrentModelName}";
            RenderString(modelStr, new Vector2(6f, h - 71));

            var voxelsStr = $"Voxels: {CurrentChunk.BlockCount}";
            RenderString(voxelsStr, new Vector2(6f, h - 46));

            var sizeStr = $"Size: {CurrentChunk.SizeX} x {CurrentChunk.SizeY} x {CurrentChunk.SizeZ}";
            RenderString(sizeStr, new Vector2(6f, h - 21));

            var stateStr = $"RenderState: {State.RenderState}";
            RenderString(stateStr, new Vector2(w - 350, h - 21));

            _spriteBatch.End();

            _deviceState.Pop();

        }

        private void RenderString(string str, Vector2 position)
        {
            _spriteBatch.DrawString(_font, str, position, Color.Black);
            _spriteBatch.DrawString(_font, str, position - Vector2.One, Color.White);
        }

        private void HandleInput()
        {
            if (Input.IsPressed(Keys.Escape)) Exit();
        }

        private static readonly int RenderStateCount = Enum.GetValues(typeof(RenderState)).Length;

        public enum RenderState
        {
            Simple = 0,
            ShadowMap = 1,
            // todo broken because of light direction hack to get shadow map working... need to fix this
            Depth = 2
        }

        public enum CameraState
        {
            Static = 0,
            Rotating = 1,
            Front = 2,
            Back = 3,
        }
    }
}
