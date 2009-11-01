
using System;
using OpenTK.Graphics.ES11;
using System.Drawing;
using CocosNet;

namespace CocosNetTests {

	public abstract class DrawPrimitivesBase : TestBase {
		protected override ICloneable[] Scenes {
			get { return new ICloneable[] {new HorizontalDrawPrimitives(),new VerticalDrawPrimitives()}; }
		}

		public DrawPrimitivesBase(DeviceOrientation orientation) : base(orientation) {
		}
		
		public override void Draw() {
			SizeF s = Director.Instance.WinSize;
			
			GL.Enable(All.LineSmooth);
			Primitives.DrawLine(PointF.Empty, new PointF(s.Width, s.Height));
			
			
			GL.Disable(All.LineSmooth);
			GL.LineWidth(5f);
			GL.Color4(255, 0, 0, 255);
			Primitives.DrawLine(new PointF(0, s.Height), new PointF(s.Width, 0));
			
			// draw big point in the center
			GL.PointSize(64);
			GL.Color4(0, 0, 255, 128);
			Primitives.DrawPoint(new PointF(s.Width / 2, s.Height / 2));
			
			// draw 4 small points
			PointF[] points = {
				new PointF(60, 60),
				new PointF(70, 70),
				new PointF(60, 70),
				new PointF(70, 60)
			};
			GL.PointSize(4);
			GL.Color4(0, 255, 255, 255);
			Primitives.DrawPoints(points);
			
			// draw a green circle with 10 segments
			GL.LineWidth(16);
			GL.Color4(0, 255, 0, 255);
			Primitives.DrawCircle(new PointF(s.Width / 2, s.Height / 2), 100, 0, 10, false);
			
			// draw a green circle with 50 segments with line to center
			GL.LineWidth(2);
			GL.Color4(0, 255, 255, 255);
			Primitives.DrawCircle(new PointF(s.Width / 2, s.Height / 2), 50, 90.ToRadians(), 50, true);
			
			// open yellow poly
			GL.Color4(255, 255, 0, 255);
			GL.LineWidth(10);
			PointF[] vertices = {
				new PointF(0, 0),
				new PointF(50, 50),
				new PointF(100, 50),
				new PointF(100, 100),
				new PointF(50, 100)
			};
			Primitives.DrawPoly(vertices, false);
			
			// closed purble poly
			GL.Color4(255, 0, 255, 255);
			GL.LineWidth(2);
			PointF[] vertices2 = {
				new PointF(30, 130),
				new PointF(30, 230),
				new PointF(50, 200)
			};
			Primitives.DrawPoly(vertices2, true);
			
			// draw quad bezier path
			Primitives.DrawQuadBezier(new PointF(0, s.Height), new PointF(s.Width / 2, s.Height / 2), new PointF(s.Width, s.Height), 50);
			
			// draw cubic bezier path
			Primitives.DrawCubicBezier(new PointF(s.Width / 2, s.Height / 2), new PointF(s.Width / 2 + 30, s.Height / 2 + 50), new PointF(s.Width / 2 + 60, s.Height / 2 - 50), new PointF(s.Width, s.Height / 2), 100);
			
			
			// restore original values
			GL.LineWidth(1);
			GL.Color4(255, 255, 255, 255);
			GL.PointSize(1);
		}

		public override string ToString() {
			return "DrawPrimitives";
		}
	}

	public class HorizontalDrawPrimitives : DrawPrimitivesBase {
		public HorizontalDrawPrimitives() : base(DeviceOrientation.LandscapeLeft) {
		}

		public override object Clone() {
			return new HorizontalDrawPrimitives();
		}
		
	}

	public class VerticalDrawPrimitives : DrawPrimitivesBase {
		public VerticalDrawPrimitives() : base(DeviceOrientation.Portrait) {
		}

		public override object Clone() {
			return new VerticalDrawPrimitives();
		}
	}
}
