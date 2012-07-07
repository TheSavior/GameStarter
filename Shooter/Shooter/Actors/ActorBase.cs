using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter.Actors
{
	public class ActorBase : DrawableComponentManager
	{
		// Velocity of the Actor
		public Vector2 Velocity;

		public bool Active;

		// Animation representing the player
		public Texture2D Texture;

		// Position of the center of the Actor
		// relative to the upper left side of the screen
		public Vector2 Position;

		public float Scale;

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

		public override void Reset()
		{
			Active = true;
			Enabled = true;
			Scale = 1f;
			DrawDirection = Direction.Right;

			base.Reset();
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteEffects effect = SpriteEffects.None;
			if (DrawDirection == Direction.Left)
			{
				effect = SpriteEffects.FlipHorizontally;
			}

			var origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
			Globals.SpriteBatch.Draw(Texture, Position, null, Color.White, 0, origin, Scale, effect, 0);
			base.Draw(gameTime);
		}
	}
}
