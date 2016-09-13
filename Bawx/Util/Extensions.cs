using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace Bawx.Util
{
    public static class Extensions
    {

        #region Packing

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte GetX(this Byte4 data)
        {
            return (byte) (data.PackedValue & 0xFF);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte GetY(this Byte4 data)
        {
            return (byte) ((data.PackedValue >> 0x8) & 0xFF);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte GetZ(this Byte4 data)
        {
            return (byte) ((data.PackedValue >> 0x10) & 0xFF);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte GetW(this Byte4 data)
        {
            return (byte) ((data.PackedValue >> 0x18) & 0xFF);
        }

        public static Vector3 ToVector3(this Byte4 data)
        {
            return new Vector3(
                (data.PackedValue & 0xFF),
                (data.PackedValue >> 0x8) & 0xFF,
                (data.PackedValue >> 0x10) & 0xFF);
        }

        public static void SetX(this Byte4 data, byte value)
        {
            data.PackedValue = (data.PackedValue & 0xFFFFFF00) | (uint) value;
        }

        public static void SetY(this Byte4 data, byte value)
        {
            data.PackedValue = (data.PackedValue & 0xFFFF00FF) | ((uint) value << 8);
        }

        public static void SetZ(this Byte4 data, byte value)
        {
            data.PackedValue = (data.PackedValue & 0xFF00FFFF) | ((uint) value << 16);
        }

        public static void SetW(this Byte4 data, byte value)
        {
            data.PackedValue = (data.PackedValue & 0x00FFFFFF) | ((uint) value << 24);
        }

        public static void SetXYZ(this Byte4 data, Vector3 value)
        {
            var w = data.GetW();
            data.PackedValue = value.Pack() | ((uint) w << 24);
        }

        public static uint Pack(this Vector3 v)
        {
            return (uint) v.X | ((uint) v.Y << 8) | ((uint) v.Z << 16);
        }

        #endregion

        #region Lighting

        /// <summary>
        /// Creates the ViewProjection matrix from the perspective of the directional
        /// light using the cameras bounding frustum to determine what is visible 
        /// in the scene.
        /// </summary>
        /// <returns>The ViewProjection for the light</returns>
        public static Matrix GetViewProjectionMatrix(this Vector3 lightDirection, BoundingFrustum cameraFrustrum)
        {
            // Matrix with that will rotate in points the direction of the light
            var lightRotation = Matrix.CreateLookAt(Vector3.Zero, 
                                                       lightDirection,
                                                       Vector3.Up);

            // Get the corners of the frustum
            var frustumCorners = cameraFrustrum.GetCorners();

            // Transform the positions of the corners into the direction of the light
            for (var i = 0; i < frustumCorners.Length; i++)
                frustumCorners[i] = Vector3.Transform(frustumCorners[i], lightRotation);

            // Find the smallest box around the points
            var lightBox = BoundingBox.CreateFromPoints(frustumCorners);

            var boxSize = lightBox.Max - lightBox.Min;
            var halfBoxSize = boxSize * 0.5f;

            // The position of the light should be in the center of the back
            // panel of the box. 
            var lightPosition = lightBox.Min + halfBoxSize;
            lightPosition.Z = lightBox.Min.Z;

            // We need the position back in world coordinates so we transform 
            // the light position by the inverse of the lights rotation
            lightPosition = Vector3.Transform(lightPosition, Matrix.Invert(lightRotation));

            // Create the view matrix for the light
            var lightView = Matrix.CreateLookAt(lightPosition, lightPosition + lightDirection, Vector3.Up);

            // Create the projection matrix for the light
            // The projection is orthographic since we are using a directional light
            var lightProjection = Matrix.CreateOrthographic(boxSize.X, boxSize.Y, -boxSize.Z, boxSize.Z);

            return lightView * lightProjection;

        }

        #endregion
    }
}