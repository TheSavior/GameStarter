namespace Shooter
{
	using System;
	using Microsoft.Xna.Framework;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class Animations
	{
		/*
		 * t = time elapsed from anim start
		 * b = start value 
		 * c = target - b (also known as change)
		 * d = goal duration
		 */

		private Vector2 Linear(float elapsed, Vector2 start, Vector2 change, float duration)
		{
			return change * elapsed / duration + start;
		}

		private Vector2 Quad(float elapsed, Vector2 start, Vector2 change, float duration)
		{
			float x = InternalQuad(elapsed, start.X, change.X, duration);
			float y = InternalQuad(elapsed, start.Y, change.Y, duration);
			return new Vector2(x, y);
		}

		/*
		 * These equations taken from http://gizma.com/easing/
		 */
		private float InternalLinear(float t, float b, float c, float d)
		{
			return c * t / d + b;
		}

		private float InternalQuad(float t, float b, float c, float d)
		{
			t /= d / 2;
			if (t < 1)
				return c / 2 * t * t + b;
			t--;
			return -c / 2 * (t * (t - 2) - 1) + b;
		}


		Vector2 start;
		Vector2 vectorRef;
		float startTime;
		float duration;
		float elapsed;
		Vector2 change;

		Action<Vector2> callback;

		public void Animate(Vector2 vector, Vector2 change, float duration, Action<Vector2> callback)
		{
			this.start = new Vector2(vector.X, vector.Y);
			this.vectorRef = vector;
			startTime = (float)Globals.GameTime.TotalGameTime.TotalSeconds;
			this.duration = duration;
			this.change = change;

			this.callback = callback;
		}


		public void Update()
		{
			if (callback == null)
				return;

			float elapsed = (float)Globals.GameTime.TotalGameTime.TotalSeconds - startTime;
			if (elapsed > duration)
				return;

			var newValue = Quad(elapsed, start, change, duration);
			this.callback(newValue);
		}
	}
}
