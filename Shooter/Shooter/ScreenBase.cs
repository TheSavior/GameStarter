using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace Shooter
{
	public abstract class ScreenBase : DrawableComponentManager
	{
		public ScreenBase(Game game)
			: base(game)
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

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			foreach (GameComponent component in Components)
			{
				if (component.Enabled == true)
				{
					component.Update(gameTime);
				}
			}
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			foreach (GameComponent component in Components)
			{
				if (component is DrawableGameComponent &&
					((DrawableGameComponent)component).Visible)
				{
					((DrawableGameComponent)component).Draw(gameTime);
				}
			}
		}

		public virtual void Show()
		{
			this.Visible = true;
			this.Enabled = true;
			foreach (GameComponent component in Components)
			{
				component.Enabled = true;
				if (component is DrawableGameComponent)
				{
					((DrawableGameComponent)component).Visible = true;
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
				if (component is DrawableGameComponent)
				{
					((DrawableGameComponent)component).Visible = false;
				}
			}
		}

	}
}
