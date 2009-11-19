
using System;
using NUnit.Framework;
using CocosNet;
using CocosNet.Vector;

namespace CocosNetUnitTests {


	[TestFixture()]
	public class RotationMatrixTests {

		[Test]
		public void Construction() {
			const float degrees = 30;
			
			RotationMatrix r = new RotationMatrix(30);
			
			Assert.AreEqual(degrees, r.Angle);
			
			Assert.AreEqual(4, r.Count, "RotationMatrix should have 4 entries");
			Assert.AreEqual(2, r.RowCount, "RotationMatrix should have 2 rows");
			Assert.AreEqual(2, r.ColumnCount, "RotationMatrix should have 2 columns");
			
			float radians = ExtensionMethods.ToRadians(degrees);
			
			float[] expectedValues = {
				(float)Math.Cos(radians),
				-(float)Math.Sin(radians),
				(float)Math.Sin(radians),
				(float)Math.Cos(radians)
			};
			
			for (int i = 0; i < expectedValues.Length; ++i) {
				Assert.AreEqual(expectedValues[i], r[i], "Value {0} is incorrect", i);
			}
		}
	}
}
