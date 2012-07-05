using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter.Actors
{
	class Player : ActorBase
	{
		// Velocity of the Player
		public Vector2 Velocity;

		private float increase_speed = .10f;
		private float decrease_speed = .05f;

		private float max_speed = 3f;

		public Player()
		{
			this.Active = true;
		}

		public override void LoadContent()
		{
			Texture = Game.Content.Load<Texture2D>("Fish");
			base.LoadContent();
		}

		public void Bigger()
		{
			Scale += .05f;
		}

		public void Smaller()
		{
			Scale -= .05f;
		}

		public override void Update(GameTime gameTime)
		{
			UpdatePosition();

			base.Update(gameTime);
		}

		public void Eat(float enemySize)
		{
			// We should grow at some ratio of the enemy size to our current size
			// First figure out what percent they are of us
			var length = BoundingVector.Length();

			Debug.WriteLine("Size Before: {0}", length);
			var percent = enemySize / length;
			var add = MathHelper.Lerp(0, 1, percent);

			Scale += add;

			Debug.WriteLine("Size After: {0}", BoundingVector.Length());
			Debug.WriteLine("");

		}

		public void UpdatePosition()
		{
			Position.X += Velocity.X;
			if (
				Position.X < BoundingBox.Width / 2 ||
				Position.X > Game.GraphicsDevice.Viewport.Width - BoundingBox.Width / 2)
			{
				Velocity.X = 0;
			}
			Position.X = MathHelper.Clamp(
				Position.X,
				0 + BoundingBox.Width / 2,
				Game.GraphicsDevice.Viewport.Width - BoundingBox.Width / 2);

			Position.Y += Velocity.Y;


			if (Position.Y < 0 + BoundingBox.Height / 2 ||
				Position.Y > Game.GraphicsDevice.Viewport.Height - BoundingBox.Height / 2)
			{
				Velocity.Y = 0;
			}
			Position.Y = MathHelper.Clamp(
				Position.Y,
				0 + BoundingBox.Height / 2,
				Position.Y = Game.GraphicsDevice.Viewport.Height - BoundingBox.Height / 2);
		}

		public void AddDirection(Direction direction)
		{
			switch (direction)
			{
				case Direction.Right:
					DrawDirection = Direction.Right;
					Velocity.X += increase_speed;
					break;
				case Direction.Left:
					DrawDirection = Direction.Left;
					Velocity.X -= increase_speed;
					break;
				case Direction.Up:
					Velocity.Y -= increase_speed;
					break;
				case Direction.Down:
					Velocity.Y += increase_speed;
					break;
			}

			Velocity.X = MathHelper.Clamp(Velocity.X, max_speed * -1, max_speed);
			Velocity.Y = MathHelper.Clamp(Velocity.Y, max_speed * -1, max_speed);
		}

		// Method called when you just let go of the X-direction
		//      keys. It will slow down the ball, giving the feel
		//      of momentum.
		public void slowDX()
		{
			if (Velocity.X > 0)
			{
				Velocity.X -= decrease_speed;
				Velocity.X = Math.Max(Velocity.X, 0);
			}
			else if (Velocity.X < 0)
			{
				Velocity.X += decrease_speed;
				Velocity.X = Math.Min(Velocity.X, 0);
			}
		}

		// Method called when you just let go of the Y-direction
		//      keys. It will slow down the ball, giving the feel
		//      of momentum.
		public void slowDY()
		{
			if (Velocity.Y > 0)
			{
				Velocity.Y -= decrease_speed;
				Velocity.Y = Math.Max(Velocity.Y, 0);
			}
			else if (Velocity.Y < 0)
			{
				Velocity.Y += decrease_speed;
				Velocity.Y = Math.Min(Velocity.Y, 0);
			}
		}
	}
}
