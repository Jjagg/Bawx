using System.Collections.Generic;
using Bawx;
using InputHelper;
using MGWheels.MiniUtils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VoxViewerDx
{
    internal class VoxViewer : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private RasterizerState _wireframeRasterizerState = new RasterizerState { CullMode = CullMode.CullClockwiseFace, FillMode = FillMode.WireFrame };

        private Matrix _viewMatrix;
        private Matrix _projectionMatrix;
        private Matrix _cameraRotationMatrix;

        private float _cameraAngle = 0;

        private SpriteBatch _spriteBatch;
        private Texture2D _background;

        private SpriteFont _font;

        private GraphicsStateStack _deviceState;
        private List<string> _modelNames;
        private List<Chunk> _modelChunks;
        private int _chunkIndex;
        private string CurrentModelName => _modelNames[_chunkIndex];
        private Chunk CurrentChunk => _modelChunks[_chunkIndex];

        public VoxViewer()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.HardwareModeSwitch = false;
            _graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";

            IsFixedTimeStep = false;
            _graphics.SynchronizeWithVerticalRetrace = false;
        }

        protected override void Initialize()
        {
            Components.Add(new FrameRateCounter(this));
            Components.Add(new Input(this));
            base.Initialize();
        }

        private static float Remap(float val, float min1, float max1, float min2, float max2)
        {
            return (val - min1) / (max1 - min1) * (max2 - min2) + min2;
        }

        protected override void LoadContent()
        {
            _deviceState = new GraphicsStateStack(GraphicsDevice);

            _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);
            _font = Content.Load<SpriteFont>("font");

            // nice gradient background
            const int resolution = 64;
            _background = new Texture2D(_graphics.GraphicsDevice, 1, resolution);
            var gradient = new Color[resolution];
            for (var i = 0; i < resolution; i++)
            {
                var val = Remap((float)i/resolution, 0f, 1f, 0.5f, 0.8f);
                gradient[i] = new Color(val, val, val, 1f);
            }
            _background.SetData(gradient);

            _modelNames = new List<string>();
            _modelNames.Add("3x3x3");
            _modelNames.Add("8x8x8");
            _modelNames.Add("castle");
            _modelNames.Add("chr_knight");
            _modelNames.Add("chr_old");
            _modelNames.Add("chr_rain");
            _modelNames.Add("chr_sword");
            _modelNames.Add("doom");
            _modelNames.Add("ephtracy");
            _modelNames.Add("menger");
            _modelNames.Add("monu1");
            _modelNames.Add("monu9");
            _modelNames.Add("nature");
            _modelNames.Add("shelf");
            _modelNames.Add("teapot");

            _modelChunks = new List<Chunk>();
            foreach (var model in _modelNames)
            {
                var chunk = Content.Load<Chunk>(model);
                _modelChunks.Add(chunk);
                chunk.Position = new Vector3(-chunk.SizeX/2, -chunk.SizeY/2, -chunk.SizeZ/2);
                chunk.Effect.CurrentTechnique = chunk.Effect.Techniques["Instancing"];
            }
        }

        protected override void Update(GameTime gameTime)
        {
            HandleInput();

            _cameraAngle += 25f * (float) gameTime.ElapsedGameTime.TotalSeconds;
            _cameraRotationMatrix = Matrix.CreateTranslation(CurrentChunk.Center + new Vector3(0, 0, CurrentChunk.SizeZ + 50))*
                                    Matrix.CreateRotationX(-MathHelper.Pi/12f)*
                                    Matrix.CreateRotationY(MathHelper.ToRadians(_cameraAngle));

            _viewMatrix = Matrix.CreateLookAt(_cameraRotationMatrix.Translation, CurrentChunk.Center, Vector3.Up);
            _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, _graphics.GraphicsDevice.Viewport.AspectRatio, 1, CurrentChunk.SizeZ*2 + 70);

            CurrentChunk.Effect.View = _viewMatrix;
            CurrentChunk.Effect.Projection = _projectionMatrix;


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            _deviceState.Push();

            // Render the background
            _spriteBatch.Begin();
            _spriteBatch.Draw(_background, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
            _spriteBatch.End();
            _deviceState.Pop();

            // Render the active model
            CurrentChunk.Draw();

            _deviceState.Push();

            // Render UI
            _spriteBatch.Begin();

            var modelIndexStr = $"Index: {_chunkIndex}";
            _spriteBatch.DrawString(_font, modelIndexStr, new Vector2(6f, GraphicsDevice.PresentationParameters.BackBufferHeight - 96), Color.Black);
            _spriteBatch.DrawString(_font, modelIndexStr, new Vector2(5f, GraphicsDevice.PresentationParameters.BackBufferHeight - 97), Color.White);

            var modelStr = $"Model: {CurrentModelName}";
            _spriteBatch.DrawString(_font, modelStr, new Vector2(6f, GraphicsDevice.PresentationParameters.BackBufferHeight - 71), Color.Black);
            _spriteBatch.DrawString(_font, modelStr, new Vector2(5f, GraphicsDevice.PresentationParameters.BackBufferHeight - 72), Color.White);

            var voxelsStr = $"Voxels: {CurrentChunk.BlockCount}";
            _spriteBatch.DrawString(_font, voxelsStr, new Vector2(6f, GraphicsDevice.PresentationParameters.BackBufferHeight - 46), Color.Black);
            _spriteBatch.DrawString(_font, voxelsStr, new Vector2(5f, GraphicsDevice.PresentationParameters.BackBufferHeight - 47), Color.White);

            var sizeStr = $"Size: {CurrentChunk.SizeX} x {CurrentChunk.SizeY} x {CurrentChunk.SizeZ}";
            _spriteBatch.DrawString(_font, sizeStr, new Vector2(6f, GraphicsDevice.PresentationParameters.BackBufferHeight - 21), Color.Black);
            _spriteBatch.DrawString(_font, sizeStr, new Vector2(5f, GraphicsDevice.PresentationParameters.BackBufferHeight - 22), Color.White);

            _spriteBatch.End();

            // Render components
            base.Draw(gameTime);
            _deviceState.Pop();
        }

        private void HandleInput()
        {
            if (Input.IsPressed(Keys.Escape)) Exit();

            if (Input.IsPressed(Keys.Z))
                ToggleWireframe();

            if (Input.IsPressed(Keys.Left) || Input.IsPressed(Keys.A)) PreviousModel();
            if (Input.IsPressed(Keys.Right) || Input.IsPressed(Keys.D)) NextModel();

            if (Input.IsPressed(Keys.D1)) SetModel(0);
            if (Input.IsPressed(Keys.D2)) SetModel(1);
            if (Input.IsPressed(Keys.D3)) SetModel(2);
            if (Input.IsPressed(Keys.D4)) SetModel(3);
            if (Input.IsPressed(Keys.D5)) SetModel(4);
            if (Input.IsPressed(Keys.D6)) SetModel(5);
            if (Input.IsPressed(Keys.D7)) SetModel(6);
            if (Input.IsPressed(Keys.D8)) SetModel(7);
            if (Input.IsPressed(Keys.D9)) SetModel(8);
        }

        private void SetModel(int index)
        {
            var length = _modelChunks.Count;
            _chunkIndex = (index%length + length)%length;
        }

        private void NextModel()
        {
            _chunkIndex = (_chunkIndex + 1) % _modelChunks.Count;
        }

        private void PreviousModel()
        {
            _chunkIndex = (_chunkIndex - 1 + _modelChunks.Count) % _modelChunks.Count;
        }

        private void ToggleWireframe()
        {
            if (GraphicsDevice.RasterizerState == _wireframeRasterizerState)
                GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            else
                GraphicsDevice.RasterizerState = _wireframeRasterizerState;
        }

        /*private VertexPositionColor[] cubeVertexData;
        private int[] cubeIndexData;

        private VoxelData[] voxels;
        private VoxelScene scene;
        */

        /*
        protected override void Initialize()
        {
            CreateCube(Color.White, out cubeVertexData, out cubeIndexData);

            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            background = new Texture2D(graphics.GraphicsDevice, 1, 1);
            background.SetData(new [] { Color.Gray });

            scene = Content.Load<VoxelScene>("monu1");
            voxels = scene.Voxels;
        }

        public void CreateCube(Color color, out VertexPositionColor[] vertexData, out int[] indexData)
        {

            vertexData = new []
            {

                // front
                new VertexPositionColor(new Vector3(-0.5f, -0.5f, -0.5f), color),
                new VertexPositionColor(new Vector3(0.5f, -0.5f, -0.5f), color),
                new VertexPositionColor(new Vector3(-0.5f, 0.5f, -0.5f), color),
                new VertexPositionColor(new Vector3(0.5f, 0.5f, -0.5f), color),

                // top
                new VertexPositionColor(new Vector3(-0.5f, 0.5f, -0.5f), color),
                new VertexPositionColor(new Vector3(0.5f, 0.5f, -0.5f), color),
                new VertexPositionColor(new Vector3(-0.5f, 0.5f, 0.5f), color),
                new VertexPositionColor(new Vector3(0.5f, 0.5f, 0.5f), color),

                // back
                new VertexPositionColor(new Vector3(0.5f, -0.5f, 0.5f), color),
                new VertexPositionColor(new Vector3(-0.5f, -0.5f, 0.5f), color),
                new VertexPositionColor(new Vector3(0.5f, 0.5f, 0.5f), color),
                new VertexPositionColor(new Vector3(-0.5f, 0.5f, 0.5f), color),

                // bottom
                new VertexPositionColor(new Vector3(0.5f, -0.5f, -0.5f), color),
                new VertexPositionColor(new Vector3(-0.5f, -0.5f, -0.5f), color),
                new VertexPositionColor(new Vector3(0.5f, -0.5f, 0.5f), color),
                new VertexPositionColor(new Vector3(-0.5f, -0.5f, 0.5f), color),

                // left
                new VertexPositionColor(new Vector3(-0.5f, -0.5f, 0.5f), color),
                new VertexPositionColor(new Vector3(-0.5f, -0.5f, -0.5f), color),
                new VertexPositionColor(new Vector3(-0.5f, 0.5f, 0.5f), color),
                new VertexPositionColor(new Vector3(-0.5f, 0.5f, -0.5f), color),

                // right
                new VertexPositionColor(new Vector3(0.5f, -0.5f, -0.5f), color),
                new VertexPositionColor(new Vector3(0.5f, -0.5f, 0.5f), color),
                new VertexPositionColor(new Vector3(0.5f, 0.5f, -0.5f), color),
                new VertexPositionColor(new Vector3(0.5f, 0.5f, 0.5f), color),

            };

            indexData = new [] { 
                0, 1, 2, 3, 2, 1,
                4, 5, 6, 7, 6, 5,
                8, 9, 10, 11, 10, 9,
                12, 13, 14, 15, 14, 13,
                16, 17, 18, 19, 18, 17,
                20, 21, 22, 23, 22, 21
             };

        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
            spriteBatch.End();

            cameraAngle += 2.5f;
            cameraRotationMatrix =  Matrix.CreateTranslation(new Vector3(0, 0, 40)) * Matrix.CreateRotationY(MathHelper.ToRadians(cameraAngle));
  
            viewMatrix = Matrix.CreateLookAt(cameraRotationMatrix.Translation, new Vector3(-10, 5, 10), Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphics.GraphicsDevice.Viewport.AspectRatio, 1, 100);

            GraphicsDevice.DepthStencilState = DepthStencilState.Default; 

            var blockCount = voxels.Length;

            var effect = new BasicEffect(graphics.GraphicsDevice);
            effect.View = viewMatrix;
            effect.Projection = projectionMatrix;

            lock (voxels)
            {
                for (int i = 0; i < blockCount; i++)
                {
                    var voxel = voxels[i];
                    effect.DiffuseColor = voxel.Color.ToVector3();
                    effect.World = Matrix.CreateTranslation(voxel.X, voxel.Y, voxel.Z);
                    effect.World = effect.World * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateRotationZ(MathHelper.ToRadians(180));

                    foreach (var pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, cubeVertexData, 0, 4 * 6, cubeIndexData, 0, cubeIndexData.Length / 3);
                    }

                }
            }

            base.Draw(gameTime); 
           
        }
        */
    }
}
