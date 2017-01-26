using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace VoxViewer
{
    public class Input : GameComponent
    {
        private static KeyboardState _oldState;
        private static KeyboardState _currentState;

        public Input(Game game) : base(game)
        {
            _currentState = Keyboard.GetState();
        }

        public override void Update(GameTime gameTime)
        {
            _oldState = _currentState;
            _currentState = Keyboard.GetState();
        }

        public static bool IsDown(Keys key)
        {
            return _currentState.IsKeyDown(key);
        }

        public static bool IsUp(Keys key)
        {
            return _currentState.IsKeyUp(key);
        }

        public static bool IsPressed(Keys key)
        {
            return _oldState.IsKeyUp(key) && _currentState.IsKeyDown(key);
        }
    }
}
