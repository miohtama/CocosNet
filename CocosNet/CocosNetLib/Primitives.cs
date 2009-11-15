
using System;
using System.Drawing;
using CocosNet.Base;
using OpenTK.Graphics.ES11;

namespace CocosNet {


	public static class Primitives {
		private static Vertex2F[] PointsToVertices(PointF[] points) {
			Vertex2F[] vertices = new Vertex2F[points.Length];
			for (int i = 0; i < points.Length; ++i) {
				vertices[i] = points[i].ToVertex2F();
			}
			
			return vertices;
		}
		
		public static void DrawPoint(PointF point) {			
			Vertex2F[] vertex = { point.ToVertex2F() };
			
			GL.VertexPointer(2, All.Float, 0, vertex);
			GL.EnableClientState(All.VertexArray);
			
			GL.DrawArrays(All.Points, 0, 1);
			
			GL.DisableClientState(All.VertexArray);
		}

		public static void DrawPoints(PointF[] points) {
			if (points == null) {
				throw new ArgumentNullException("points");
			}
			
			Vertex2F[] vertices = PointsToVertices(points);
			
			GL.VertexPointer(2, All.Float, 0, vertices);
			GL.EnableClientState(All.VertexArray);
			
			GL.DrawArrays(All.Points, 0, vertices.Length);
			
			GL.DisableClientState(All.VertexArray);
		}

		public static void DrawLine(PointF origin, PointF destination) {
			Vertex2F[] vertices = {
				origin.ToVertex2F(),
				destination.ToVertex2F()
			};
			
			GL.VertexPointer(2, All.Float, 0, vertices);
			GL.EnableClientState(All.VertexArray);
			
			GL.DrawArrays(All.Lines, 0, 2);
			
			GL.DisableClientState(All.VertexArray);
		}

		public static void DrawPoly(PointF[] points, bool closePolygon) {
			if (points == null) {
				throw new ArgumentNullException("points");
			}
			
			Vertex2F[] poli = PointsToVertices(points);
			
			GL.VertexPointer(2, All.Float, 0, poli);
			GL.EnableClientState(All.VertexArray);
			
			if (closePolygon) {
				GL.DrawArrays(All.LineLoop, 0, poli.Length);
			} else {
				GL.DrawArrays(All.LineStrip, 0, poli.Length);
			}
			
			GL.DisableClientState(All.VertexArray);
		}

		public static void DrawCircle(PointF center, float radius, float a, int segments, bool drawLineToCenter) {
			int additionalSegment = drawLineToCenter ? 2 : 1;
			
			float coef = 2f * (float)Math.PI / segments;
			
			Vertex2F[] vertices = new Vertex2F[segments + 2];
			
			for (int i = 0; i <= segments; ++i) {
				float rads = i * coef;
				float j = radius * (float)Math.Cos(rads + a) + center.X;
				float k = radius * (float)Math.Sin(rads + a) + center.Y;
				
				vertices[i].X = j;
				vertices[i].Y = k;
			}
			vertices[segments + 1] = center.ToVertex2F();
			
			GL.VertexPointer(2, All.Float, 0, vertices);
			GL.EnableClientState(All.VertexArray);
			
			GL.DrawArrays(All.LineStrip, 0, segments + additionalSegment);
			
			GL.DisableClientState(All.VertexArray);
		}

		public static void DrawQuadBezier(PointF origin, PointF control, PointF destination, int segments) {
			Vertex2F[] vertices = new Vertex2F[segments + 1];
			
			float t = 0;
			for (int i = 0; i < segments; ++i) {
				float x = (float)Math.Pow(1 - t, 2) * origin.X + 2f * (1 - t) * t * control.X + t * t * destination.X;
				float y = (float)Math.Pow(1 - t, 2) * origin.Y + 2f * (1 - t) * t * control.Y + t * t * destination.Y;
				
				vertices[i].X = x;
				vertices[i].Y = y;
				t += 1f / segments;
			}
			
			vertices[segments] = destination.ToVertex2F();
			
			GL.VertexPointer(2, All.Float, 0, vertices);
			GL.EnableClientState(All.VertexArray);
			GL.DrawArrays(All.LineStrip, 0, segments+1);
			GL.DisableClientState(All.VertexArray);
		}
		
		public static void DrawCubicBezier(PointF origin, PointF control1, PointF control2, PointF destination, int segments) {
			Vertex2F[] vertices = new Vertex2F[segments + 1];
			
			float t = 0;
			for (int i = 0; i < segments; ++i) {
				float x = (float)Math.Pow(1 - t, 3) * origin.X + 3.0f * (float)Math.Pow(1 - t, 2) * t * control1.X + 3.0f * (1 - t) * t * t * control2.X + t * t * t * destination.X;
				float y = (float)Math.Pow(1 - t, 3) * origin.Y + 3.0f * (float)Math.Pow(1 - t, 2) * t * control1.Y + 3.0f * (1 - t) * t * t * control2.Y + t * t * t * destination.Y;
				
				vertices[i].X = x;
				vertices[i].Y = y;
				t += 1f / segments;
			}
			
			vertices[segments] = destination.ToVertex2F();
			
			GL.VertexPointer(2, All.Float, 0, vertices);
			GL.EnableClientState(All.VertexArray);
			GL.DrawArrays(All.LineStrip, 0, segments + 1);
			GL.DisableClientState(All.VertexArray);
		}
	}
}
