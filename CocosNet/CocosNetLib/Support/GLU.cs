// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using OpenTK.Graphics.ES11;

namespace CocosNet.Support {


	public static class GLU {
		public static void Perspective(float fovy, float aspect, float zNear, float zFar) {
			
			float ymax = zNear * (float)Math.Tan(fovy * (float)Math.PI / 360);
			float ymin = -ymax;
			float xmin = ymin * aspect;
			float xmax = ymax * aspect;
			
			GL.Frustum(xmin, xmax, ymin, ymax, zNear, zFar);
		}


		public static void LookAt(float eyex, float eyey, float eyez, float centerx, float centery, float centerz, float upx, float upy, float upz) {
			float[] m = new float[16];
			float[] x = new float[3];
			float[] y = new float[3];
			float[] z = new float[3];
			float mag;
			
						/* Make rotation matrix */

			/* Z vector */
z[0] = eyex - centerx;
			z[1] = eyey - centery;
			z[2] = eyez - centerz;
			mag = (float)Math.Sqrt(z[0] * z[0] + z[1] * z[1] + z[2] * z[2]);
			if (mag != 0) {
				z[0] /= mag;
				z[1] /= mag;
				z[2] /= mag;
			}
			
						/* Y vector */
y[0] = upx;
			y[1] = upy;
			y[2] = upz;
			
						/* X vector = Y cross Z */
x[0] = y[1] * z[2] - y[2] * z[1];
			x[1] = -y[0] * z[2] + y[2] * z[0];
			x[2] = y[0] * z[1] - y[1] * z[0];
			
						/* Recompute Y = Z cross X */
y[0] = z[1] * x[2] - z[2] * x[1];
			y[1] = -z[0] * x[2] + z[2] * x[0];
			y[2] = z[0] * x[1] - z[1] * x[0];
			
						/* cross product gives area of parallelogram, which is < 1.0 for
     * non-perpendicular unit-length vectors; so normalize x, y here
     */

mag = (float)Math.Sqrt(x[0] * x[0] + x[1] * x[1] + x[2] * x[2]);
			if (mag != 0) {
				x[0] /= mag;
				x[1] /= mag;
				x[2] /= mag;
			}
			
			mag = (float)Math.Sqrt(y[0] * y[0] + y[1] * y[1] + y[2] * y[2]);
			if (mag != 0) {
				y[0] /= mag;
				y[1] /= mag;
				y[2] /= mag;
			}
			
			M(0, 0, x[0], m);
			M(0, 1, x[1], m);
			M(0, 2, x[2], m);
			M(0, 3, 0f, m);
			M(1, 0, y[0], m);
			M(1, 1, y[1], m);
			M(1, 2, y[2], m);
			M(1, 3, 0f, m);
			M(2, 0, z[0], m);
			M(2, 1, z[1], m);
			M(2, 2, z[2], m);
			M(2, 3, 0f, m);
			M(3, 0, 0f, m);
			M(3, 1, 0f, m);
			M(3, 2, 0f, m);
			M(3, 3, 1f, m);
			
			
			float[] fixedM = new float[16];
			for (int a = 0; a < 16; ++a) {
				fixedM[a] = m[a];
			}
			
			GL.MultMatrix(fixedM);
			
			
			
						/* Translate Eye to Origin */
GL.Translate(-eyex, -eyey, -eyez);
		}

		private static void M(int row, int col, float value, float[] data) {
			data[col * 4 + row] = value;
		}
	}
}
