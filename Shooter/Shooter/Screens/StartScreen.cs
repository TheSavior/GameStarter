using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Shooter.Screens
{
	class StartScreen : ScreenBase
	{
		MenuComponent menuComponent;
		Texture2D image;
		Rectangle imageRectangle;

		KeyboardState oldKeyboardState;
		KeyboardState keyboardState;

		public StartScreen(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont, Texture2D image)
			: base(game, spriteBatch)
		{
			string[] menuItems = { "Start Game", "End Game" };
			menuComponent = new MenuComponent(game,
				spriteBatch,
				spriteFont,
				menuItems);
			Components.Add(menuComponent);
			
			this.image = image;
			imageRectangle = new Rectangle(
				0,
				0,
				Game.Window.ClientBounds.Width,
				Game.Window.ClientBounds.Height);
		}

		public override void Update(GameTime gameTime)
		{
			// TODO: Add your update logic here
			keyboardState = Keyboard.GetState();

			if (CheckKey(Keys.Enter))
			{
				if (SelectedIndex == 0)
				{
					Game1.screenManager.Navigate(Screen.Game);
				}
				else if (SelectedIndex == 1)
				{
					this.Game.Exit();
				}
			}
 
			base.Update(gameTime);

			oldKeyboardState = keyboardState;
		}

		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Draw(image, imageRectangle, Color.White);
			base.Draw(gameTime);
		}

		private bool CheckKey(Keys theKey)
		{
			return keyboardState.IsKeyUp(theKey) &&
				oldKeyboardState.IsKeyDown(theKey);
		}
	}

}
