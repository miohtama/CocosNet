
using System;

namespace CocosNet.Vector {


	public class RotationMatrix {
		private float[] _values;

		public RotationMatrix(float angle) {
			Angle = angle;
			
			float radians = Angle.ToRadians();
			
			_values = new float[] {
				(float)Math.Cos(radians),
				-(float)Math.Sin(radians),
				(float)Math.Sin(radians),
				(float)Math.Cos(radians)
			}; 
		}
		
		public float Angle { get; private set; }
		public int Count {
			get { return 4; }
		}
		
		public int RowCount {
			get {
				return 2;
			}
		}
		
		public int ColumnCount {
			get {
				return 2;
			}
		}
		
		public float this[int i] {
			get {
				return _values[i];
			}
		}
	}
}
