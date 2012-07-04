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
			Globals.ScreenManager.Update(gameTime);
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
			Globals.ScreenManager.Draw(gameTime);
			base.Draw(gameTime);
		}
	}
}
