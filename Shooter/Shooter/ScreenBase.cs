using Microsoft.Xna.Framework;


namespace Shooter
{
	public abstract class ScreenBase : DrawableComponentManager
	{
		public ScreenBase()
		{
		}

		public override void Initialize()
		{
			base.Initialize();

			foreach (GameComponent component in Components)
			{
				if (component.Enabled == true)
				{
					component.Initialize();
				}
			}
		}

		public new virtual void LoadContent()
		{
			base.LoadContent();
		}

		public new virtual void UnloadContent()
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
