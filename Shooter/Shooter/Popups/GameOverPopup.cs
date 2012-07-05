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

		SpriteFont font;

		public GameOverPopup()
		{
			var padding = 100;
			drawRectangle = new Rectangle(
				padding,
				padding,
				Game.Window.ClientBounds.Width - padding * 2,
				Game.Window.ClientBounds.Height - padding * 2);
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void LoadContent()
		{
			spriteBatch = new SpriteBatch(Globals.Graphics.GraphicsDevice);
			image = Content.Load<Texture2D>("alienmetal");

			font = Game.Content.Load<SpriteFont>("gameFont");

			base.LoadContent();
		}

		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			spriteBatch.Draw(image, drawRectangle, Color.White);

			var str = "Game Over";

			Vector2 size = font.MeasureString(str);

			var position = new Vector2(
				(drawRectangle.Width - size.X) / 2 + drawRectangle.X,
				(drawRectangle.Height - size.Y) / 2 + drawRectangle.Y);

			spriteBatch.DrawString(font, "Game Over", position, Color.Black);

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
