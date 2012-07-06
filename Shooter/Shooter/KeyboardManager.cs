using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Shooter
{
	public class KeyboardManager : GameComponent
	{
		// Keyboard states used to determine key presses
		KeyboardState previousKeyboardState;
		KeyboardState currentKeyboardState;

		public KeyboardManager()
			: base(Globals.Game)
		{

		}

		public override void Update(GameTime gameTime)
		{
			if (currentKeyboardState != null)
			{
				previousKeyboardState = currentKeyboardState;
			}

			// Read the current state of the keyboard and gamepad and store it
			currentKeyboardState = Keyboard.GetState();

			if (previousKeyboardState == null)
			{
				previousKeyboardState = currentKeyboardState;
			}
		}

		public bool IsKeyPress(Keys key)
		{
			return IsKeyUp(key) && WasKeyDown(key);
		}

		public bool IsKeyUp(Keys key)
		{
			return currentKeyboardState.IsKeyUp(key);
		}

		public bool IsKeyDown(Keys key)
		{
			return currentKeyboardState.IsKeyDown(key);
		}

		public bool WasKeyUp(Keys key)
		{
			return previousKeyboardState.IsKeyUp(key);
		}

		public bool WasKeyDown(Keys key)
		{
			return previousKeyboardState.IsKeyDown(key);
		}
	}
}
