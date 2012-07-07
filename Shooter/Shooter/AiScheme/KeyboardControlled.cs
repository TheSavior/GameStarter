using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Shooter.Actors;

namespace Shooter.AiScheme
{
	public abstract class KeyboardControlled : ActorBase
	{
		public float max_speed;
		public float increase_speed;
		public float decrease_speed;

		public KeyboardControlled()
		{
			max_speed = 1f;
			increase_speed = .10f;
			decrease_speed = .05f;
		}

		public override void Update(GameTime gameTime)
		{
			if (!Active)
				return;

			ReadInput();
			UpdatePosition();

			base.Update(gameTime);
		}

		private void ReadInput()
		{
			// Use the Keyboard
			bool left = Globals.KeyManager.IsKeyDown(Keys.Left);
			bool right = Globals.KeyManager.IsKeyDown(Keys.Right);
			bool up = Globals.KeyManager.IsKeyDown(Keys.Up);
			bool down = Globals.KeyManager.IsKeyDown(Keys.Down);

			if (left) AddDirection(Direction.Left);
			if (right) AddDirection(Direction.Right);
			if (up) AddDirection(Direction.Up);
			if (down) AddDirection(Direction.Down);

			if (!right & !left)
			{
				slowDX();
			}
			if (!up & !down)
			{
				slowDY();
			}
		}

		private void UpdatePosition()
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

		private void AddDirection(Direction direction)
		{
			switch (direction)
			{
				case Direction.Right:
					Velocity.X += increase_speed;
					DrawDirection = Direction.Right;
					break;
				case Direction.Left:
					Velocity.X -= increase_speed;
					DrawDirection = Direction.Left;
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
		private void slowDX()
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
		private void slowDY()
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
