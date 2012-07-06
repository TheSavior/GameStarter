using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Shooter
{
	public abstract class DrawableComponentManager : DrawableComponent
	{
		protected ContentManager Content;

		public List<GameComponent> Components
		{
			get;
			protected set;
		}

		public DrawableComponentManager()
		{
			Components = new List<GameComponent>();
			Content = Game.Content;
		}

		public override void Initialize()
		{
			base.Initialize();

			foreach (GameComponent component in Components)
			{
				component.Initialize();
			}
		}

		public override void LoadContent()
		{
			base.LoadContent();

			foreach (GameComponent component in Components)
			{
				if (component is DrawableComponent)
				{
					((DrawableComponent)component).LoadContent();
				}
			}

			Reset();
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
				if (component is DrawableComponent &&
					((DrawableComponent)component).Visible)
				{
					((DrawableComponent)component).Draw(gameTime);
				}
			}
		}

		public override void Reset()
		{
			foreach (GameComponent component in Components)
			{
				if (component is DrawableComponent)
				{
					((DrawableComponent)component).Reset();
				}
			}
		}
	}
}
