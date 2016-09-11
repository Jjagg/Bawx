// Code taken or based from BoxHelpers class in the MonoGame test framework

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bawx.Util
{
    public static class CubeBuilder
    {
        #region Util

        // Extension method to copy an array
        private static T[] Copy<T>(this T[] array)
        {
            var copy = new T[array.Length];
            Array.Copy(array, copy, array.Length);
            return copy;
        }

        #endregion

        #region Position

        private static VertexPosition[] _vertexPosition;
        public static VertexPosition[] GetPosition(bool copy = false)
        {
            if (_vertexPosition == null)
            {
                _vertexPosition = new[]
                {
                    // Front
                    new VertexPosition(new Vector3(-0.5f, -0.5f, -0.5f)),
                    new VertexPosition(new Vector3(0.5f, -0.5f, -0.5f)),
                    new VertexPosition(new Vector3(0.5f, 0.5f, -0.5f)),
                    new VertexPosition(new Vector3(-0.5f, 0.5f, -0.5f)),

                    // Back
                    new VertexPosition(new Vector3(-0.5f, -0.5f, 0.5f)),
                    new VertexPosition(new Vector3(0.5f, -0.5f, 0.5f)),
                    new VertexPosition(new Vector3(0.5f, 0.5f, 0.5f)),
                    new VertexPosition(new Vector3(-0.5f, 0.5f, 0.5f)),

                    // Top
                    new VertexPosition(new Vector3(-0.5f, -0.5f, -0.5f)),
                    new VertexPosition(new Vector3(0.5f, -0.5f, -0.5f)),
                    new VertexPosition(new Vector3(0.5f, -0.5f, 0.5f)),
                    new VertexPosition(new Vector3(-0.5f, -0.5f, 0.5f)),

                    // Bottom
                    new VertexPosition(new Vector3(-0.5f, 0.5f, -0.5f)),
                    new VertexPosition(new Vector3(0.5f, 0.5f, -0.5f)),
                    new VertexPosition(new Vector3(0.5f, 0.5f, 0.5f)),
                    new VertexPosition(new Vector3(-0.5f, 0.5f, 0.5f)),

                    // Left
                    new VertexPosition(new Vector3(-0.5f, -0.5f, -0.5f)),
                    new VertexPosition(new Vector3(-0.5f, -0.5f, 0.5f)),
                    new VertexPosition(new Vector3(-0.5f, 0.5f, 0.5f)),
                    new VertexPosition(new Vector3(-0.5f, 0.5f, -0.5f)),

                    // Right
                    new VertexPosition(new Vector3(0.5f, -0.5f, -0.5f)),
                    new VertexPosition(new Vector3(0.5f, -0.5f, 0.5f)),
                    new VertexPosition(new Vector3(0.5f, 0.5f, 0.5f)),
                    new VertexPosition(new Vector3(0.5f, 0.5f, -0.5f)),
                };
            }
            return copy ? _vertexPosition.Copy() : _vertexPosition;
        }

        public static VertexPosition[] GetPosition(Vector3 position)
        {
            return new[]
            {
                // Front
                new VertexPosition(position + new Vector3(-0.5f, -0.5f, -0.5f)),
                new VertexPosition(position + new Vector3(0.5f, -0.5f, -0.5f)),
                new VertexPosition(position + new Vector3(0.5f, 0.5f, -0.5f)),
                new VertexPosition(position + new Vector3(-0.5f, 0.5f, -0.5f)),

                // Back
                new VertexPosition(position + new Vector3(-0.5f, -0.5f, 0.5f)),
                new VertexPosition(position + new Vector3(0.5f, -0.5f, 0.5f)),
                new VertexPosition(position + new Vector3(0.5f, 0.5f, 0.5f)),
                new VertexPosition(position + new Vector3(-0.5f, 0.5f, 0.5f)),

                // Top
                new VertexPosition(position + new Vector3(-0.5f, -0.5f, -0.5f)),
                new VertexPosition(position + new Vector3(0.5f, -0.5f, -0.5f)),
                new VertexPosition(position + new Vector3(0.5f, -0.5f, 0.5f)),
                new VertexPosition(position + new Vector3(-0.5f, -0.5f, 0.5f)),

                // Bottom
                new VertexPosition(position + new Vector3(-0.5f, 0.5f, -0.5f)),
                new VertexPosition(position + new Vector3(0.5f, 0.5f, -0.5f)),
                new VertexPosition(position + new Vector3(0.5f, 0.5f, 0.5f)),
                new VertexPosition(position + new Vector3(-0.5f, 0.5f, 0.5f)),

                // Left
                new VertexPosition(position + new Vector3(-0.5f, -0.5f, -0.5f)),
                new VertexPosition(position + new Vector3(-0.5f, -0.5f, 0.5f)),
                new VertexPosition(position + new Vector3(-0.5f, 0.5f, 0.5f)),
                new VertexPosition(position + new Vector3(-0.5f, 0.5f, -0.5f)),

                // Right
                new VertexPosition(position + new Vector3(0.5f, -0.5f, -0.5f)),
                new VertexPosition(position + new Vector3(0.5f, -0.5f, 0.5f)),
                new VertexPosition(position + new Vector3(0.5f, 0.5f, 0.5f)),
                new VertexPosition(position + new Vector3(0.5f, 0.5f, -0.5f)),
            };
        }

        #endregion

        #region PositionNormal

        private static VertexPositionNormal[] _vertexNormal;
        public static VertexPositionNormal[] GetNormal(bool copy = false)
        {
            if (_vertexNormal == null)
            {
                _vertexNormal = new[]
                {
                    // Front
                    new VertexPositionNormal(new Vector3(-0.5f, -0.5f, -0.5f), -Vector3.UnitZ),
                    new VertexPositionNormal(new Vector3(0.5f, -0.5f, -0.5f), -Vector3.UnitZ),
                    new VertexPositionNormal(new Vector3(0.5f, 0.5f, -0.5f), -Vector3.UnitZ),
                    new VertexPositionNormal(new Vector3(-0.5f, 0.5f, -0.5f), -Vector3.UnitZ),

                    // Back
                    new VertexPositionNormal(new Vector3(-0.5f, -0.5f, 0.5f), Vector3.UnitZ),
                    new VertexPositionNormal(new Vector3(0.5f, -0.5f, 0.5f), Vector3.UnitZ),
                    new VertexPositionNormal(new Vector3(0.5f, 0.5f, 0.5f), Vector3.UnitZ),
                    new VertexPositionNormal(new Vector3(-0.5f, 0.5f, 0.5f), Vector3.UnitZ),

                    // Top
                    new VertexPositionNormal(new Vector3(-0.5f, -0.5f, -0.5f), -Vector3.UnitY),
                    new VertexPositionNormal(new Vector3(0.5f, -0.5f, -0.5f), -Vector3.UnitY),
                    new VertexPositionNormal(new Vector3(0.5f, -0.5f, 0.5f), -Vector3.UnitY),
                    new VertexPositionNormal(new Vector3(-0.5f, -0.5f, 0.5f), -Vector3.UnitY),

                    // Bottom
                    new VertexPositionNormal(new Vector3(-0.5f, 0.5f, -0.5f), Vector3.UnitY),
                    new VertexPositionNormal(new Vector3(0.5f, 0.5f, -0.5f), Vector3.UnitY),
                    new VertexPositionNormal(new Vector3(0.5f, 0.5f, 0.5f), Vector3.UnitY),
                    new VertexPositionNormal(new Vector3(-0.5f, 0.5f, 0.5f), Vector3.UnitY),

                    // Left
                    new VertexPositionNormal(new Vector3(-0.5f, -0.5f, -0.5f), -Vector3.UnitX),
                    new VertexPositionNormal(new Vector3(-0.5f, -0.5f, 0.5f), -Vector3.UnitX),
                    new VertexPositionNormal(new Vector3(-0.5f, 0.5f, 0.5f), -Vector3.UnitX),
                    new VertexPositionNormal(new Vector3(-0.5f, 0.5f, -0.5f), -Vector3.UnitX),

                    // Right
                    new VertexPositionNormal(new Vector3(0.5f, -0.5f, -0.5f), Vector3.UnitX),
                    new VertexPositionNormal(new Vector3(0.5f, -0.5f, 0.5f), Vector3.UnitX),
                    new VertexPositionNormal(new Vector3(0.5f, 0.5f, 0.5f), Vector3.UnitX),
                    new VertexPositionNormal(new Vector3(0.5f, 0.5f, -0.5f), Vector3.UnitX),
                };
            }

            return copy ? _vertexNormal.Copy() : _vertexNormal;
        }

        public static VertexPositionNormal[] GetNormal(Vector3 position)
        {
            return new[]
            {
                // Front
                new VertexPositionNormal(position + new Vector3(-0.5f, -0.5f, -0.5f), -Vector3.UnitZ),
                new VertexPositionNormal(position + new Vector3(0.5f, -0.5f, -0.5f), -Vector3.UnitZ),
                new VertexPositionNormal(position + new Vector3(0.5f, 0.5f, -0.5f), -Vector3.UnitZ),
                new VertexPositionNormal(position + new Vector3(-0.5f, 0.5f, -0.5f), -Vector3.UnitZ),

                // Back
                new VertexPositionNormal(position + new Vector3(-0.5f, -0.5f, 0.5f), Vector3.UnitZ),
                new VertexPositionNormal(position + new Vector3(0.5f, -0.5f, 0.5f), Vector3.UnitZ),
                new VertexPositionNormal(position + new Vector3(0.5f, 0.5f, 0.5f), Vector3.UnitZ),
                new VertexPositionNormal(position + new Vector3(-0.5f, 0.5f, 0.5f), Vector3.UnitZ),

                // Top
                new VertexPositionNormal(position + new Vector3(-0.5f, -0.5f, -0.5f), -Vector3.UnitY),
                new VertexPositionNormal(position + new Vector3(0.5f, -0.5f, -0.5f), -Vector3.UnitY),
                new VertexPositionNormal(position + new Vector3(0.5f, -0.5f, 0.5f), -Vector3.UnitY),
                new VertexPositionNormal(position + new Vector3(-0.5f, -0.5f, 0.5f), -Vector3.UnitY),

                // Bottom
                new VertexPositionNormal(position + new Vector3(-0.5f, 0.5f, -0.5f), Vector3.UnitY),
                new VertexPositionNormal(position + new Vector3(0.5f, 0.5f, -0.5f), Vector3.UnitY),
                new VertexPositionNormal(position + new Vector3(0.5f, 0.5f, 0.5f), Vector3.UnitY),
                new VertexPositionNormal(position + new Vector3(-0.5f, 0.5f, 0.5f), Vector3.UnitY),

                // Left
                new VertexPositionNormal(position + new Vector3(-0.5f, -0.5f, -0.5f), -Vector3.UnitX),
                new VertexPositionNormal(position + new Vector3(-0.5f, -0.5f, 0.5f), -Vector3.UnitX),
                new VertexPositionNormal(position + new Vector3(-0.5f, 0.5f, 0.5f), -Vector3.UnitX),
                new VertexPositionNormal(position + new Vector3(-0.5f, 0.5f, -0.5f), -Vector3.UnitX),

                // Right
                new VertexPositionNormal(position + new Vector3(0.5f, -0.5f, -0.5f), Vector3.UnitX),
                new VertexPositionNormal(position + new Vector3(0.5f, -0.5f, 0.5f), Vector3.UnitX),
                new VertexPositionNormal(position + new Vector3(0.5f, 0.5f, 0.5f), Vector3.UnitX),
                new VertexPositionNormal(position + new Vector3(0.5f, 0.5f, -0.5f), Vector3.UnitX),
            };
        }

        private static VertexBuffer _normalBuffer;
        public static VertexBuffer GetNormalBuffer(GraphicsDevice graphicsDevice)
        {
            if (_normalBuffer == null)
            {
                _normalBuffer = new VertexBuffer(graphicsDevice, VertexPositionNormal.VertexDeclaration, 24,
                    BufferUsage.WriteOnly);
                _normalBuffer.SetData(GetNormal());
            }
            return _normalBuffer;
        }

        #endregion

        #region PositionNormalColor

        private static VertexPositionNormalColor[] _normalColor;
        public static VertexPositionNormalColor[] GetNormalColor()
        {
            if (_normalColor == null)
            {
                _normalColor = new[]
                {
                    // Front
                    new VertexPositionNormalColor(new Vector3(-0.5f, -0.5f, -0.5f), -Vector3.UnitZ, Color.White),
                    new VertexPositionNormalColor(new Vector3(0.5f, -0.5f, -0.5f), -Vector3.UnitZ, Color.White),
                    new VertexPositionNormalColor(new Vector3(0.5f, 0.5f, -0.5f), -Vector3.UnitZ, Color.White),
                    new VertexPositionNormalColor(new Vector3(-0.5f, 0.5f, -0.5f), -Vector3.UnitZ, Color.White),

                    // Back
                    new VertexPositionNormalColor(new Vector3(-0.5f, -0.5f, 0.5f), Vector3.UnitZ, Color.White),
                    new VertexPositionNormalColor(new Vector3(0.5f, -0.5f, 0.5f), Vector3.UnitZ, Color.White),
                    new VertexPositionNormalColor(new Vector3(0.5f, 0.5f, 0.5f), Vector3.UnitZ, Color.White),
                    new VertexPositionNormalColor(new Vector3(-0.5f, 0.5f, 0.5f), Vector3.UnitZ, Color.White),

                    // Top
                    new VertexPositionNormalColor(new Vector3(-0.5f, -0.5f, -0.5f), -Vector3.UnitY, Color.White),
                    new VertexPositionNormalColor(new Vector3(0.5f, -0.5f, -0.5f), -Vector3.UnitY, Color.White),
                    new VertexPositionNormalColor(new Vector3(0.5f, -0.5f, 0.5f), -Vector3.UnitY, Color.White),
                    new VertexPositionNormalColor(new Vector3(-0.5f, -0.5f, 0.5f), -Vector3.UnitY, Color.White),

                    // Bottom
                    new VertexPositionNormalColor(new Vector3(-0.5f, 0.5f, -0.5f), Vector3.UnitY, Color.White),
                    new VertexPositionNormalColor(new Vector3(0.5f, 0.5f, -0.5f), Vector3.UnitY, Color.White),
                    new VertexPositionNormalColor(new Vector3(0.5f, 0.5f, 0.5f), Vector3.UnitY, Color.White),
                    new VertexPositionNormalColor(new Vector3(-0.5f, 0.5f, 0.5f), Vector3.UnitY, Color.White),

                    // Left
                    new VertexPositionNormalColor(new Vector3(-0.5f, -0.5f, -0.5f), -Vector3.UnitX, Color.White),
                    new VertexPositionNormalColor(new Vector3(-0.5f, -0.5f, 0.5f), -Vector3.UnitX, Color.White),
                    new VertexPositionNormalColor(new Vector3(-0.5f, 0.5f, 0.5f), -Vector3.UnitX, Color.White),
                    new VertexPositionNormalColor(new Vector3(-0.5f, 0.5f, -0.5f), -Vector3.UnitX, Color.White),

                    // Right
                    new VertexPositionNormalColor(new Vector3(0.5f, -0.5f, -0.5f), Vector3.UnitX, Color.White),
                    new VertexPositionNormalColor(new Vector3(0.5f, -0.5f, 0.5f), Vector3.UnitX, Color.White),
                    new VertexPositionNormalColor(new Vector3(0.5f, 0.5f, 0.5f), Vector3.UnitX, Color.White),
                    new VertexPositionNormalColor(new Vector3(0.5f, 0.5f, -0.5f), Vector3.UnitX, Color.White),
                };
            }
            return _normalColor;
        }

        public static VertexPositionNormalColor[] GetNormalColor(Vector3 position, Color? color = null)
        {
            var c = color ?? Color.White;

            return new[]
            {
                // Front
                new VertexPositionNormalColor(position + new Vector3(-0.5f, -0.5f, -0.5f), -Vector3.UnitZ, c),
                new VertexPositionNormalColor(position + new Vector3(0.5f, -0.5f, -0.5f), -Vector3.UnitZ, c),
                new VertexPositionNormalColor(position + new Vector3(0.5f, 0.5f, -0.5f), -Vector3.UnitZ, c),
                new VertexPositionNormalColor(position + new Vector3(-0.5f, 0.5f, -0.5f), -Vector3.UnitZ, c),

                // Back
                new VertexPositionNormalColor(position + new Vector3(-0.5f, -0.5f, 0.5f), Vector3.UnitZ, c),
                new VertexPositionNormalColor(position + new Vector3(0.5f, -0.5f, 0.5f), Vector3.UnitZ, c),
                new VertexPositionNormalColor(position + new Vector3(0.5f, 0.5f, 0.5f), Vector3.UnitZ, c),
                new VertexPositionNormalColor(position + new Vector3(-0.5f, 0.5f, 0.5f), Vector3.UnitZ, c),

                // Top
                new VertexPositionNormalColor(position + new Vector3(-0.5f, -0.5f, -0.5f), -Vector3.UnitY, c),
                new VertexPositionNormalColor(position + new Vector3(0.5f, -0.5f, -0.5f), -Vector3.UnitY, c),
                new VertexPositionNormalColor(position + new Vector3(0.5f, -0.5f, 0.5f), -Vector3.UnitY, c),
                new VertexPositionNormalColor(position + new Vector3(-0.5f, -0.5f, 0.5f), -Vector3.UnitY, c),

                // Bottom
                new VertexPositionNormalColor(position + new Vector3(-0.5f, 0.5f, -0.5f), Vector3.UnitY, c),
                new VertexPositionNormalColor(position + new Vector3(0.5f, 0.5f, -0.5f), Vector3.UnitY, c),
                new VertexPositionNormalColor(position + new Vector3(0.5f, 0.5f, 0.5f), Vector3.UnitY, c),
                new VertexPositionNormalColor(position + new Vector3(-0.5f, 0.5f, 0.5f), Vector3.UnitY, c),

                // Left
                new VertexPositionNormalColor(position + new Vector3(-0.5f, -0.5f, -0.5f), -Vector3.UnitX, c),
                new VertexPositionNormalColor(position + new Vector3(-0.5f, -0.5f, 0.5f), -Vector3.UnitX, c),
                new VertexPositionNormalColor(position + new Vector3(-0.5f, 0.5f, 0.5f), -Vector3.UnitX, c),
                new VertexPositionNormalColor(position + new Vector3(-0.5f, 0.5f, -0.5f), -Vector3.UnitX, c),

                // Right
                new VertexPositionNormalColor(position + new Vector3(0.5f, -0.5f, -0.5f), Vector3.UnitX, c),
                new VertexPositionNormalColor(position + new Vector3(0.5f, -0.5f, 0.5f), Vector3.UnitX, c),
                new VertexPositionNormalColor(position + new Vector3(0.5f, 0.5f, 0.5f), Vector3.UnitX, c),
                new VertexPositionNormalColor(position + new Vector3(0.5f, 0.5f, -0.5f), Vector3.UnitX, c),
            };
        }

        #endregion

        #region Short Indices

        private static short[] _shortIndices;
        public static short[] GetShortIndices(bool copy = false)
        {
            if (_shortIndices == null)
            {
                _shortIndices = new short[]
                {
                    // Front
                    0, 1, 2,
                    2, 3, 0,

                    // Back
                    6, 5, 4,
                    4, 7, 6,

                    // Top
                    10, 9, 8,
                    8, 11, 10,

                    // Bottom
                    12, 13, 14,
                    14, 15, 12,

                    // Left
                    18, 17, 16,
                    16, 19, 18,

                    // Right
                    20, 21, 22,
                    22, 23, 20,
                };
            }

            return copy ? _shortIndices.Copy() : _shortIndices;
        }

        private static IndexBuffer _shortIndexBuffer;
        public static IndexBuffer GetShortIndexBuffer(GraphicsDevice graphicsDevice)
        {
            if (_shortIndexBuffer == null)
            {
                _shortIndexBuffer = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, 36, BufferUsage.WriteOnly);
                _shortIndexBuffer.SetData(GetShortIndices());
            }
            return _shortIndexBuffer;
        }

        #endregion

        #region Int Indices

        private static int[] _intIndices;
        public static int[] GetIntIndex(bool copy = false)
        {
            if (_intIndices == null)
            {
                _intIndices = new int[]
                {
                    // Front
                    0, 1, 2,
                    2, 3, 0,

                    // Back
                    6, 5, 4,
                    4, 7, 6,

                    // Top
                    10, 9, 8,
                    8, 11, 10,

                    // Bottom
                    12, 13, 14,
                    14, 15, 12,

                    // Left
                    18, 17, 16,
                    16, 19, 18,

                    // Right
                    20, 21, 22,
                    22, 23, 20,
                };
            }

            return copy ? _intIndices.Copy() : _intIndices;
        }

        #endregion
    }
}