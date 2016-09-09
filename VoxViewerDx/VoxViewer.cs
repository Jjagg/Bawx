using System.Collections.Generic;
using Bawx;
using MGWheels.MiniUtils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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


        private Chunk _chunk;

        private GraphicsStateStack _deviceState;
        private List<Chunk> _modelChunks;

        public VoxViewer()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            //_graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";

            IsFixedTimeStep = false;
            _graphics.SynchronizeWithVerticalRetrace = false;
        }

        protected override void Initialize()
        {
            Components.Add(new FrameRateCounter(this));
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

            _chunk = Content.Load<Chunk>("monu1");
            _chunk.Effect.CurrentTechnique = _chunk.Effect.Techniques["Instancing"];
        }

        protected override void Update(GameTime gameTime)
        {
            _cameraAngle += 25f * (float) gameTime.ElapsedGameTime.TotalSeconds;
            _cameraRotationMatrix = Matrix.CreateTranslation(new Vector3(0, 0, _chunk.SizeZ*1.5f))*
                                    Matrix.CreateRotationX(-MathHelper.Pi/12f)*
                                    Matrix.CreateRotationY(MathHelper.ToRadians(_cameraAngle));

            _viewMatrix = Matrix.CreateLookAt(_cameraRotationMatrix.Translation, Vector3.Zero, Vector3.Up);
            _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, _graphics.GraphicsDevice.Viewport.AspectRatio, 1, _chunk.SizeZ*3);

            _chunk.Effect.View = _viewMatrix;
            _chunk.Effect.Projection = _projectionMatrix;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.LightGray);

            _deviceState.Push();
            _spriteBatch.Begin();
            _spriteBatch.Draw(_background, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
            _spriteBatch.End();
            _deviceState.Pop();

            _chunk.Draw();

            _deviceState.Push();
            base.Draw(gameTime);
            _deviceState.Pop();
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
