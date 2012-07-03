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

		SpriteBatch spriteBatch;

		public StartScreen(Game game)
			: base(game)
		{
			string[] menuItems = { "Start Game", "End Game" };
			
			menuComponent = new MenuComponent(game, menuItems);
			Components.Add(menuComponent);
			
			imageRectangle = new Rectangle(
				0,
				0,
				Game.Window.ClientBounds.Width,
				Game.Window.ClientBounds.Height);
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			image = Content.Load<Texture2D>("alienmetal");

			base.LoadContent();
		}

		public override void Update(GameTime gameTime)
		{
			// TODO: Add your update logic here
			keyboardState = Keyboard.GetState();

			if (CheckKey(Keys.Enter))
			{
				if (menuComponent.SelectedIndex == 0)
				{
					Game1.screenManager.Navigate(Screen.Game);
				}
				else if (menuComponent.SelectedIndex == 1)
				{
					this.Game.Exit();
				}
			}
 
			base.Update(gameTime);

			oldKeyboardState = keyboardState;
		}

		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			spriteBatch.Draw(image, imageRectangle, Color.White);
			spriteBatch.End();
			base.Draw(gameTime);
		}

		private bool CheckKey(Keys theKey)
		{
			return keyboardState.IsKeyUp(theKey) &&
				oldKeyboardState.IsKeyDown(theKey);
		}
	}

}
