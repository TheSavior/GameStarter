using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shooter.Actors;
using Shooter.Components;
using Shooter.Extensions;
using Shooter.Popups;

namespace Shooter.Screens
{
	public class ActionScreen : ScreenBase
	{
		Texture2D image;
		Rectangle imageRectangle;

		// Represents the player 
		public Player player;

		// Enemies
		List<Enemy> enemies;

		// The rate at which the enemies appear
		TimeSpan enemySpawnTime;
		TimeSpan previousSpawnTime;

		// A random number generator
		Random random;

		GameOverPopup popup;

		HudComponent hud;

		Animations ani;

		public Camera2D camera;
		float initialZoom;
		float zoomIncrement;

		public ActionScreen()
		{
			imageRectangle = new Rectangle(
				0,
				0,
				Globals.Graphics.GraphicsDevice.Viewport.Width,
				Globals.Graphics.GraphicsDevice.Viewport.Height);

			zoomIncrement = .4f;
			initialZoom = 5f;

			camera = new Camera2D(
				Globals.Graphics.GraphicsDevice.Viewport,
				Globals.Graphics.GraphicsDevice.Viewport.Width,
				Globals.Graphics.GraphicsDevice.Viewport.Height);

			// Set the time keepers to zero
			previousSpawnTime = TimeSpan.Zero;

			// Used to determine how fast enemy respawns
			enemySpawnTime = TimeSpan.FromSeconds(0.5f);

			// Initialize our random number generator
			random = new Random();

			ani = new Animations();
		}

		public override void Initialize()
		{
			// Initialize the player class
			player = new Player();
			Components.Add(player);

			popup = new GameOverPopup(this);
			Components.Add(popup);

			hud = new HudComponent(this);
			Components.Add(hud);

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
			base.Reset();

			// Initialize the enemies list
			enemies = new List<Enemy>();
			popup.Visible = false;

			Vector2 playerPosition = new Vector2(0 + player.BoundingBox.Width / 2, Globals.Graphics.GraphicsDevice.Viewport.Height / 2);
			player.Position = playerPosition;

			player.Scale = .2f;

			camera.SetZoom(initialZoom);
			camera.SetPosition(playerPosition);
		}

		public override void Update(GameTime gameTime)
		{
			// Adjust zoom if the mouse wheel has moved
			if (Globals.KeyManager.IsKeyPress(Keys.W))
				camera.ChangeZoom(-zoomIncrement);
			else if (Globals.KeyManager.IsKeyPress(Keys.Q))
				camera.ChangeZoom(zoomIncrement);

			camera.MoveTo(player.Position);

			camera.Update();

			if (!player.Active)
			{
				player.Enabled = false;
			}

			if (player.Active)
			{
				if (Globals.KeyManager.IsKeyPress(Keys.T))
				{
					ani.Animate(player.Position, new Vector2(100, 100), 1, (Vector2 value) => { player.Position = value; });
				}

				ani.Update();

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
			hud.Draw(gameTime);

			if (!player.Active)
			{
				popup.Draw(gameTime);
			}
			Globals.SpriteBatch.End();
		}

		private void AddEnemy()
		{
			// Create an enemy
			Enemy enemy = new Enemy();

			// Initialize the enemy
			enemy.Initialize();
			enemy.LoadContent();

			// Randomly generate the position of the enemy
			var viewport = Globals.Graphics.GraphicsDevice.Viewport;
			var locX = random.Next(0, viewport.Width - enemy.BoundingBox.Width);
			var locY = random.Next(0, viewport.Height - enemy.BoundingBox.Height);
			Vector2 position = new Vector2(locX, locY);
			enemy.Position = position;

			//var size = (float)(random.NextDouble() - .1 + player.Scale);
			var size = (float)(random.NextDouble() - .5 + player.Scale);
			size = Math.Max(.2f, size);
			enemy.SetSize(size);

			// Add the enemy to the active enemies list
			enemies.Add(enemy);
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
								if (player.Velocity.X < 0)
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
											player.Eat(enemies[i]);
											enemies[i].Active = false;

											camera.ZoomTo(1 / player.Scale);
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
