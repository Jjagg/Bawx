using Bawx.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bawx
{
    public class VoxelEffect : Effect
    {

        #region Techniques

        public EffectTechnique BatchTechnique;
        public EffectTechnique InstancingTechnique;
        public EffectTechnique MeshTechnique;
        public EffectTechnique InstancingDepthTechnique;
        public EffectTechnique InstancingWithShadowTechnique;
        public EffectTechnique InstancingShadowMapTechnique;

        #endregion

        #region Params

        private readonly EffectParameter _paletteParam;

        private readonly EffectParameter _positionParam;
        private readonly EffectParameter _viewParam;
        private readonly EffectParameter _projectionParam;

        private readonly EffectParameter _dirLightMatrixParam;
        private readonly EffectParameter _lightDirParam;
        private readonly EffectParameter _diffuseLightParam;
        private readonly EffectParameter _ambientLightParam;

        private readonly EffectParameter _shadowMapParam;

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

        private bool _cameraDirty = true;
        private bool _lightMatrixDirty = true;

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

        private Matrix _view = Matrix.Identity;
        public Matrix View
        {
            get { return _view; }
            set
            {
                if (_view != value)
                {
                    _view = value;
                    _cameraDirty = true;
                    _lightMatrixDirty = true;
                }
            }
        }

        private Matrix _projection = Matrix.Identity;
        public Matrix Projection
        {
            get { return _projection; }
            set
            {
                if (_projection != value)
                {
                    _projection = value;
                    _cameraDirty = true;
                    _lightMatrixDirty = true;
                }
            }
        }

        #endregion

        #region Light

        /// <summary>
        /// Keep at false to automatically compute the light View Projection matrix based on camera frustrum.
        /// If you manually want to set the lights matrix for more precision set to true.
        /// </summary>
        public bool ManualLightMatrix = false;

        private bool _lightDirty = true;

        private Vector3 _lightDirection = Vector3.Normalize(new Vector3(-1f, -0.6f, -0.4f));
        public Vector3 LightDirection
        {
            get { return _lightDirection; }
            set
            {
                if (_lightDirection != value)
                {
                    _lightDirection = value;
                    _lightDirty = true;
                    _lightMatrixDirty = true;
                }
            }
        }

        private Vector3 _diffuseLight = new Vector3(0.65f);
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

        private Color _ambientLight = new Color(0.4f, 0.4f, 0.4f);
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

        #region ShadowMap

        private bool _shadowMapDirty;

        private RenderTarget2D _shadowMap;
        public RenderTarget2D ShadowMap
        {
            get { return _shadowMap; }
            set
            {
                if (_shadowMap != value)
                {
                    _shadowMap = value;
                    _shadowMapDirty = true;
                }
            }
        }

        #endregion

        #region ctor

        public VoxelEffect(GraphicsDevice graphicsDevice) 
            : base(graphicsDevice, EffectHelper.LoadShaderBytes("voxelShader"))
        {
            BatchTechnique = Techniques["Batch"];
            InstancingTechnique = Techniques["Instancing"];
            MeshTechnique = Techniques["Mesh"];
            InstancingDepthTechnique = Techniques["InstancingDepth"];
            InstancingWithShadowTechnique = Techniques["InstancingWithShadow"];
            InstancingShadowMapTechnique = Techniques["InstancingShadowMap"];

            _paletteParam = Parameters["Palette"];

            _positionParam = Parameters["ChunkPosition"];
            _viewParam = Parameters["View"];
            _projectionParam = Parameters["Projection"];

            _dirLightMatrixParam = Parameters["DirectionalLightMatrix"];
            _lightDirParam = Parameters["LightDirection"];
            _diffuseLightParam = Parameters["DiffuseLight"];
            _ambientLightParam = Parameters["AmbientLight"];

            _shadowMapParam = Parameters["ShadowMap"];

            Parameters["Normals"].SetValue(CubeBuilder.GetNormals());
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

            if (!ManualLightMatrix && _lightMatrixDirty)
            {
                var mat = LightDirection.GetViewProjectionMatrix(new BoundingFrustum(_view*_projection));
                _dirLightMatrixParam?.SetValue(mat);
                _lightMatrixDirty = false;
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

            if (_shadowMapDirty)
            {
                _shadowMapParam?.SetValue(_shadowMap);
                _shadowMapDirty = false;
            }
        }

        #endregion
    }
}