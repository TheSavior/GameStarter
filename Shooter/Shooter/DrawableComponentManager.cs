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
	public abstract class DrawableComponentManager : DrawableGameComponent
	{
		protected SpriteBatch spriteBatch;
		protected ContentManager Content;

		public List<GameComponent> Components
		{
			get;
			protected set;
		}

		public DrawableComponentManager(Game game, SpriteBatch spriteBatch)
			: base(game)
		{
			Components = new List<GameComponent>();
			Content = Game.Content;
			this.spriteBatch = spriteBatch;
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
	}
}
