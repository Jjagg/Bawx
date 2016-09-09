using Bawx;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VoxViewer
{
    class VoxViewer : Game
    {

        GraphicsDeviceManager graphics;
        RasterizerState WIREFRAME_RASTERIZER_STATE = new RasterizerState() { CullMode = CullMode.None, FillMode = FillMode.WireFrame };

        Matrix viewMatrix;
        Matrix projectionMatrix;
        Matrix cameraRotationMatrix;

        float cameraAngle = 0;

        private VertexPositionColor[] cubeVertexData;
        private int[] cubeIndexData;

        private SpriteBatch spriteBatch;
        private Texture2D background;

        private VoxelData[] voxels;
        private VoxelScene scene;

        private Chunk chunk;

        public VoxViewer()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            background = new Texture2D(graphics.GraphicsDevice, 1, 1);
            background.SetData(new [] { Color.Gray });

            chunk = Content.Load<Chunk>("monu1");
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            /*spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
            spriteBatch.End();
            */

            cameraAngle += 2.5f;
            cameraRotationMatrix =  Matrix.CreateTranslation(new Vector3(0, 0, 40)) * Matrix.CreateRotationY(MathHelper.ToRadians(cameraAngle));
  
            viewMatrix = Matrix.CreateLookAt(cameraRotationMatrix.Translation, new Vector3(-10, 5, 10), Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphics.GraphicsDevice.Viewport.AspectRatio, 1, 100);

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            chunk.Effect.View = viewMatrix;
            chunk.Effect.Projection = projectionMatrix;

            chunk.Draw();
        }

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
