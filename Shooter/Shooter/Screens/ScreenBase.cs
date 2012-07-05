

namespace Shooter.Screens
{
	public abstract class ScreenBase : DrawableComponentManager
	{
		public ScreenBase()
		{
		}

		public override void LoadContent()
		{
			base.LoadContent();
		}

		public override void UnloadContent()
		{
			base.UnloadContent();
		}

		public virtual void Show()
		{

		}

		public virtual void Hide()
		{

		}
	}
}
