using System;
using Microsoft.Xna.Framework;

namespace Bawx
{
    internal static class Extensions
    {
        /// <summary>
        ///     Gets the center coordinates of this bounding box.
        /// </summary>
        /// <param name="box">The bounding box.</param>
        /// <returns></returns>
        public static Vector3 GetCenter(this BoundingBox box)
        {
            var width = box.Max.X - box.Min.X;
            var height = box.Max.Y - box.Min.Y;
            var depth = box.Max.Z - box.Min.Z;
            return new Vector3(
                box.Min.X + width/2.0f,
                box.Min.Y + height/2.0f,
                box.Min.Z + depth/2.0f
                );
        }

        /// <summary>
        ///     Gets the bounding box's width.
        /// </summary>
        /// <param name="box">The box.</param>
        /// <returns></returns>
        public static float GetWidth(this BoundingBox box)
        {
            return Math.Abs(box.Max.X - box.Min.X);
        }

        /// <summary>
        ///     Gets the bounding box's height.
        /// </summary>
        /// <param name="box">The box.</param>
        /// <returns></returns>
        public static float GetHeight(this BoundingBox box)
        {
            return Math.Abs(box.Max.Y - box.Min.Y);
        }

        /// <summary>
        ///     Gets the bounding box's depth.
        /// </summary>
        /// <param name="box">The box.</param>
        /// <returns></returns>
        public static float GetDepth(this BoundingBox box)
        {
            return Math.Abs(box.Max.Z - box.Min.Z);
        }

        /// <summary>
        ///     Gets the dimensions of this bounding box.
        /// </summary>
        /// <param name="box">This bounding box.</param>
        /// <param name="dim">The dimension variable to populate.</param>
        /// <returns></returns>
        public static void GetDimensions(this BoundingBox box, ref Vector3 dim)
        {
            dim.X = box.GetWidth();
            dim.Y = box.GetHeight();
            dim.Z = box.GetDepth();
        }

        /// <summary>
        ///     Gets the dimensions of this bounding box.
        /// </summary>
        /// <param name="box">This bounding box.</param>
        /// <returns></returns>
        public static Vector3 GetDimensions(this BoundingBox box)
        {
            var vec = new Vector3();
            box.GetDimensions(ref vec);
            return vec;
        }
    }
}