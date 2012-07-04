using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Game
	{
		public Game1()
		{
			Globals.Graphics = new GraphicsDeviceManager(this);
			Globals.Game = this;
			Content.RootDirectory = "Content";
		}

		protected override void Initialize()
		{
			Globals.ScreenManager = new ScreenManager();
			Globals.ScreenManager.Initialize();
			base.Initialize();
		}

		protected override void LoadContent()
		{
			Globals.ScreenManager.LoadContent();
			base.LoadContent();
		}

		protected override void UnloadContent()
		{
			Globals.ScreenManager.UnloadContent();
			base.UnloadContent();
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			Globals.ScreenManager.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
			base.Draw(gameTime);
			Globals.ScreenManager.Draw(gameTime);
		}
	}
}
