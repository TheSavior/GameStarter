using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Shooter
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class MenuComponent : DrawableGameComponent
	{
		string[] menuItems;

		Color normal = Color.White;
		Color hilite = Color.Yellow;

		KeyboardState keyboardState;
		KeyboardState oldKeyboardState;

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

		public MenuComponent(Game game, string[] menuItems)
			: base(game)
		{
			this.menuItems = menuItems;
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
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

		private bool CheckKey(Keys theKey)
		{
			return keyboardState.IsKeyUp(theKey) &&
				oldKeyboardState.IsKeyDown(theKey);
		}

		public override void Update(GameTime gameTime)
		{
			keyboardState = Keyboard.GetState();

			if (CheckKey(Keys.Down))
			{
				_selectedIndex++;
			}
			else if (CheckKey(Keys.Up))
			{
				_selectedIndex--;
			}
			_selectedIndex = _selectedIndex % menuItems.Length;

			base.Update(gameTime);

			oldKeyboardState = keyboardState;
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
