﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shooter.Screens;

namespace Shooter.Actors
{
	public class ActorBase : DrawableComponentManager
	{
		public float MaxSpeed;

		// Velocity of the Actor
		public Vector2 Velocity;

		public bool Active;

		// Animation representing the player
		public Texture2D Texture;

		// Position of the center of the Actor
		// relative to the upper left side of the screen
		public Vector2 Position;

		// Width of the actor in cm
		public float Width
		{
			get;
			set;
		}

		public Direction DrawDirection
		{
			get;
			set;
		}

		public RectangleF BoundingBox
		{
			get
			{
				var aspectRatio = (float)Texture.Width / Texture.Height;

				var width = Width * Texture.Width;
				var height = width / aspectRatio;

				return new RectangleF(
					(Position.X - width / 2),
					(Position.Y - height / 2),
					(width),
					(height)
					);
			}
		}

		public Vector2 BoundingVector
		{
			get
			{
				var aspectRatio = Width / Texture.Width;
				var height = Texture.Height * aspectRatio;

				return new Vector2(
					Position.X - Width / 2,
					Position.Y - height / 2);
			}
		}

		public override void Reset()
		{
			Active = true;
			Enabled = true;
			Width = 1f;
			DrawDirection = Direction.Right;

			base.Reset();
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteEffects effect = SpriteEffects.None;
			if (DrawDirection == Direction.Left)
			{
				effect = SpriteEffects.FlipHorizontally;
			}

			var origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
			Globals.SpriteBatch.Draw(Texture, Position, null, Color.White, 0, origin, Width, effect, 0);
			base.Draw(gameTime);
		}

		public void BoundPosition()
		{
			var leftBound = ActionScreen.WorldRectangle.Left + BoundingBox.Width / 2;
			var rightBound = ActionScreen.WorldRectangle.Right - BoundingBox.Width / 2;
			var topBound = ActionScreen.WorldRectangle.Top + BoundingBox.Height / 2;
			var bottomBound = ActionScreen.WorldRectangle.Bottom - BoundingBox.Height / 2;

			Position.X += Velocity.X;
			if (Position.X < leftBound || Position.X > rightBound)
			{
				Velocity.X = 0;
			}

			Position.X = MathHelper.Clamp(Position.X, leftBound, rightBound);

			Position.Y += Velocity.Y;
			if (Position.Y < topBound || Position.Y > bottomBound)
			{
				Velocity.Y = 0;
			}

			Position.Y = MathHelper.Clamp(Position.Y, topBound, bottomBound);
		}
	}
}
