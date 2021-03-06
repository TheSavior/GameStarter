﻿using System;
using Microsoft.Xna.Framework;

namespace Shooter.AiScheme
{
	public class Wander : AiSchemeBase
	{
		private float speedUpStep;

		private Vector2 TargetVelocity;

		private Random rand;

		public Wander(float speedUpStep)
		{
			this.speedUpStep = speedUpStep;

			rand = new Random();
		}

		public override void Reset()
		{
			Direction[] values = (Direction[])Enum.GetValues(typeof(Direction));
			var direction = values[rand.Next(0, values.Length)];

			AddDirection(direction);

			base.Reset();
		}

		private int counter;
		public override void Update()
		{
			if (counter == 0)
			{
				Direction[] values = (Direction[])Enum.GetValues(typeof(Direction));
				var direction = values[rand.Next(0, values.Length)];

				AddDirection(direction);
				counter = rand.Next(80, 300);
			}

			counter--;

			Actor.BoundPosition();

			var diff = TargetVelocity - Actor.Velocity;
			if (diff != Vector2.Zero)
			{
				Actor.Velocity += diff * .025f;
			}

			if (Actor.Velocity.X > 0)
			{
				Actor.DrawDirection = Direction.Right;
			}
			else if (Actor.Velocity.X < 0)
			{
				Actor.DrawDirection = Direction.Left;
			}

			base.Update();
		}

		private void AddDirection(Direction direction)
		{
			switch (direction)
			{
				case Direction.Right:
					TargetVelocity.X += speedUpStep;
					break;
				case Direction.Left:
					TargetVelocity.X -= speedUpStep;
					break;
				case Direction.Up:
					TargetVelocity.Y -= speedUpStep;
					break;
				case Direction.Down:
					TargetVelocity.Y += speedUpStep;
					break;
			}

			TargetVelocity.X = MathHelper.Clamp(TargetVelocity.X, Actor.MaxSpeed * -1, Actor.MaxSpeed);
			TargetVelocity.Y = MathHelper.Clamp(TargetVelocity.Y, Actor.MaxSpeed * -1, Actor.MaxSpeed);
		}
	}
}
