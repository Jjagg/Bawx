using System;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework;

namespace Bawx
{
    public static class EffectHelper
    {
        #region DX or OpenGL

        private static int GetShaderProfile() {
            // use reflection to figure out if Shader.Profile is OpenGL (0) or DirectX (1)
            var mgAssembly = Assembly.GetAssembly(typeof(Game));
            var shaderType = mgAssembly.GetType("Microsoft.Xna.Framework.Graphics.Shader");
            var profileProperty = shaderType.GetProperty("Profile");
            return (int) profileProperty.GetValue(null);
        }

        public static readonly int ShaderProfile;

        static EffectHelper()
        {
            ShaderProfile = GetShaderProfile();
            ShaderExtension = (IsUsingDx ? ".dx11" : ".ogl") + ".mgfxo";
        }

        public static bool IsUsingDx => ShaderProfile == 1;
        public static readonly string ShaderExtension;

        public static byte[] LoadShaderBytes(string name)
        {
            var fullname = "Bawx.Shaders." + name + ShaderExtension;
            var stream = typeof(EffectHelper).Assembly.GetManifestResourceStream(fullname);
            if (stream == null) throw new ArgumentException($"Cannot find effect with name {name}", nameof(name));

            using (var ms = new MemoryStream()) {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }

        #endregion


    }
}
