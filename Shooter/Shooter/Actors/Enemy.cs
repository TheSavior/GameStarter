using Microsoft.Xna.Framework.Graphics;
using Shooter.AiScheme;

namespace Shooter.Actors
{
	public class Enemy : SmartActorBase
	{
		public Enemy()
			: base(new Wander(speedUpStep: .10f))
		{
			MaxSpeed = 3f;
		}

		public override void LoadContent()
		{
            Texture = Game.Content.Load<Texture2D>("Graphics\\enemy");
			base.LoadContent();
		}

		public void SetSize(float size)
		{
			Width = size;
		}
	}
}
