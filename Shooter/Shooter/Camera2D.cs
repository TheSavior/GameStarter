using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Shooter
{
	public class Camera2D
	{
		private float _zoom;
		private Matrix _transform;
		private Vector2 _pos;
		private int _viewportWidth;
		private int _viewportHeight;
		private int _worldWidth;
		private int _worldHeight;

		private Vector2 goalPosition;
		private float goalZoom;

		private readonly float initialZoom;

		#region Properties

		public float MaxZoom
		{
			get;
			set;
		}

		public float MinZoom
		{
			get;
			set;
		}

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
			int worldHeight, float initialZoom)
		{
			Rotation = 0.0f;
			this.initialZoom = initialZoom;

			_viewportWidth = viewport.Width;
			_viewportHeight = viewport.Height;
			_worldWidth = worldWidth;
			_worldHeight = worldHeight;

			Reset();
		}

		public void Reset()
		{
			SetZoom(this.initialZoom);
			SetPosition(Vector2.Zero);
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
			float leftBarrier = -_worldWidth / 2 + _worldWidth / 2 / (_zoom / initialZoom);
			float rightBarrier = _worldWidth / 2 - _worldWidth / 2 / (_zoom / initialZoom);
			float topBarrier = -_worldHeight / 2 + _worldHeight / 2 / (_zoom / initialZoom);
			float bottomBarrier = _worldHeight / 2 - _worldHeight / 2 / (_zoom / initialZoom);

			position.X = MathHelper.Clamp(position.X, leftBarrier, rightBarrier);
			position.Y = MathHelper.Clamp(position.Y, topBarrier, bottomBarrier);

			return position;
		}

		private float BoundZoom(float zoom)
		{
			zoom = MathHelper.Clamp(zoom, MinZoom, MaxZoom);
			return zoom;
		}
	}
}
