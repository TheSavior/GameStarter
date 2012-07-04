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
		KeyboardState previousKeyboardState;
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
			enemySpawnTime = TimeSpan.FromSeconds(0.5f);

			// Initialize our random number generator
			random = new Random();

			// Initialize the enemies list
			enemies = new List<Enemy>();
		}


		public override void LoadContent()
		{
			spriteBatch = new SpriteBatch(Globals.Graphics.GraphicsDevice);

			image = Game.Content.Load<Texture2D>("greenmetal");

			// Initialize the player class
			player = new Player();
			Components.Add(player);

			base.LoadContent();
			Vector2 playerPosition = new Vector2(0 + player.BoundingBox.Width / 2, Globals.Graphics.GraphicsDevice.Viewport.Height / 2);
			player.SetPosition(playerPosition);
		}

		public override void Update(GameTime gameTime)
		{
			// Read the current state of the keyboard and gamepad and store it
			currentKeyboardState = Keyboard.GetState();

			if (previousKeyboardState == null)
			{
				previousKeyboardState = currentKeyboardState;
			}

			// Update the player
			UpdatePlayer(gameTime);

			// Update the enemies
			UpdateEnemies(gameTime);

			// Update the collision
			UpdateCollision();

			base.Update(gameTime);

			previousKeyboardState = currentKeyboardState;
		}

		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			spriteBatch.Draw(image, imageRectangle, Color.White);

			// Draw the Enemies
			for (int i = 0; i < enemies.Count; i++)
			{
				enemies[i].Draw(spriteBatch);
			}

			spriteBatch.End();

			player.Draw(gameTime);
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

			if (currentKeyboardState.IsKeyDown(Keys.A))
			{
				player.Bigger();
			}
			else if (currentKeyboardState.IsKeyDown(Keys.S))
			{
				player.Smaller();
			}

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
			Rectangle playerBounds = player.BoundingBox;
			Rectangle enemyBounds;


			// Do the collision between the player and the enemies
			for (int i = 0; i < enemies.Count; i++)
			{
				enemyBounds = enemies[i].BoundingBox;

				// Determine if the two objects collided with each
				// other
				if (playerBounds.Intersects(enemyBounds))
				{
					// Collision, either game over or success eating
					if (enemies[i].BoundingVector.Length() <= player.BoundingVector.Length())
					{
						player.Eat(enemies[i].BoundingVector.Length());
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
