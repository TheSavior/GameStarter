using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shooter.Extensions;
using Shooter.Screens;

namespace Shooter.Components
{
	public class HudComponent : ScreenBase
	{
		SpriteFont font;
		Rectangle drawRectangle;

		ActionScreen screens;

		public HudComponent(ActionScreen screen)
		{
			this.screens = screen;

			drawRectangle = new Rectangle(
				0,
				0,
				Game.Window.ClientBounds.Width,
				Game.Window.ClientBounds.Height);
		}

		public override void LoadContent()
		{
			font = Game.Content.Load<SpriteFont>("HudFont");

			base.LoadContent();
		}

		public override void Draw(GameTime gameTime)
		{
			var str = string.Format("Player Size: {0}", screens.player.Scale.ToString("#.###"));

			Vector2 stringSize = font.MeasureString(str);
			Vector2 center = stringSize / 2;
			Vector2 right = new Vector2(stringSize.X, stringSize.Y / 2);
			Vector2 left = new Vector2(0, stringSize.Y / 2);
			Vector2 topRight = new Vector2(stringSize.X, 0);

			Vector2 align = topRight;

			var position = drawRectangle.TopRight();

			// Player Size
			Globals.SpriteBatch.DrawString(font, str, position, Color.LightGreen, 0, align, 1.0f, SpriteEffects.None, 0.5f);


			str = string.Format("Camera Zoom: {0}", screens.camera.Zoom.ToString("#.###"));
			var newStringSize = font.MeasureString(str);
			center = newStringSize / 2;
			right = new Vector2(newStringSize.X, newStringSize.Y / 2);
			left = new Vector2(0, newStringSize.Y / 2);
			topRight = new Vector2(newStringSize.X, 0);

			align = topRight;

			position = drawRectangle.TopRight() + new Vector2(0, stringSize.Y);
			Globals.SpriteBatch.DrawString(font, str, position, Color.LightGreen, 0, align, 1.0f, SpriteEffects.None, 0.5f);

			base.Draw(gameTime);
		}
	}
}
