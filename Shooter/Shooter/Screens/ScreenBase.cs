using Microsoft.Xna.Framework;


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
			this.Visible = true;
			this.Enabled = true;
			foreach (GameComponent component in Components)
			{
				component.Enabled = true;
				if (component is DrawableComponent)
				{
					((DrawableComponent)component).Visible = true;
				}
			}
		}

		public virtual void Hide()
		{
			this.Visible = false;
			this.Enabled = false;
			foreach (GameComponent component in Components)
			{
				component.Enabled = false;
				if (component is DrawableComponent)
				{
					((DrawableComponent)component).Visible = false;
				}
			}
		}

	}
}
