using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter.Actors
{
	public class Enemy : ActorBase
	{
		private float speed;

		private Random rand;

		public override void Initialize()
		{
			rand = new Random();
			speed = rand.Next(1, 5) / 2f;

			Active = true;

			base.Initialize();
		}

		public override void LoadContent()
		{
			Texture = Game.Content.Load<Texture2D>("enemy");

			base.LoadContent();
		}

		public void SetSize(float size)
		{
			Scale = size;
		}

		public override void Update(GameTime gameTime)
		{
			// The enemy always moves to the left so decrement it's x position
			Position.X -= speed;

			// If the enemy is past the screen then deactivate it
			if (Position.X < -BoundingBox.Width)
			{
				// By setting the Active flag to false, the game will remove this objet from the
				// active game list
				Active = false;
			}

			base.Update(gameTime);
		}
	}
}
