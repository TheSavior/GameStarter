using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter.Actors
{
	public class Enemy : ActorBase
	{
		private float max_speed = 2f;
		private float increase_speed = .10f;
		private float decrease_speed = .05f;

		private float speed;

		private Random rand;

		public Vector2 TargetVelocity;

		public Enemy()
		{
			rand = new Random();
		}

		public override void LoadContent()
		{
			Texture = Game.Content.Load<Texture2D>("enemy");
			base.LoadContent();
		}

		public override void Reset()
		{
			speed = rand.Next(1, 5) / 2f;

			Active = true;
			base.Reset();
		}

		public void AddDirection(Direction direction)
		{
			switch (direction)
			{
				case Direction.Right:
					TargetVelocity.X += increase_speed;
					break;
				case Direction.Left:
					TargetVelocity.X -= increase_speed;
					break;
				case Direction.Up:
					TargetVelocity.Y -= increase_speed;
					break;
				case Direction.Down:
					TargetVelocity.Y += increase_speed;
					break;
			}

			TargetVelocity.X = MathHelper.Clamp(TargetVelocity.X, max_speed * -1, max_speed);
			TargetVelocity.Y = MathHelper.Clamp(TargetVelocity.Y, max_speed * -1, max_speed);
		}

		public void SetSize(float size)
		{
			Scale = size;
		}

		public override void Update(GameTime gameTime)
		{
			var diff = TargetVelocity - Velocity;
			//var scaledDiff = diff * .05f;
			if (diff != Vector2.Zero)
			{
				Velocity += diff * .025f;
			}

			if (Velocity.X > 0)
			{
				DrawDirection = Direction.Right;
			}
			else if (Velocity.X < 0)
			{
				DrawDirection = Direction.Left;
			}

			// The enemy always moves to the left so decrement it's x position
			//Position.X -= speed;

			Position += Velocity;

			/*
			var target = TargetDirection;

			if (Position.X < 0 || Position.X > Game.GraphicsDevice.Viewport.Width)
			{
				target.X = -target.X;
			}

			if (Position.Y < 0 || Position.Y > Game.GraphicsDevice.Viewport.Width)
			{
				target.Y = -target.Y;
			}

			TargetDirection = target;
			*/

			base.Update(gameTime);
		}
	}
}
