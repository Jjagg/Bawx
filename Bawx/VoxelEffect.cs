using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;

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

        private readonly EffectParameter _paletteParam;

        private readonly EffectParameter _positionParam;
        private readonly EffectParameter _viewParam;
        private readonly EffectParameter _projectionParam;

        private readonly EffectParameter _lightDirParam;
        private readonly EffectParameter _diffuseLightParam;
        private readonly EffectParameter _ambientLightParam;

        #endregion


        #region Palette

        private bool _paletteDirty;

        private Vector4[] _palette;
        public Vector4[] Palette
        {
            get { return _palette; }
            set
            {
                if (value != _palette)
                {
                    _palette = value;
                    _paletteDirty = true;
                }
            }
        }

        #endregion

        #region Camera

        private bool _cameraDirty;

        private Vector3 _chunkPosition;
        public Vector3 ChunkPosition
        {
            get { return _chunkPosition; }
            set
            {

                if (_chunkPosition != value)
                {
                    _chunkPosition = value;
                    _cameraDirty = true;
                }
            }
        }

        private Matrix _view;
        public Matrix View
        {
            get { return _view; }
            set
            {
                if (_view != value)
                {
                    _view = value;
                    _cameraDirty = true;
                }
            }
        }

        private Matrix _projection;
        public Matrix Projection
        {
            get { return _projection; }
            set
            {
                if (_projection != value)
                {
                    _projection = value;
                    _cameraDirty = true;
                }
            }
        }

        #endregion

        #region Light
        private bool _lightDirty = true;

        private Vector3 _lightDirection = Vector3.Normalize(new Vector3(-1f, -1f, -0.6f));
        public Vector3 LightDirection
        {
            get { return _lightDirection; }
            set
            {
                if (_lightDirection != value)
                {
                    _lightDirection = value;
                    _lightDirty = true;
                }
            }
        }

        private Vector3 _diffuseLight = new Vector3(0.75f);
        public Vector3 DiffuseLight
        {
            get { return _diffuseLight; }
            set
            {
                if (_diffuseLight != value)
                {
                    _diffuseLight = value;
                    _lightDirty = true;
                }
            }
        }

        private Color _ambientLight = new Color(0.35f, 0.35f, 0.35f);
        public Color AmbientLight
        {
            get { return _ambientLight; }
            set
            {
                if (_ambientLight != value)
                {
                    _ambientLight = value;
                    _lightDirty = true;
                }
            }
        }

        #endregion

        #region ctor

        public VoxelEffect(GraphicsDevice graphicsDevice) : base(graphicsDevice, LoadShaderBytes($"Bawx.Shaders.voxelShader{ShaderExtension}.mgfxo"))
        {
            BatchTechnique = Techniques["Batch"];
            InstancingTechnique = Techniques["Instancing"];

            _paletteParam = Parameters["Palette"];

            _positionParam = Parameters["ChunkPosition"];
            _viewParam = Parameters["View"];
            _projectionParam = Parameters["Projection"];

            _lightDirParam = Parameters["LightDirection"];
            _diffuseLightParam = Parameters["DiffuseLight"];
            _ambientLightParam = Parameters["AmbientLight"];
        }

        #endregion

        #region OnApply

        protected override void OnApply()
        {
            if (_paletteDirty)
            {
                _paletteParam.SetValue(Palette);

                _paletteDirty = false;
            }

            if (_cameraDirty)
            {
                _positionParam?.SetValue(ChunkPosition);
                _viewParam?.SetValue(View);
                _projectionParam?.SetValue(Projection);

                _cameraDirty = false;
            }

            if (_lightDirty)
            {
                _lightDirParam?.SetValue(LightDirection);
                _diffuseLightParam?.SetValue(DiffuseLight);
                _ambientLightParam?.SetValue(AmbientLight.ToVector3());

                _lightDirty = false;
            }
        }

        #endregion
    }
}