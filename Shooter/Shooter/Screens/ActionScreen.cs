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

		public static RectangleF WorldRectangle;

		// Represents the player 
		public static Player Player;

		// Enemies
		List<Enemy> enemies;

		// The rate at which the enemies appear
		TimeSpan enemySpawnTime;
		TimeSpan previousSpawnTime;

		// A random number generator
		Random random;

		GameOverPopup popup;

		HudComponent hud;

		public Camera2D camera;
		float initialZoom;
		float zoomIncrement;

		public ActionScreen()
		{
			var viewport = Globals.Graphics.GraphicsDevice.Viewport;
			Vector2 aspectRatio = new Vector2(viewport.Width, viewport.Height);
			aspectRatio.Normalize();

			// 100cm scaled 
			float width = (100 * aspectRatio.X * 2);
			float height = (100 * aspectRatio.Y * 2);

			WorldRectangle = new RectangleF(
				-width / 2,
				-height / 2,
				width,
				height);

			zoomIncrement = .5f;
			initialZoom = (float)viewport.Width / width;

			camera = new Camera2D(
				viewport,
				width,
				height,
				initialZoom);

			camera.MinZoom = initialZoom - 5;
			camera.MaxZoom = 10f;

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
			Player = new Player();
			Components.Add(Player);

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

			Vector2 playerPosition = new Vector2(0, 0);
			Player.Position = playerPosition;

			Player.Width = .2f;

			camera.Reset();
			camera.SetPosition(playerPosition);
		}

		public override void Update(GameTime gameTime)
		{
			// Adjust zoom if the mouse wheel has moved
			if (Globals.KeyManager.IsKeyPress(Keys.W))
				camera.ChangeZoom(-zoomIncrement);
			else if (Globals.KeyManager.IsKeyPress(Keys.Q))
				camera.ChangeZoom(zoomIncrement);

			camera.MoveTo(Player.Position);

			camera.Update();

			if (!Player.Active)
			{
				Player.Enabled = false;
			}

			if (Player.Active)
			{
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

			Vector2 scale = new Vector2((float)WorldRectangle.Width / image.Width, (float)WorldRectangle.Height / image.Height);
			//Globals.SpriteBatch.Draw(image, WorldRectangle, Color.White);

			//Globals.SpriteBatch.Draw(image, Vector2.Zero, null, Color.White, 0f, image.Bounds.Center(), SpriteEffects.None, 0);

			var center = image.Bounds.Center();




			Globals.SpriteBatch.Draw(image, Vector2.Zero, null, Color.White, 0f, image.Bounds.Center(), scale, SpriteEffects.None, 0);
			// Draw the Enemies
			for (int i = 0; i < enemies.Count; i++)
			{
				enemies[i].Draw(gameTime);
			}

			Player.Draw(gameTime);

			Globals.SpriteBatch.End();

			Globals.SpriteBatch.Begin();
			hud.Draw(gameTime);

			if (!Player.Active)
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

			var halfEnemy = enemy.BoundingVector / 2;

			var locX = random.Next((int)(WorldRectangle.Left + halfEnemy.X), (int)(WorldRectangle.Right - halfEnemy.X));
			var locY = random.Next((int)(WorldRectangle.Top + halfEnemy.Y), (int)(WorldRectangle.Bottom - halfEnemy.Y));
			Vector2 position = new Vector2(locX, locY);
			enemy.Position = position;

			//var size = (float)(random.NextDouble() - .1 + player.Scale);
			var size = (float)(random.NextDouble() * Player.Width);
			size = Math.Max(Player.Width / 2, size);
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
			RectangleF playerBounds = Player.BoundingBox;
			RectangleF enemyBounds;

			Color[,] playerColors = Player.Texture.ToColorArray();
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
					RectangleF collisionRectangle = RectangleF.Intersect(playerBounds, enemyBounds);

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
							Vector2 playerTexturePixelPos = playerScaledPixelPos / Player.Width;

							// Given an x and y, figure out if the enemy bounds contains it
							// if yes, find the position of that location relative to the top left of
							// the enemies bounding box. Then scale that down by enemies scale
							if (enemyBounds.Contains((int)realWorldPixelPos.X, (int)realWorldPixelPos.Y))
							{
								// it is inside the enemy box
								Vector2 enemyScaledPixelPos = realWorldPixelPos - new Vector2(enemyBounds.X, enemyBounds.Y);
								Vector2 enemyTexturePixelPos = enemyScaledPixelPos / enemies[i].Width;

								// Lets handle flipping
								int playerTextureWithFlip = (int)playerTexturePixelPos.X;
								if (Player.Velocity.X < 0)
								{
									playerTextureWithFlip = Player.Texture.Width - playerTextureWithFlip - 1;
								}

								if (playerColors[playerTextureWithFlip, (int)playerTexturePixelPos.Y].A > 0)
								{
									if (enemyColors[(int)enemyTexturePixelPos.X, (int)enemyTexturePixelPos.Y].A > 0)
									{
										// Collision, either game over or success eating
										if (enemies[i].Width <= Player.Width)
										{
											Player.Eat(enemies[i]);
											enemies[i].Active = false;

											camera.ZoomTo(1 / Player.Width);
										}
										else
										{
											Player.Active = false;
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
