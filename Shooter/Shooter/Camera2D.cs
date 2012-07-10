using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Shooter
{
	public class Camera2D
	{
		private const float zoomUpperLimit = 5f;
		private const float zoomLowerLimit = .5f;

		private float _zoom;
		private Matrix _transform;
		private Vector2 _pos;
		private int _viewportWidth;
		private int _viewportHeight;
		private int _worldWidth;
		private int _worldHeight;

		private Vector2 goalPosition;
		private float goalZoom;

		#region Properties

		public float Zoom
		{
			get { return _zoom; }
			private set
			{
				_zoom = BoundZoom(value);
			}
		}

		public float Rotation
		{
			get;
			private set;
		}

		public Vector2 Position
		{
			get { return _pos; }
			private set
			{
				_pos = BoundPosition(value);
			}
		}

		#endregion

		public Camera2D(Viewport viewport, int worldWidth,
			int worldHeight)
		{
			Rotation = 0.0f;
			SetZoom(2f);
			SetPosition(Vector2.Zero);

			_viewportWidth = viewport.Width;
			_viewportHeight = viewport.Height;
			_worldWidth = worldWidth;
			_worldHeight = worldHeight;
		}

		public void ChangeZoom(float change)
		{
			goalZoom += change;
			goalZoom = BoundZoom(goalZoom);
		}

		public void ZoomTo(float zoom)
		{
			goalZoom = BoundZoom(zoom);
		}

		public void SetZoom(float zoom)
		{
			goalZoom = Zoom = BoundZoom(zoom);
		}

		public void MoveTo(Vector2 position)
		{
			goalPosition = BoundPosition(position);
		}

		public void SetPosition(Vector2 position)
		{
			goalPosition = Position = BoundPosition(position);
		}

		public void Update()
		{
			var zoomDiff = goalZoom - Zoom;
			Zoom += zoomDiff * .05f;

			var posDiff = goalPosition - Position;
			Position += posDiff * Zoom * .05f;
		}

		public Matrix GetTransformation()
		{
			_transform =
				Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
				Matrix.CreateRotationZ(Rotation) *
				Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
				Matrix.CreateTranslation(new Vector3(_viewportWidth * 0.5f,
					_viewportHeight * 0.5f, 0));

			return _transform;
		}

		private Vector2 BoundPosition(Vector2 position)
		{
			//return position;

			//float leftBarrier = (float)(_worldWidth + _viewportWidth/2) / _zoom;
			float leftBarrier = -_worldWidth / 2 + _worldWidth / 2 / _zoom;
			float rightBarrier = _worldWidth / 2 - _worldWidth / 2 / _zoom;
			float topBarrier = -_worldHeight / 2 + _worldHeight / 2 / _zoom;
			float bottomBarrier = _worldHeight / 2 - _worldHeight / 2 / _zoom;
			
			/*float rightBarrier = _worldWidth - (float)_viewportWidth * .5f / _zoom;
			float topBarrier = _worldHeight -
					(float)_viewportHeight * .5f / _zoom;
			float bottomBarrier = (float)_viewportHeight *
					.5f / _zoom;
			

			/*
			float leftBarrier = (float)-_worldWidth + _viewportWidth/_zoom;
			float rightBarrier = _worldWidth;
			float topBarrier = _worldHeight -
					(float)_viewportHeight * .5f / _zoom;
			float bottomBarrier = (float)_viewportHeight *
					.5f / _zoom;
			*/
			if (position.X < leftBarrier)
				position.X = leftBarrier;
			if (position.X > rightBarrier)
				position.X = rightBarrier;
			if (position.Y < topBarrier)
				position.Y = topBarrier;
			if (position.Y > bottomBarrier)
				position.Y = bottomBarrier;

			return position;
		}

		private float BoundZoom(float zoom)
		{
			if (zoom < zoomLowerLimit)
				zoom = zoomLowerLimit;
			if (zoom > zoomUpperLimit)
				zoom = zoomUpperLimit;

			return zoom;
		}
	}
}
