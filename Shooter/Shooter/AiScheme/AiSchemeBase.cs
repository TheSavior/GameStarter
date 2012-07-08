

using Shooter.Actors;
namespace Shooter.AiScheme
{
	public abstract class AiSchemeBase
	{
		public SmartActorBase Actor
		{
			get;
			set;
		}

		public AiSchemeBase()
		{
		}

		public virtual void Reset()
		{

		}

		public virtual void Update()
		{

		}
	}
}
