using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter.Actors
{
	public class ActorBase : DrawableComponent
	{
		public bool Active;

		// Animation representing the player
		public Texture2D Texture;

		// Position of the center of the Actor
		// relative to the upper left side of the screen
		public Vector2 Position;

		public float Scale;

		protected SpriteBatch spriteBatch;

		public Direction DrawDirection
		{
			get;
			protected set;
		}

		public Rectangle BoundingBox
		{
			get
			{
				return new Rectangle(
					(int)(Position.X - (Texture.Width * Scale) / 2),
					(int)(Position.Y - (Texture.Height * Scale) / 2),
					(int)(Texture.Width * Scale),
					(int)(Texture.Height * Scale));
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
			this.Scale = 1f;
		}

		public override void Initialize()
		{
			DrawDirection = Direction.Right;
			base.Initialize();
		}

		/// <summary>
		/// Load actor content
		/// </summary>
		/// <remarks>The Texture must be set when this is called</remarks>
		public override void LoadContent()
		{
			spriteBatch = new SpriteBatch(Globals.Graphics.GraphicsDevice);

			base.LoadContent();
		}

		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			SpriteEffects effect = SpriteEffects.None;
			if (DrawDirection == Direction.Left)
			{
				effect = SpriteEffects.FlipHorizontally;
			}
			spriteBatch.Draw(Texture, BoundingBox, null, Color.White, 0f, Vector2.Zero, effect, 0);
			spriteBatch.End();
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
	}
}
