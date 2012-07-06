using Microsoft.Xna.Framework;

namespace Shooter.Extensions
{
	public static class RectangleExtensions
	{
		public static Vector2 Center(this Rectangle rectangle)
		{
			return new Vector2(rectangle.Center.X, rectangle.Center.Y);
		}

		public static Vector2 TopRight(this Rectangle Rectangle)
		{
			return new Vector2(Rectangle.Right, Rectangle.Top);
		}
	}
}
