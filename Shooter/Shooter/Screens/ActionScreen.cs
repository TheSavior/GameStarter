using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shooter.Actors;

namespace Shooter.Screens
{
	class ActionScreen : ScreenBase
	{
		Texture2D image;
		Rectangle imageRectangle;

		// Represents the player 
		Player player;

		// Enemies
		List<Enemy> enemies;

		// The rate at which the enemies appear
		TimeSpan enemySpawnTime;
		TimeSpan previousSpawnTime;

		// A random number generator
		Random random;

		// Keyboard states used to determine key presses
		KeyboardState currentKeyboardState;

		SpriteBatch spriteBatch;

		public ActionScreen()
		{
			imageRectangle = new Rectangle(
				0,
				0,
				Globals.Game.Window.ClientBounds.Width,
				Game.Window.ClientBounds.Height);

			// Set the time keepers to zero
			previousSpawnTime = TimeSpan.Zero;

			// Used to determine how fast enemy respawns
			enemySpawnTime = TimeSpan.FromSeconds(1.0f);

			// Initialize our random number generator
			random = new Random();

			// Initialize the enemies list
			enemies = new List<Enemy>();
		}


		public override void LoadContent()
		{
			spriteBatch = new SpriteBatch(Globals.Graphics.GraphicsDevice);
			image = Game.Content.Load<Texture2D>("greenmetal");

			Vector2 playerPosition = new Vector2(0, Globals.Graphics.GraphicsDevice.Viewport.Height / 2);

			// Initialize the player class
			player = new Player(this.Game, playerPosition);
			Components.Add(player);

			base.LoadContent();
		}

		public override void Update(GameTime gameTime)
		{
			// Read the current state of the keyboard and gamepad and store it
			currentKeyboardState = Keyboard.GetState();

			// Update the player
			UpdatePlayer(gameTime);

			// Update the enemies
			UpdateEnemies(gameTime);

			// Update the collision
			UpdateCollision();

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			spriteBatch.Begin();
			// Draw the Enemies
			for (int i = 0; i < enemies.Count; i++)
			{
				enemies[i].Draw(spriteBatch);
			}
			spriteBatch.End();
		}

		private void AddEnemy()
		{
			// Create an enemy
			Enemy enemy = new Enemy(1);

			// Randomly generate the position of the enemy
			var viewport = Globals.Graphics.GraphicsDevice.Viewport;
			Vector2 position = new Vector2(viewport.Width, random.Next(100, viewport.Height - 100));

			// Initialize the enemy
			enemy.Initialize(position);
			enemy.LoadContent();

			// Add the enemy to the active enemies list
			enemies.Add(enemy);
		}

		private void UpdatePlayer(GameTime gameTime)
		{
			// Use the Keyboard
			bool left = currentKeyboardState.IsKeyDown(Keys.Left);
			bool right = currentKeyboardState.IsKeyDown(Keys.Right);
			bool up = currentKeyboardState.IsKeyDown(Keys.Up);
			bool down = currentKeyboardState.IsKeyDown(Keys.Down);

			if (left) player.AddDirection(Direction.Left);
			if (right) player.AddDirection(Direction.Right);
			if (up) player.AddDirection(Direction.Up);
			if (down) player.AddDirection(Direction.Down);

			if (!right & !left)
			{
				player.slowDX();
			}
			if (!up & !down)
			{
				player.slowDY();
			}
		}

		private void UpdateEnemies(GameTime gameTime)
		{
			// Spawn a new enemy enemy every 1.5 seconds
			if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
			{
				previousSpawnTime = gameTime.TotalGameTime;

				// Add an Enemy
				AddEnemy();
			}

			// Update the Enemies
			for (int i = enemies.Count - 1; i >= 0; i--)
			{
				enemies[i].Update(gameTime);

				if (enemies[i].Active == false)
				{
					enemies.RemoveAt(i);
				}
			}
		}

		private void UpdateCollision()
		{
			// Use the Rectangle's built-in intersect function to 
			// determine if two objects are overlapping
			Rectangle rectangle1;
			Rectangle rectangle2;

			// Only create the rectangle once for the player
			rectangle1 = player.Bounds;

			// Do the collision between the player and the enemies
			for (int i = 0; i < enemies.Count; i++)
			{
				rectangle2 = new Rectangle((int)enemies[i].Position.X,
				(int)enemies[i].Position.Y,
				enemies[i].ScaledWidth,
				enemies[i].ScaledHeight);

				// Determine if the two objects collided with each
				// other
				if (rectangle1.Intersects(rectangle2))
				{
					// Collision, either game over or success eating
					if (enemies[i].size <= player.Size)
					{
						player.Eat(enemies[i].size);
						enemies[i].Active = false;
					}
					else
					{
						player.Active = false;
					}
				}
			}
		}
	}

}
