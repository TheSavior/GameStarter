using Microsoft.Xna.Framework;

namespace Shooter
{
	public abstract class DrawableComponent : DrawableGameComponent
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

		public virtual void Reset()
		{

		}
	}
}
