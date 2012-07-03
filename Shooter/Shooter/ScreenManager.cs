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

		public ScreenManager(Game game)
			: base(game)
		{
			startScreen = new StartScreen(this.Game);
			startScreen.Hide();
			Components.Add(startScreen);

			actionScreen = new ActionScreen(this.Game);
			actionScreen.Hide();
			Components.Add(actionScreen);

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
