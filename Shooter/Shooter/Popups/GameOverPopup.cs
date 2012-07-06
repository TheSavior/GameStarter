using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shooter.Screens;

namespace Shooter.Popups
{
	public class GameOverPopup : ScreenBase
	{
		Rectangle drawRectangle;
		Texture2D image;

		SpriteFont font;

		ScreenBase owner;

		public GameOverPopup(ScreenBase owner)
		{
			this.owner = owner;

			var padding = 100;

			drawRectangle = new Rectangle(
				padding,
				padding,
				Game.Window.ClientBounds.Width - padding * 2,
				Game.Window.ClientBounds.Height - padding * 2);
		}

		public override void LoadContent()
		{
			image = Content.Load<Texture2D>("alienmetal");

			font = Game.Content.Load<SpriteFont>("gameFont");

			base.LoadContent();
		}

		public override void Update(GameTime gameTime)
		{
			if (Globals.KeyManager.IsKeyPress(Keys.Space))
			{
				// Reset
				owner.Reset();
			}

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			Globals.SpriteBatch.Draw(image, drawRectangle, Color.White);

			var str = "Game Over";

			Vector2 size = font.MeasureString(str);

			var position = new Vector2(
				(drawRectangle.Width - size.X) / 2 + drawRectangle.X,
				(drawRectangle.Height - size.Y) / 2 + drawRectangle.Y);

			Globals.SpriteBatch.DrawString(font, "Game Over", position, Color.Black);

			base.Draw(gameTime);
		}
	}
}
