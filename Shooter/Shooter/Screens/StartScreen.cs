using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shooter.Components;

namespace Shooter.Screens
{
	class StartScreen : ScreenBase
	{
		MenuComponent menuComponent;
		Texture2D image;
		Rectangle imageRectangle;

		SpriteBatch spriteBatch;

		public StartScreen()
		{
			string[] menuItems = { "Start Game", "End Game" };

			menuComponent = new MenuComponent(menuItems);
			Components.Add(menuComponent);

			imageRectangle = new Rectangle(
				0,
				0,
				Game.Window.ClientBounds.Width,
				Game.Window.ClientBounds.Height);
		}

		public override void LoadContent()
		{
			spriteBatch = new SpriteBatch(Globals.Graphics.GraphicsDevice);

			image = Content.Load<Texture2D>("alienmetal");

			base.LoadContent();
		}

		public override void Update(GameTime gameTime)
		{
			if (Globals.KeyManager.IsKeyPress(Keys.Enter))
			{
				if (menuComponent.SelectedIndex == 0)
				{
					Globals.ScreenManager.Navigate(Screen.Game);
				}
				else if (menuComponent.SelectedIndex == 1)
				{
					this.Game.Exit();
				}
			}

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			spriteBatch.Draw(image, imageRectangle, Color.White);
			spriteBatch.End();
			base.Draw(gameTime);
		}
	}
}
