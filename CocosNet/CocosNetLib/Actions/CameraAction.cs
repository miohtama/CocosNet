using System;

namespace CocosNet.Actions {
	public abstract class CameraAction : IntervalAction {
		protected float _centerXOrig;
		protected float _centerYOrig;
		protected float _centerZOrig;

		protected float _eyeXOrig;
		protected float _eyeYOrig;
		protected float _eyeZOrig;

		protected float _upXOrig;
		protected float _upYOrig;
		protected float _upZOrig;

		public CameraAction(float duration) : base(duration) {
		}

		public override void Start() {
			base.Start();
			
			Target.Camera.GetCenter(out _centerXOrig, out _centerYOrig, out _centerZOrig);
			Target.Camera.GetEye(out _eyeXOrig, out _eyeYOrig, out _eyeZOrig);
			Target.Camera.GetUp(out _upXOrig, out _upYOrig, out _upZOrig);
		}
		
	}

	public class OrbitCamera : CameraAction {
		private float _radius;
		private float _deltaRadius;
		private float _angleZ;
		private float _deltaAngleZ;
		private float _angleX;
		private float _deltaAngleX;

		private float _radZ;
		private float _radDeltaZ;
		private float _radX;
		private float _radDeltaX;

		private void SphericalRadius(out float newRadius, out float zenith, out float azimuth) {
			float ex, ey, ez, cx, cy, cz, x, y, z;
			float r;
			// radius
			float s;
			
			Target.Camera.GetEye(out ex, out ey, out ez);
			Target.Camera.GetCenter(out cx, out cy, out cz);
			
			x = ex - cx;
			y = ey - cy;
			z = ez - cz;
			
			r = Convert.ToSingle(Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2)));
			s = Convert.ToSingle(Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)));
			if (s == 0f)
				s = 1E-08f;
			if (r == 0f)
				r = 1E-08f;
			
			zenith = Convert.ToSingle(Math.Acos(z / r));
			if (x < 0)
				azimuth = Convert.ToSingle(Math.PI - Math.Asin(y / s));
			else
				azimuth = (float)Math.Asin(y / s);
			
			newRadius = r / Camera.ZEye;
		}

		public OrbitCamera(float duration, float radius, float deltaRadius, float angleZ, float deltaAngleZ, float angleX, float deltaAngleX) : base(duration) {
			_radius = radius;
			_deltaRadius = deltaRadius;
			_angleZ = angleZ;
			_deltaAngleZ = deltaAngleZ;
			_angleX = angleX;
			_deltaAngleX = deltaAngleX;
			
			_radDeltaZ = _deltaAngleZ.ToRadians();
			_radDeltaX = _deltaAngleX.ToRadians();
		}

		public override object Clone() {
			return new OrbitCamera(Duration, _radius, _deltaRadius, _angleZ, _deltaAngleZ, _angleX, _deltaAngleX);
		}

		public override void Start() {
			base.Start();
			
			float r, zenith, azimuth;
			
			SphericalRadius(out r, out zenith, out azimuth);
			if (float.IsNaN(_radius)) {
				_radius = r;
			}
			if (float.IsNaN(_angleZ)) {
				_angleZ = zenith.ToDegrees();
			}
			if (float.IsNaN(_angleX)) {
				_angleX = azimuth.ToDegrees();
			}
			
			_radZ = _angleZ.ToRadians();
			_radX = _angleX.ToRadians();
		}

		public override void Update(float t) {
			float r = (_radius + _deltaRadius * t) * Camera.ZEye;
			float za = _radZ + _radDeltaZ * t;
			float xa = _radX + _radDeltaX * t;
			
			float i = Convert.ToSingle(Math.Sin(za) * Math.Cos(xa) * r + _centerXOrig);
			float j = Convert.ToSingle(Math.Sin(za) * Math.Sin(xa) * r + _centerYOrig);
			float k = Convert.ToSingle(Math.Cos(za) * r + _centerZOrig);
			
			Target.Camera.SetEye(i, j, k);
		}

		public override Action Reverse() {
			return new ReverseTime(this);
		}
		
	}
}
