using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Shooter.Screens;

namespace Shooter
{
	public class ScreenManager : DrawableComponentManager
	{
		ScreenBase activeScreen;
		StartScreen startScreen;
		ActionScreen actionScreen;

		public ScreenManager(Game game, SpriteBatch spriteBatch)
			: base(game, spriteBatch)
		{
		}

		public override void Initialize()
		{
			base.Initialize();

			startScreen = new StartScreen(
				this.Game,
				spriteBatch,
				Content.Load<SpriteFont>("gameFont"),
				Content.Load<Texture2D>("alienmetal"));
			Game.Components.Add(startScreen);
			startScreen.Initialize();
			startScreen.Hide();

			actionScreen = new ActionScreen(
				this.Game,
				spriteBatch,
				Content.Load<Texture2D>("greenmetal"));
			Components.Add(actionScreen);
			actionScreen.Initialize();
			actionScreen.Hide();

			activeScreen = startScreen;
			activeScreen.Show();
		}

		public override void Draw(GameTime gameTime)
		{
			activeScreen.Draw(gameTime);
			base.Draw(gameTime);
		}

		public void Navigate(Screen screen) 
		{
			activeScreen.Hide();
			switch (screen)
			{
				case Screen.Game:
					activeScreen = actionScreen;
					break;
				case Screen.Start:
					activeScreen = startScreen;
					break;
			}
			activeScreen.Show();
		}
	}
}
