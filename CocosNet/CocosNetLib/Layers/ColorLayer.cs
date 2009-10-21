
using System;
using CocosNet.Base;
using System.Drawing;
using Color=CocosNet.Base.Color;
using OpenTK.Graphics.ES11;

namespace CocosNet.Layers {


	public class ColorLayer : Layer {
		private Color _color;
		private byte[] _squareColors;
		private float[] _squareVertices;
		
		private void UpdateColor() {
			for (int i = 0; i < 4; ++i) {
				_squareColors[i * 4] = _color.R;
				_squareColors[i * 4 + 1] = _color.G;
				_squareColors[i * 4 + 2] = _color.B;
				_squareColors[i * 4 + 3] = _color.A;
			}
		}
		
		public ColorLayer() : this(Colors.Black) {
		}
		
		public ColorLayer(Color color) : this(color, Director.Instance.WinSize) {
		}
		
		public ColorLayer(Color color, SizeF contentSize) {
			_squareColors = new byte[4 * 4];
			
			ContentSize = contentSize;
			Color = color;
		}
		
		public override SizeF ContentSize {
			get {
				return base.ContentSize;
			}
			set {
				if (_squareVertices == null) {
					_squareVertices = new float[4 * 2];
				}
				
				_squareVertices[2] = value.Width;
				_squareVertices[5] = value.Height;
				_squareVertices[6] = value.Width;
				_squareVertices[7] = value.Height;
				
				base.ContentSize = value;
			}
		}
		
		public Color Color {
			get {
				return _color;
			}
			set {
				_color = value;
				UpdateColor();
			}
		}
		
		public override void Draw()
		{
			GL.VertexPointer(2, All.Float, 0, _squareVertices);
			GL.EnableClientState(All.VertexArray);
			GL.ColorPointer(4, All.UnsignedByte, 0, _squareColors);
			GL.EnableClientState(All.ColorArray);
			
			if (_color.A != 255)
				GL.BlendFunc(All.SrcAlpha, All.OneMinusSrcAlpha);
			
			GL.DrawArrays(All.TriangleStrip, 0, 4);
			
			if (_color.A != 255)
				GL.BlendFunc(BlendFunc.DefaultBlendSrc, BlendFunc.DefaultBlendDst);
			
			GL.DisableClientState(All.VertexArray);
			GL.DisableClientState(All.ColorArray);
		}

	}
}
