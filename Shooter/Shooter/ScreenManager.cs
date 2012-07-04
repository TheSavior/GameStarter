using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Shooter.Screens;

namespace Shooter
{
	public class ScreenManager
	{
		ScreenBase activeScreen;

		StartScreen startScreen;
		ActionScreen actionScreen;

		List<ScreenBase> screens;

		public ScreenManager()
		{
			startScreen = new StartScreen();
			actionScreen = new ActionScreen();

			screens = new List<ScreenBase>();
			screens.Add(startScreen);
			screens.Add(actionScreen);

			activeScreen = startScreen;
		}

		public void Initialize()
		{
			foreach (ScreenBase screen in screens)
			{
				screen.Initialize();
			}
		}

		public void LoadContent()
		{
			foreach (ScreenBase screen in screens)
			{
				screen.LoadContent();
			}
		}

		public void UnloadContent()
		{
			foreach (ScreenBase screen in screens)
			{
				screen.UnloadContent();
			}
		}

		public void Update(GameTime gameTime)
		{
			activeScreen.Update(gameTime);
		}

		public void Draw(GameTime gameTime)
		{
			activeScreen.Draw(gameTime);
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
