using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bawx
{
    public class VoxelEffect : Effect
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

        static VoxelEffect()
        {
            ShaderProfile = GetShaderProfile();
            ShaderExtension = IsUsingDx ? ".dx11" : ".ogl";
        }

        private static bool IsUsingDx => ShaderProfile == 1;
        private static readonly string ShaderExtension;

        private static byte[] LoadShaderBytes(string name) {
            var stream = typeof(VoxelEffect).Assembly.GetManifestResourceStream(name);
            using (var ms = new MemoryStream()) {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }

        #endregion

        #region Techniques

        public EffectTechnique BatchTechnique;
        public EffectTechnique InstancingTechnique;

        #endregion

        #region Params

        private readonly EffectParameter _worldParam;
        private readonly EffectParameter _viewParam;
        private readonly EffectParameter _projectionParam;

        private readonly EffectParameter _lightDirParam;
        private readonly EffectParameter _diffuseLightParam;
        private readonly EffectParameter _ambientLightParam;

        #endregion

        // TODO caching
        public Vector3 World;
        public Matrix View;
        public Matrix Projection;

        public Vector3 LightDirection = Vector3.Normalize(new Vector3(-1f,-1f,-0.6f));
        public Vector3 DiffuseLight = new Vector3(0.75f);
        public Color AmbientLight = new Color(0.35f, 0.35f, 0.35f);

        public VoxelEffect(GraphicsDevice graphicsDevice) : base(graphicsDevice, LoadShaderBytes($"Bawx.Shaders.voxelShader{ShaderExtension}.mgfxo"))
        {
            BatchTechnique = Techniques["Batch"];
            InstancingTechnique = Techniques["Instancing"];

            _worldParam = Parameters["World"];
            _viewParam = Parameters["View"];
            _projectionParam = Parameters["Projection"];

            _lightDirParam = Parameters["LightDirection"];
            _diffuseLightParam = Parameters["DiffuseLight"];
            _ambientLightParam = Parameters["AmbientLight"];
        }

        protected override void OnApply()
        {
            _worldParam?.SetValue(World);
            _viewParam?.SetValue(View);
            _projectionParam?.SetValue(Projection);

            _lightDirParam?.SetValue(LightDirection);
            _diffuseLightParam?.SetValue(DiffuseLight);
            _ambientLightParam?.SetValue(AmbientLight.ToVector3());
        }
    }
}