using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace Bawx.Util
{
    public static class Extensions
    {

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
    }
}