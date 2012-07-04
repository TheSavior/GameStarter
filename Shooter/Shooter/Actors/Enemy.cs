using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter.Actors
{
	public class Enemy : DrawableComponent
	{
		// Animation representing the Enemy
		public Texture2D EnemyTexture;

		// The position of the enemy ship relative to the top left corner of the screen
		public Vector2 Position;

		// The amount of score the enemy will give to the player
		public float size;

		private float speed;

		private Random rand;

		public bool Active;

		public Rectangle BoundingBox
		{
			get
			{
				return new Rectangle(
					(int)Position.X - EnemyTexture.Width / 2,
					(int)Position.Y - EnemyTexture.Height / 2,
					EnemyTexture.Width,
					EnemyTexture.Height);
			}
		}

		public Enemy(float size)
		{
			this.size = size;
		}

		public void Initialize(Vector2 position)
		{
			// Set the position of the enemy
			Position = position;

			rand = new Random();
			speed = rand.Next(1, 5) / 2f;

			Active = true;

			base.Initialize();
		}

		public override void LoadContent()
		{
			EnemyTexture = Game.Content.Load<Texture2D>("enemy");

			base.LoadContent();
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
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			var origin = new Vector2(BoundingBox.Width / 2, BoundingBox.Height / 2);
			spriteBatch.Draw(EnemyTexture, Position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
		}
	}
}
