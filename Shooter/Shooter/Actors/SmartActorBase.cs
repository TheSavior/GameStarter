
using Microsoft.Xna.Framework;
using Shooter.AiScheme;
namespace Shooter.Actors
{
	public class SmartActorBase : ActorBase
	{
		public AiSchemeBase AiScheme;

		public SmartActorBase(AiSchemeBase aiScheme)
		{
			AiScheme = aiScheme;
			AiScheme.Actor = this;
		}

		public override void Update(GameTime gameTime)
		{
			AiScheme.Update();

			base.Update(gameTime);
		}
	}
}
