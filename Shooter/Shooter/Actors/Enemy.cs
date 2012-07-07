using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shooter.AiScheme;

namespace Shooter.Actors
{
	public class Enemy : Wander
	{
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
