using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bawx.Rendering.Effects
{
    public class VoxelTracerEffect : Effect
    {

        #region Params

        private readonly EffectParameter _paletteParam;

        #endregion

        public VoxelTracerEffect(GraphicsDevice graphicsDevice) 
            : base(graphicsDevice, EffectHelper.LoadShaderBytes("voxelTracer"))
        {
            _paletteParam = Parameters["Palette"];
        }

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

        #region OnApply

        protected override void OnApply()
        {
            if (_paletteDirty)
            {
                _paletteParam.SetValue(Palette);
                _paletteDirty = false;
            }
        }

        #endregion
    }
}
