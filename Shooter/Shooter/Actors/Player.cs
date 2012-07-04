﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter.Actors
{
	class Player : DrawableComponent
	{
		public bool Active;

		// Animation representing the player
		public Texture2D PlayerTexture;

		// Position of the Player relative to the upper left side of the screen
		private Vector2 position;

		// Velocity of the Player
		public Vector2 Velocity;

		public float Size;

		private float increase_speed = .10f;
		private float decrease_speed = .05f;

		private float max_speed = 3f;

		public Rectangle BoundingBox
		{
			get
			{
				return new Rectangle(
					(int)position.X - PlayerTexture.Width / 2,
					(int)position.Y - PlayerTexture.Height / 2,
					PlayerTexture.Width,
					PlayerTexture.Height);
			}
		}

		private SpriteBatch spriteBatch;

		public Player()
		{
			this.Active = true;
			this.Size = 1;
		}

		/// <summary>
		/// Sets the center of the player
		/// </summary>
		/// <param name="position"></param>
		public void SetPosition(Vector2 position)
		{
			// Set the starting position of the player around the middle of the screen and to the back
			this.position = position;
		}

		public override void LoadContent()
		{
			PlayerTexture = Game.Content.Load<Texture2D>("Fish");
			spriteBatch = new SpriteBatch(Globals.Graphics.GraphicsDevice);
			base.LoadContent();
		}

		public override void Update(GameTime gameTime)
		{
			UpdatePosition();

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			// Center the player
			var origin = new Vector2(BoundingBox.Width / 2, BoundingBox.Height / 2);

			spriteBatch.Begin();
			spriteBatch.Draw(PlayerTexture, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
			spriteBatch.End();
		}

		public void Eat(float enemySize)
		{

		}

		public void UpdatePosition()
		{
			position.X += Velocity.X;
			if (
				position.X < 0 ||
				position.X > Game.GraphicsDevice.Viewport.Width - PlayerTexture.Width)
			{
				Velocity.X = 0;
			}
			position.X = MathHelper.Clamp(
				position.X,
				0,
				Game.GraphicsDevice.Viewport.Width - PlayerTexture.Width);

			position.Y += Velocity.Y;


			if (position.Y < 0 ||
				position.Y > Game.GraphicsDevice.Viewport.Height - PlayerTexture.Height)
			{
				Velocity.Y = 0;
			}
			position.Y = MathHelper.Clamp(
				position.Y,
				0,
				position.Y = Game.GraphicsDevice.Viewport.Height - PlayerTexture.Height);
		}

		public void AddDirection(Direction direction)
		{
			switch (direction)
			{
				case Direction.Right:
					Velocity.X += increase_speed;
					break;
				case Direction.Left:
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
