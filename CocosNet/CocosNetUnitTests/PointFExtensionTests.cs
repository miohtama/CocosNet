
using System;
using NUnit.Framework;
using CocosNet;
using CocosNet.Vector;
using System.Drawing;

//
// NOTE:
// MonoDevelop when used for MonoTouch doesn't officially support NUnit projects.
// The fact that they work is actually be a lucky accident. Novell has indicated they
// would like to make this officially supported in the future. In the mean time, there
// is one limitation, despite referencing CocosNetLib, this project cannot see any
// extension methods defined in CocosNetLib as extensions methods, they only come through 
// as standard static methods.
//
// For PointF, the extension methods must be called here as standard static methods until this
// issue gets resolved.
//


namespace CocosNetUnitTests {


	[TestFixture()]
	public class PointFExtensionTests {

		[Test]
		public void Add() {
			const float ax = 100;
			const float ay = 245;
			const float bbx = 35.22f;
			const float bby = 5634.225f;
			
			PointF a = new PointF(ax, ay);
			PointF bb = new PointF(bbx, bby);
			
			PointF result = PointFExtensions.Add(a, bb);
			
			Assert.AreEqual(ax + bbx, result.X, "Add: X result is wrong");
			Assert.AreEqual(ay + bby, result.Y, "Add: Y result is wrong");
			
			result = PointFExtensions.Add(bb, a);
			
			Assert.AreEqual(ax + bbx, result.X, "Add: X result is wrong");
			Assert.AreEqual(ay + bby, result.Y, "Add: Y result is wrong");
		}

		[Test]
		public void Subtract() {
			const float ax = 100;
			const float ay = 245;
			const float bbx = 35.22f;
			const float bby = 5634.225f;
			
			PointF a = new PointF(ax, ay);
			PointF bb = new PointF(bbx, bby);
			
			PointF result = PointFExtensions.Subtract(a, bb);
			
			Assert.AreEqual(ax - bbx, result.X, "Add: X result is wrong");
			Assert.AreEqual(ay - bby, result.Y, "Add: Y result is wrong");
			
			result = PointFExtensions.Subtract(bb, a);
			
			Assert.AreEqual(bbx - ax, result.X, "Add: X result is wrong");
			Assert.AreEqual(bby - ay, result.Y, "Add: Y result is wrong");
		}
		
		[Test]
		public void Negate() {
			const float x = 340.23434f;
			const float y = 34546.2323f;
			
			PointF p = new PointF(x,y);
			
			PointF expected = new PointF(-x, -y);
			
			Assert.AreEqual(expected, PointFExtensions.Negate(p), "Negate returned unexpected result");
		}
		
		[Test]
		public void Rotate() {
			// first rotate 90 degrees
			// which is basically just flip-flop the values, but due to rounding issues
			// the resulting point needs to be (100 * (float)Math.Cos(radians), 100 * (float)Math.Sin(radians))
			PointF p = new PointF(100, 0);
			
			PointF result = PointFExtensions.Rotate(p, 90);
			float radians = ExtensionMethods.ToRadians(90.0f);
			Assert.AreEqual(new PointF(100 * (float)Math.Cos(radians), 100 * (float)Math.Sin(radians)), result);
			
			// now rotate by 45 degrees. In a 45 degree right triangle, the sides are 1,1,sqrt(2)
			// so if the hypotenuse ends up being 100, then the other two sides are 100 / sqrt(2)
			result = PointFExtensions.Rotate(p, 45);
			
			float root2 = (float)Math.Sqrt(2);
			
			Assert.AreEqual(new PointF(100.0f / root2, 100.0f /root2), result);
		}
		
		[Test]
		public void RotateAround() {
			PointF anchor = new PointF(50, 50);
			
			PointF rotatee = new PointF(100, 50);
			
			// seems like it should be anchor, rotater, 45
			// but remember this is an extension method it'd really be called rotatee.RotateAround(anchor, 45)
			PointF result = PointFExtensions.RotateAround(rotatee, anchor, 45);
			
			float root2 = (float)Math.Sqrt(2);
			float side = 50 / root2;
			float x = side + 50;
			float y = side + 50;
			
			Assert.AreEqual(new PointF(x, y), result);
		}
		
	}
}
