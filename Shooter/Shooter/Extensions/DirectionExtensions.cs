using Microsoft.Xna.Framework;

namespace Shooter.Extensions
{
	public static class DirectionExtensions
	{
		public static Vector2 Vector(this Direction direction)
		{
			switch (direction)
			{
				case Direction.Up:
					return -Vector2.UnitY;
				case Direction.Down:
					return Vector2.UnitY;
				case Direction.Right:
					return Vector2.UnitX;
				case Direction.Left:
					return -Vector2.UnitX;
			}

			return Vector2.Zero;
		}
	}
}
