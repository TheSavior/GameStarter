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

		public override void LoadContent()
		{
			Texture = Game.Content.Load<Texture2D>("Fish");
			base.LoadContent();
		}

		public void Eat(ActorBase actor)
		{
			// We should grow at some ratio of the enemy size to our current size
			// First figure out what percent they are of us
			var length = BoundingVector.Length();

			var percent = actor.BoundingVector.Length() / length;
			var add = MathHelper.Lerp(0, Scale / 10, percent);

			Scale += add;
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
			Scale += .05f;
		}

		private void Smaller()
		{
			Scale -= .05f;
		}
		#endregion
	}
}
