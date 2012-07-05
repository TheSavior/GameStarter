using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter.Actors
{
	public class ActorBase : DrawableComponent
	{
		public bool Active;

		// Animation representing the player
		public Texture2D Texture;

		// Position of the Player relative to the upper left side of the screen
		protected Vector2 Position;

		public Vector2 Scale;

		// Unit vector pointing in the direction of the proportions of the
		// Texture
		protected Vector2 ScalingFactor;

		protected SpriteBatch spriteBatch;

		public Rectangle BoundingBox
		{
			get
			{
				return new Rectangle(
					(int)(Position.X - (Texture.Width + Scale.X) / 2),
					(int)(Position.Y - (Texture.Height + Scale.Y) / 2),
					(int)(Texture.Width + Scale.X),
					(int)(Texture.Height + Scale.Y));
			}
		}

		public Vector2 BoundingVector
		{
			get
			{
				return new Vector2(
					BoundingBox.Width,
					BoundingBox.Height);
			}
		}

		public ActorBase()
		{
			this.Scale = new Vector2(1, 1);
		}

		/// <summary>
		/// Sets the center of the player
		/// </summary>
		/// <param name="position"></param>
		public void SetPosition(Vector2 position)
		{
			// Set the starting position of the player around the middle of the screen and to the back
			this.Position = position;
		}

		/// <summary>
		/// Load actor content
		/// </summary>
		/// <remarks>The Texture must be set when this is called</remarks>
		public override void LoadContent()
		{
			spriteBatch = new SpriteBatch(Globals.Graphics.GraphicsDevice);

			ScalingFactor = new Vector2(Texture.Width, Texture.Height);
			ScalingFactor.Normalize();

			base.LoadContent();
		}
	}
}
