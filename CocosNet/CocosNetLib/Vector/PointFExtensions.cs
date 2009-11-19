
using System;
using System.Drawing;
using CocosNet.Base;

namespace CocosNet.Vector {


	public static class PointFExtensions {
		public static float GetDegreeAngleFrom(this PointF p, PointF other) {
			return p.GetRadianAngleFrom(other).ToDegrees();
		}

		private const float Pi = (float)Math.PI;
		private const float PiOver2 = Pi / 2f;
		private const float TwoPi = 2f * Pi;

		public static float GetRadianAngleFrom(this PointF p, PointF other) {
			float x = p.X - other.X;
			float y = p.Y - other.Y;
			
			// special case for x equals zero, division by zero
			if (x == 0f) {
				if (y < 0f)
					return PiOver2 * 3;
				else
					return PiOver2;
			}
			
			// special case for y equals zero, the negative gets lost
			if (y == 0f) {
				if (x < 0f)
					return Pi;
				else
					return 0;
			}
			
			float tan = y / x;
			float radians = (float)Math.Atan(tan);
			
			// quad 1
			if (x > 0 && y > 0) {
				return radians;
			
			// quad 2, add 180
			} else if (x < 0 && y > 0) {
				return radians + Pi;
			
			// quad 3, add 180
			} else if (x < 0 && y < 0) {
				return radians + Pi;
			
			// if(x > 0 && y < 0) {
			} else {
				return radians + TwoPi;
			}
		}

		public static PointF Extrude(this PointF p, float angle, float length) {
			float rads = angle.ToRadians();
			float x = length * (float)Math.Cos(rads);
			float y = length * (float)Math.Sin(rads);
			
			
			PointF ret = new PointF(p.X + x, p.Y + y);
			return ret;
		}


		public static PointF Negate(this PointF p) {
			return new PointF(-p.X, -p.Y);
		}

		public static PointF Subtract(this PointF p, PointF other) {
			p.X -= other.X;
			p.Y -= other.Y;
			return p;
		}

		public static PointF Add(this PointF p, PointF other) {
			p.X += other.X;
			p.Y += other.Y;
			return p;
		}

		public static PointF Multiply(this PointF p, float multiplier) {
			return new PointF(p.X * multiplier, p.Y * multiplier);
		}

		public static float Dot(this PointF p, PointF other) {
			return p.X * other.X + p.Y * other.Y;
		}

		public static float Length(this PointF p) {
			return (float)Math.Sqrt(p.Dot(p));
		}

		public static PointF Normalize(this PointF p) {
			return p.Multiply(1f / p.Length());
		}

		unsafe private static float InvsSqrt(float x) {
			float xhalf = 0.5f * x;
			int i = *(int*)&x;
			i = 0x5f375a86 - (i >> 1);
			x = *(float*)&i;
			x = x * (1.5f - xhalf * x * x);
			
			return x;
		}

		public static PointF NormalizeFast(this PointF p) {
			float inv = InvsSqrt(p.X * p.X + p.Y * p.Y);
			p.X *= inv;
			p.Y *= inv;
			
			return p;
		}

		public static Vertex2F ToVertex2F(this PointF p) {
			Vertex2F v = new Vertex2F();
			v.X = p.X;
			v.Y = p.Y;
			
			return v;
		}

		public static float Distance(this PointF p, PointF other) {
			return p.Subtract(other).Length();
		}

		public static PointF Translate(this PointF p, PointF delta) {
			p.X += delta.X;
			p.Y += delta.Y;
			
			return p;
		}
		
		public static PointF Rotate(this PointF p, float angle) {
			RotationMatrix r = new RotationMatrix(angle);
			
			float x = r[0] * p.X + r[1] * p.Y;
			float y = r[2] * p.X + r[3] * p.Y;
			
			return new PointF(x, y);
		}
		
		public static PointF RotateAround(this PointF p, PointF anchor, float angle) {
			p = p.Subtract(anchor);
			
			p = p.Rotate(angle);
			
			p = p.Add(anchor);
		
			return p;
		}
	}
}
