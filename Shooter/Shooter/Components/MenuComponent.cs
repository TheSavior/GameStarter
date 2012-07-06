using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Shooter.Components
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class MenuComponent : DrawableComponent
	{
		string[] menuItems;

		Color normal = Color.White;
		Color hilite = Color.Yellow;

		SpriteBatch spriteBatch;
		SpriteFont spriteFont;

		Vector2 position;
		float width = 0f;
		float height = 0f;

		int _selectedIndex;
		public int SelectedIndex
		{
			get { return _selectedIndex; }
			set
			{
				_selectedIndex = (int)MathHelper.Clamp(value, 0, menuItems.Length - 1);
			}
		}

		public MenuComponent(string[] menuItems)
		{
			this.menuItems = menuItems;
		}

		public override void LoadContent()
		{
			spriteBatch = new SpriteBatch(Globals.Graphics.GraphicsDevice);
			spriteFont = Game.Content.Load<SpriteFont>("gameFont");
			MeasureMenu();

			base.LoadContent();
		}

		private void MeasureMenu()
		{
			height = 0;
			width = 0;
			foreach (string item in menuItems)
			{
				Vector2 size = spriteFont.MeasureString(item);
				if (size.X > width)
				{
					width = size.X;
				}
				height += spriteFont.LineSpacing + 5;
			}

			position = new Vector2(
				(Game.Window.ClientBounds.Width - width) / 2,
				(Game.Window.ClientBounds.Height - height) / 2);
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void Update(GameTime gameTime)
		{

			if (Globals.KeyManager.IsKeyPress(Keys.Down))
			{
				_selectedIndex++;
			}
			else if (Globals.KeyManager.IsKeyPress(Keys.Up))
			{
				_selectedIndex--;
			}
			_selectedIndex = _selectedIndex % menuItems.Length;
			// This can happen with mod because it is in the range -x to x
			// not true mod (remainder in c# and c++)
			if (_selectedIndex < 0)
			{
				_selectedIndex += menuItems.Length;
			}

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			Vector2 location = position;
			Color tint;

			spriteBatch.Begin();
			for (int i = 0; i < menuItems.Length; i++)
			{
				if (i == _selectedIndex)
				{
					tint = hilite;
				}
				else
				{
					tint = normal;
				}

				spriteBatch.DrawString(
					spriteFont,
					menuItems[i],
					location,
					tint);
				location.Y += spriteFont.LineSpacing + 5;
			}
			spriteBatch.End();
		}
	}
}
