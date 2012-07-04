using Microsoft.Xna.Framework;

namespace Shooter
{
	public class DrawableComponent : DrawableGameComponent
	{
		public DrawableComponent()
			: base(Globals.Game)
		{

		}

		public new virtual void LoadContent()
		{
			base.LoadContent();
		}

		public new virtual void UnloadContent()
		{
			base.UnloadContent();
		}
	}
}
