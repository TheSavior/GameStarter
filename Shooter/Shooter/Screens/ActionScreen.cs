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

		GameOverPopup popup;

		Camera2D camera;
		float initialZoom;
		float zoomIncrement;

		int previousScroll;

		public ActionScreen()
		{
			imageRectangle = new Rectangle(
				0,
				0,
				Globals.Graphics.GraphicsDevice.Viewport.Width,
				Globals.Graphics.GraphicsDevice.Viewport.Height);

			zoomIncrement = .1f;
			initialZoom = 1f;

			camera = new Camera2D(
				Globals.Graphics.GraphicsDevice.Viewport,
				Globals.Graphics.GraphicsDevice.Viewport.Width,
				Globals.Graphics.GraphicsDevice.Viewport.Height,
				initialZoom);

			// Set the time keepers to zero
			previousSpawnTime = TimeSpan.Zero;

			// Used to determine how fast enemy respawns
			enemySpawnTime = TimeSpan.FromSeconds(0.5f);

			// Initialize our random number generator
			random = new Random();
		}

		public override void Initialize()
		{
			// Initialize the player class
			player = new Player();
			Components.Add(player);

			popup = new GameOverPopup(this);
			Components.Add(popup);

			base.Initialize();
		}

		public override void LoadContent()
		{
			image = Game.Content.Load<Texture2D>("greenmetal");

			// We need to LoadContent on the player before we set its position
			base.LoadContent();
		}

		public override void Reset()
		{
			// Initialize the enemies list
			enemies = new List<Enemy>();
			popup.Visible = false;

			Vector2 playerPosition = new Vector2(0 + player.BoundingBox.Width / 2, Globals.Graphics.GraphicsDevice.Viewport.Height / 2);
			player.SetPosition(playerPosition);

			camera.Zoom = initialZoom;

			base.Reset();
		}

		public override void Update(GameTime gameTime)
		{
			if (!player.Active || Globals.KeyManager.IsKeyPress(Keys.C))
			{
				popup.Visible = true;
				player.Enabled = false;
			}
			else if (Globals.KeyManager.IsKeyPress(Keys.V))
			{
				popup.Visible = false;
				player.Enabled = true;
			}

			MouseState mouseStateCurrent = Mouse.GetState();
			// Adjust zoom if the mouse wheel has moved
			if (mouseStateCurrent.ScrollWheelValue > previousScroll)
				camera.Zoom += zoomIncrement;
			else if (mouseStateCurrent.ScrollWheelValue < previousScroll)
				camera.Zoom -= zoomIncrement;

			previousScroll = mouseStateCurrent.ScrollWheelValue;

			// Move the camera when the arrow keys are pressed
			Vector2 movement = Vector2.Zero;
			Viewport vp = Globals.Graphics.GraphicsDevice.Viewport;

			if (Globals.KeyManager.IsKeyDown(Keys.NumPad4))
				movement.X--;
			if (Globals.KeyManager.IsKeyDown(Keys.NumPad6))
				movement.X++;
			if (Globals.KeyManager.IsKeyDown(Keys.NumPad8))
				movement.Y--;
			if (Globals.KeyManager.IsKeyDown(Keys.NumPad2))
				movement.Y++;

			//camera.Pos += movement * 20;
			//camera.Pos = player.Position;
			var diff = player.Position - camera.Pos;
			camera.Pos += diff * .05f;

			if (player.Active)
			{
				// Update the player
				UpdatePlayer(gameTime);

				// Update the enemies
				UpdateEnemies(gameTime);

				// Update the collision
				UpdateCollision();
			}

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{

			Globals.SpriteBatch.Begin(SpriteSortMode.Deferred,
					null, null, null, null, null,
					camera.GetTransformation());

			Globals.SpriteBatch.Draw(image, imageRectangle, Color.White);

			// Draw the Enemies
			for (int i = 0; i < enemies.Count; i++)
			{
				enemies[i].Draw(gameTime);
			}

			player.Draw(gameTime);

			Globals.SpriteBatch.End();

			Globals.SpriteBatch.Begin();
			if (!player.Active)
			{
				popup.Draw(gameTime);
			}
			Globals.SpriteBatch.End();
		}

		private void AddEnemy()
		{
			// Create an enemy
			var size = (float)(random.NextDouble() + .5);
			Debug.WriteLine("Enemy size {0}", size);
			Enemy enemy = new Enemy();

			// Initialize the enemy
			enemy.Initialize();
			enemy.LoadContent();

			// Randomly generate the position of the enemy
			var viewport = Globals.Graphics.GraphicsDevice.Viewport;
			Vector2 position = new Vector2(viewport.Width + enemy.BoundingBox.Width, random.Next(100, viewport.Height - 100));

			enemy.SetPosition(position);
			enemy.SetSize(size);

			// Add the enemy to the active enemies list
			enemies.Add(enemy);
		}

		private void UpdatePlayer(GameTime gameTime)
		{
			// Use the Keyboard
			bool left = Globals.KeyManager.IsKeyDown(Keys.Left);
			bool right = Globals.KeyManager.IsKeyDown(Keys.Right);
			bool up = Globals.KeyManager.IsKeyDown(Keys.Up);
			bool down = Globals.KeyManager.IsKeyDown(Keys.Down);

			if (left) player.AddDirection(Direction.Left);
			if (right) player.AddDirection(Direction.Right);
			if (up) player.AddDirection(Direction.Up);
			if (down) player.AddDirection(Direction.Down);

			if (Globals.KeyManager.IsKeyDown(Keys.A))
			{
				player.Bigger();
			}
			else if (Globals.KeyManager.IsKeyDown(Keys.S))
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
					// The two big rectangles intersect, lets grab just
					// the rectangle where they intersect
					Rectangle collisionRectangle = Rectangle.Intersect(playerBounds, enemyBounds);

					// Possible collision, lets start checking every pixel
					for (int x1 = 0; x1 < collisionRectangle.Width; x1++)
					{
						for (int y1 = 0; y1 < collisionRectangle.Height; y1++)
						{
							// Where is this pixel in the whole screen
							Vector2 realWorldPixelPos = new Vector2(collisionRectangle.X + x1, collisionRectangle.Y + y1);

							// Scaled pixel position of player
							Vector2 playerScaledPixelPos = realWorldPixelPos - new Vector2(playerBounds.X, playerBounds.Y);

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

								// Lets handle flipping
								int playerTextureWithFlip = (int)playerTexturePixelPos.X;
								if (player.DrawDirection == Direction.Left)
								{
									playerTextureWithFlip = player.Texture.Width - playerTextureWithFlip - 1;
								}

								if (playerColors[playerTextureWithFlip, (int)playerTexturePixelPos.Y].A > 0)
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
