using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shooter.Screens;

namespace Shooter.Popups
{
	public class GameOverPopup : ScreenBase
	{
		Rectangle drawRectangle;
		Texture2D image;

		SpriteBatch spriteBatch;

		public GameOverPopup()
		{
			drawRectangle = new Rectangle(
				20,
				20,
				Game.Window.ClientBounds.Width - 40,
				Game.Window.ClientBounds.Height - 40);
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void LoadContent()
		{
			spriteBatch = new SpriteBatch(Globals.Graphics.GraphicsDevice);
			image = Content.Load<Texture2D>("alienmetal");
			base.LoadContent();
		}

		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			spriteBatch.Draw(image, drawRectangle, Color.White);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
