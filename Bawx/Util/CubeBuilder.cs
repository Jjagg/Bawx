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

        public static short[] GetShortIndices(int start = 0, bool copy = false)
        {
            return new[]
            {
                // Front
                (short) (start + 0), (short) (start + 1), (short) (start + 2),
                (short) (start + 2), (short) (start + 3), (short) (start + 0),

                // Back
                (short) (start + 6), (short) (start + 5), (short) (start + 4),
                (short) (start + 4), (short) (start + 7), (short) (start + 6),

                // Top
                (short) (start + 10), (short) (start + 9), (short) (start + 8),
                (short) (start + 8), (short) (start + 11), (short) (start + 10),

                // Bottom
                (short) (start + 12), (short) (start + 13), (short) (start + 14),
                (short) (start + 14), (short) (start + 15), (short) (start + 12),

                // Left
                (short) (start + 18), (short) (start + 17), (short) (start + 16),
                (short) (start + 16), (short) (start + 19), (short) (start + 18),

                // Right
                (short) (start + 20), (short) (start + 21), (short) (start + 22),
                (short) (start + 22), (short) (start + 23), (short) (start + 20),
            };
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

        public static int[] GetIndices(int start = 0, bool copy = false)
        {
            return new[]
            {
                // Front
                start + 0, start + 1, start + 2,
                start + 2, start + 3, start + 0,

                // Back
                start + 6, start + 5, start + 4,
                start + 4, start + 7, start + 6,

                // Top
                start + 10, start + 9, start + 8,
                start + 8, start + 11, start + 10,

                // Bottom
                start + 12, start + 13, start + 14,
                start + 14, start + 15, start + 12,

                // Left
                start + 18, start + 17, start + 16,
                start + 16, start + 19, start + 18,

                // Right
                start + 20, start + 21, start + 22,
                start + 22, start + 23, start + 20,
            };
        }

        #endregion

        public static Vector3[] GetNormals()
        {
            return new[]
            {
                Vector3.UnitX,
                -Vector3.UnitX,
                Vector3.UnitY,
                -Vector3.UnitY,
                Vector3.UnitZ,
                -Vector3.UnitZ,
            };
        }
    }
}