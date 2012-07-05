using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shooter.Actors;
using Shooter.Extensions;
using Shooter.Popups;

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

		GameOverPopup popup;

		public ActionScreen()
		{
			imageRectangle = new Rectangle(
				0,
				0,
				Game.Window.ClientBounds.Width,
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

		public override void Initialize()
		{
			// Initialize the player class
			player = new Player();
			Components.Add(player);

			popup = new GameOverPopup();
			popup.Visible = false;
			Components.Add(popup);

			base.Initialize();
		}


		public override void LoadContent()
		{
			spriteBatch = new SpriteBatch(Globals.Graphics.GraphicsDevice);

			image = Game.Content.Load<Texture2D>("greenmetal");

			// We need to LoadContent on the player before we set its position
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

			if (!player.Active || currentKeyboardState.IsKeyDown(Keys.C) && previousKeyboardState.IsKeyUp(Keys.C))
			{
				popup.Visible = true;
				player.Enabled = false;
			}
			else if (currentKeyboardState.IsKeyDown(Keys.V) && previousKeyboardState.IsKeyUp(Keys.V))
			{
				popup.Visible = false;
				player.Enabled = true;
			}

			if (player.Enabled)
			{
				// Update the player
				UpdatePlayer(gameTime);

				// Update the enemies
				UpdateEnemies(gameTime);

				// Update the collision
				UpdateCollision();
			}

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

			base.Draw(gameTime);
		}

		private void AddEnemy()
		{
			// Create an enemy
			var size = (float)(random.NextDouble() + .5);
			Debug.WriteLine("Enemy size {0}", size);
			Enemy enemy = new Enemy();

			// Randomly generate the position of the enemy
			var viewport = Globals.Graphics.GraphicsDevice.Viewport;
			Vector2 position = new Vector2(viewport.Width, random.Next(100, viewport.Height - 100));

			// Initialize the enemy
			enemy.Initialize();
			enemy.LoadContent();
			enemy.SetPosition(position);
			enemy.SetSize(size);

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

			Color[,] playerColors = player.Texture.ToColorArray();
			Color[,] enemyColors;

			// Do the collision between the player and the enemies
			for (int i = 0; i < enemies.Count; i++)
			{
				enemyBounds = enemies[i].BoundingBox;
				enemyColors = enemies[i].Texture.ToColorArray();

				// Determine if the two objects collided with each
				// other
				if (playerBounds.Intersects(enemyBounds))
				{
					// Possible collision, lets start checking every pixel
					for (int x1 = 0; x1 < playerBounds.Width; x1++)
					{
						for (int y1 = 0; y1 < playerBounds.Height; y1++)
						{
							// Scaled pixel position of player
							Vector2 playerScaledPixelPos = new Vector2(x1, y1);

							Vector2 realWorldPixelPos = playerScaledPixelPos + new Vector2(playerBounds.X, playerBounds.Y);

							// Find the location of this pixel on the original texture
							// by dividing the scaled one by the scale
							Vector2 playerTexturePixelPos = playerScaledPixelPos / player.Scale;

							// Given an x and y, figure out if the enemy bounds contains it
							// if yes, find the position of that location relative to the top left of
							// the enemies bounding box. Then scale that down by enemies scale
							if (enemyBounds.Contains((int)realWorldPixelPos.X, (int)realWorldPixelPos.Y))
							{
								// it is inside the enemy box
								Vector2 enemyScaledPixelPos = realWorldPixelPos - new Vector2(enemyBounds.X, enemyBounds.Y);
								Vector2 enemyTexturePixelPos = enemyScaledPixelPos / enemies[i].Scale;


								if (playerColors[(int)playerTexturePixelPos.X, (int)playerTexturePixelPos.Y].A > 0)
								{
									if (enemyColors[(int)enemyTexturePixelPos.X, (int)enemyTexturePixelPos.Y].A > 0)
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
										return;

									}
								}
							}
						}
					}
				}
			}
		}
	}

}
