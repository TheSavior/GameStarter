using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
	public class Enemy : DrawableGameComponent
	{
		// Animation representing the Enemy
		public Texture2D EnemyTexture;

		// The position of the enemy ship relative to the top left corner of thescreen
		public Vector2 Position;

		// The amount of score the enemy will give to the player
		public float size;

		private float speed;

		private Random rand;

		public bool Active;

		// Get the width of the enemy ship


		public int ScaledWidth
		{
			get { return EnemyTexture.Width * size; }
		}

		// Get the height of the enemy ship
		public int ScaledHeight
		{
			get { return EnemyTexture.Height * size; }
		}

		public Enemy(Game game, float size)
			: base(game)
		{
			this.size = size;
		}

		public void Initialize(Texture2D texture, Vector2 position)
		{
			EnemyTexture = texture;

			// Set the position of the enemy
			Position = position;

			rand = new Random();
			speed = rand.Next(1, 5) / 2f;

			Active = true;
		}

		public override void Update(GameTime gameTime)
		{
			// The enemy always moves to the left so decrement it's xposition
			Position.X -= speed;

			// If the enemy is past the screen then deactivateit
			if (Position.X < -ScaledWidth)
			{
				// By setting the Active flag to false, the game will remove this objet fromthe 
				// active game list
				Active = false;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(EnemyTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}


	}
}
