using System;
using System.Drawing;

namespace CocosNet.Actions {
	public struct BezierConfig {
		public PointF StartPosition { get; set; }
		public PointF EndPosition { get; set; }
		public PointF ControlPoint1 { get; set; }
		public PointF ControlPoint2 { get; set; }
		
		public BezierConfig(PointF start, PointF end, PointF cp1, PointF cp2) : this() {
			StartPosition = start;
			EndPosition = end;
			ControlPoint1 = cp1;
			ControlPoint2 = cp2;
		}
		
		public BezierConfig Negate() {
			BezierConfig ret = new BezierConfig();
			
			ret.StartPosition = StartPosition.Negate();
			ret.EndPosition = EndPosition.Negate();
			ret.ControlPoint1 = ControlPoint1.Negate();
			ret.ControlPoint2 = ControlPoint2.Negate();
			
			return ret;
		}
			
	}
}
