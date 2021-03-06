﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Shooter.AiScheme
{
	public class KeyboardControlled : AiSchemeBase
	{
		public float SpeedUpStep;
		public float SpeedDownStep;

		public KeyboardControlled(float speedUpStep, float speedDownStep)
		{
			SpeedUpStep = speedUpStep;
			SpeedDownStep = speedDownStep;
		}

		public override void Update()
		{
			ReadInput();
			Actor.BoundPosition();

			base.Update();
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

		private void AddDirection(Direction direction)
		{
			switch (direction)
			{
				case Direction.Right:
					Actor.Velocity.X += SpeedUpStep;
					Actor.DrawDirection = Direction.Right;
					break;
				case Direction.Left:
					Actor.Velocity.X -= SpeedUpStep;
					Actor.DrawDirection = Direction.Left;
					break;
				case Direction.Up:
					Actor.Velocity.Y -= SpeedUpStep;
					break;
				case Direction.Down:
					Actor.Velocity.Y += SpeedUpStep;
					break;
			}

			Actor.Velocity.X = MathHelper.Clamp(Actor.Velocity.X, Actor.MaxSpeed * -1, Actor.MaxSpeed);
			Actor.Velocity.Y = MathHelper.Clamp(Actor.Velocity.Y, Actor.MaxSpeed * -1, Actor.MaxSpeed);
		}

		// Method called when you just let go of the X-direction
		//      keys. It will slow down the ball, giving the feel
		//      of momentum.
		private void slowDX()
		{
			if (Actor.Velocity.X > 0)
			{
				Actor.Velocity.X -= SpeedDownStep;
				Actor.Velocity.X = Math.Max(Actor.Velocity.X, 0);
			}
			else if (Actor.Velocity.X < 0)
			{
				Actor.Velocity.X += SpeedDownStep;
				Actor.Velocity.X = Math.Min(Actor.Velocity.X, 0);
			}
		}

		// Method called when you just let go of the Y-direction
		//      keys. It will slow down the ball, giving the feel
		//      of momentum.
		private void slowDY()
		{
			if (Actor.Velocity.Y > 0)
			{
				Actor.Velocity.Y -= SpeedDownStep;
				Actor.Velocity.Y = Math.Max(Actor.Velocity.Y, 0);
			}
			else if (Actor.Velocity.Y < 0)
			{
				Actor.Velocity.Y += SpeedDownStep;
				Actor.Velocity.Y = Math.Min(Actor.Velocity.Y, 0);
			}
		}


	}
}
