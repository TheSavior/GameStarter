using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shooter.AiScheme;

namespace Shooter.Actors
{
	public class Player : SmartActorBase
	{
		public Player()
			: base(new KeyboardControlled(speedUpStep: .10f, speedDownStep: .05f))
		{
			MaxSpeed = 3f;
		}

		public override void Reset()
		{
			base.Reset();
			Width = 3;
		}

		public override void LoadContent()
		{
            Texture = Game.Content.Load<Texture2D>("Graphics\\Fish");
			base.LoadContent();
		}

		public void Eat(ActorBase actor)
		{
			// We should grow at some ratio of the enemy size to our current size
			// First figure out what percent they are of us
			var length = BoundingVector.Length();

			var percent = actor.BoundingVector.Length() / length;
			var add = MathHelper.Lerp(0, Width / 10, percent);

			Width += add;
		}

		public override void Update(GameTime gameTime)
		{
			// Debug Helpers
			if (Globals.KeyManager.IsKeyDown(Keys.A))
			{
				Bigger();
			}
			else if (Globals.KeyManager.IsKeyDown(Keys.S))
			{
				Smaller();
			}

			base.Update(gameTime);
		}

		#region Debug Helpers
		private void Bigger()
		{
			Width += .05f;
		}

		private void Smaller()
		{
			Width -= .05f;
		}
		#endregion
	}
}
