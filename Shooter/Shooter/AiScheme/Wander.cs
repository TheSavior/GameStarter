using System;
using Microsoft.Xna.Framework;

namespace Shooter.AiScheme
{
	public class Wander : AiSchemeBase
	{
		private float max_speed;
		private float increase_speed;

		private float speed;

		private Vector2 TargetVelocity;

		private Random rand;

		public Wander()
		{
			max_speed = 2f;
			increase_speed = .10f;

			rand = new Random();
			speed = rand.Next(1, 5) / 2f;
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

			Actor.Position += Actor.Velocity;


			var target = TargetVelocity;

			if (Actor.Position.X < 0 || Actor.Position.X > Globals.Graphics.GraphicsDevice.Viewport.Width)
			{
				target.X = -target.X;
			}

			if (Actor.Position.Y < 0 || Actor.Position.Y > Globals.Graphics.GraphicsDevice.Viewport.Width)
			{
				target.Y = -target.Y;
			}

			TargetVelocity = target;


			base.Update();
		}

		private void AddDirection(Direction direction)
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
	}
}
