using Shooter.AiScheme;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter.Actors
{
	public class Enemy : SmartActorBase
	{
		public Enemy()
			: base(new Wander())
		{
			MaxSpeed = 3f;
		}

		public override void LoadContent()
		{
			Texture = Game.Content.Load<Texture2D>("enemy");
			base.LoadContent();
		}

		public override void Reset()
		{
			Active = true;
			base.Reset();
		}

		public void SetSize(float size)
		{
			Scale = size;
		}
	}
}
