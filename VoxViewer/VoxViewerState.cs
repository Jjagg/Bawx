using System;
using System.Collections.Generic;
using Bawx.VoxelData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VoxViewerDx;

namespace VoxViewer
{
    internal partial class VoxViewer : Game
    {

        public class VoxViewerState
        {
            private readonly VoxViewer _voxViewer;

            private readonly RasterizerState _wireframeRasterizerState = new RasterizerState { CullMode = CullMode.CullCounterClockwiseFace, FillMode = FillMode.WireFrame };

            public int ChunkIndex { get; private set; }
            private int ModelCount => _modelNames.Count;

            public RenderState RenderState = RenderState.Simple;
            public CameraState CameraState = CameraState.Static;
            public bool WireframeMode;

            private float _cameraAngle;
            public Matrix ViewMatrix { get; private set; }
            public Matrix ProjectionMatrix { get; private set; }

            private readonly List<string> _modelNames;
            private readonly List<Chunk> _modelChunks;

            public string CurrentModelName => _modelNames[ChunkIndex];
            public Chunk CurrentChunk => _modelChunks[ChunkIndex];

            private GraphicsDevice GraphicsDevice => _voxViewer.GraphicsDevice;

            public VoxViewerState(VoxViewer voxViewer, List<string> modelNames, List<Chunk> modelChunks)
            {
                _voxViewer = voxViewer;
                _modelNames = modelNames;
                _modelChunks = modelChunks;
                RecomputeProjection();
            }

            public void Update(GameTime gameTime)
            {
                HandleInput();

                UpdateCameraState(gameTime);
            }

            public void HandleInput()
            {
                if (Input.IsPressed(Keys.Left) || Input.IsPressed(Keys.A)) SetModel(ChunkIndex -1);
                if (Input.IsPressed(Keys.Right) || Input.IsPressed(Keys.D)) SetModel(ChunkIndex + 1);

                if (Input.IsPressed(Keys.D1)) SetModel(0);
                if (Input.IsPressed(Keys.D2)) SetModel(1);
                if (Input.IsPressed(Keys.D3)) SetModel(2);
                if (Input.IsPressed(Keys.D4)) SetModel(3);
                if (Input.IsPressed(Keys.D5)) SetModel(4);
                if (Input.IsPressed(Keys.D6)) SetModel(5);
                if (Input.IsPressed(Keys.D7)) SetModel(6);
                if (Input.IsPressed(Keys.D8)) SetModel(7);
                if (Input.IsPressed(Keys.D9)) SetModel(8);

                if (Input.IsPressed(Keys.Up))
                    RenderState = (RenderState) ((int) (RenderState + 1)%RenderStateCount);
                if (Input.IsPressed(Keys.Down))
                    RenderState = (RenderState) ((int) (RenderState - 1 + RenderStateCount)%RenderStateCount);

                if (Input.IsPressed(Keys.Z)) ToggleWireframe();

                if (Input.IsPressed(Keys.R)) ToggleRotation();
            }

            public void UpdateCameraState(GameTime gameTime)
            {
                if (CameraState == CameraState.Rotating)
                    _cameraAngle += 0.6f*(float) gameTime.ElapsedGameTime.TotalSeconds;
                if (CameraState == CameraState.Rotating || CameraState == CameraState.Static)
                {
                    var dist = CurrentChunk.SizeZ + 70;
                    var cameraPos = CurrentChunk.Center + new Vector3(
                        dist*(float) Math.Sin(_cameraAngle),
                        dist/4f,
                        dist*(float) Math.Cos(_cameraAngle));

                    ViewMatrix = Matrix.CreateLookAt(cameraPos, CurrentChunk.Center, Vector3.Up);
                }
            }

            private void ToggleWireframe()
            {
                WireframeMode = !WireframeMode;
                // TODO culling
                GraphicsDevice.RasterizerState = WireframeMode ? _wireframeRasterizerState : RasterizerState.CullNone;
            }

            private void ToggleRotation()
            {
                CameraState = CameraState == CameraState.Rotating ? CameraState.Static : CameraState.Rotating;
            }

            private void SetModel(int index)
            {
                var length = ModelCount;
                ChunkIndex = (index%length + length)%length;
                RecomputeProjection();
            }

            private void RecomputeProjection()
            {
                ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                    MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 
                    CurrentChunk.SizeZ*2 + 100);
            }
        }
    }
}
