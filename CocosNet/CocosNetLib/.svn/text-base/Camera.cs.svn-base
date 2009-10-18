using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.ES11;
using System.Drawing;
using CocosNet.Support;

namespace CocosNet {
	public class Camera {
		private float _eyeX, _eyeY, _eyeZ;
		private float _centerX, _centerY, _centerZ;
		private float _upX, _upY, _upZ;

		public static float ZEye {
			get {
				SizeF s = Director.Instance.DisplaySize;
				return s.Height / 1.1566f;
			}
		}

		public Camera() {
			Restore();
		}

		public bool Dirty { get; set; }

		public void Restore() {
			SizeF s = Director.Instance.DisplaySize;
			
			_eyeX = s.Width / 2;
			_eyeY = s.Height / 2;
			_eyeZ = ZEye;
			
			_centerX = s.Width / 2;
			_centerY = s.Height / 2;
			_centerZ = 0;
			
			_upX = 0;
			_upY = 1;
			_upZ = 0;
			
			Dirty = false;
		}

		public void Locate() {
			if (Dirty) {
				DeviceOrientation orientation = Director.Instance.DeviceOrientation;
				
				switch (orientation) {
					case DeviceOrientation.Portrait:
						break;
					case DeviceOrientation.PortraitUpsideDown:
						GL.Rotate(-180, 0, 0, 1);
						break;
					case DeviceOrientation.LandscapeLeft:
						GL.Rotate(-90, 0, 0, 1);
						break;
					case DeviceOrientation.LandscapeRight:
						GL.Rotate(90, 0, 0, 1);
						break;
				}
				
				GLU.LookAt(_eyeX, _eyeY, _eyeZ, _centerX, _centerY, _centerZ, _upX, _upY, _upZ);
				
				switch (orientation) {
					case DeviceOrientation.Portrait:
					case DeviceOrientation.PortraitUpsideDown:
						break;
					case DeviceOrientation.LandscapeLeft:
						GL.Translate(-80, 80, 0);
						break;
					case DeviceOrientation.LandscapeRight:
						GL.Translate(-80, 80, 0);
						break;
				}
			}
		}

		public void SetEye(float x, float y, float z) {
			_eyeX = x;
			_eyeY = y;
			_eyeZ = z;
		}

		public void GetEye(out float x, out float y, out float z) {
			x = _eyeX;
			y = _eyeY;
			z = _eyeZ;
		}

		public void SetCenter(float x, float y, float z) {
			_centerX = x;
			_centerY = y;
			_centerZ = z;
		}

		public void GetCenter(out float x, out float y, out float z) {
			x = _centerX;
			y = _centerY;
			z = _centerZ;
		}

		public void SetUp(float x, float y, float z) {
			_upX = x;
			_upY = y;
			_upZ = z;
		}

		public void GetUp(out float x, out float y, out float z) {
			x = _upX;
			y = _upY;
			z = _upZ;
		}
	}
}
