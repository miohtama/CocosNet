// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.ES11;
using System.Drawing;
using CocosNet.Support;
using Color = CocosNet.Base.Color;

namespace CocosNet.Base {
	public class TextureNode : CocosNode, IDisposable {
		private Texture2D _texture;
		private Color _color = new Color();

		public BlendFunc BlendFunc { get; set; }
		
		public Color Color {
			get { 
				return _color; 
			}
			set { 
				_color = value; 
			}
		}
		
		public bool OpacityModifyRgb { get; set; }
		
		public TextureNode() {
			Color = Colors.White;
			AnchorPoint = new PointF(0.5f, 0.5f);
			BlendFunc = BlendFunc.DefaultBlendFunc;
		}
		
		~TextureNode() {
			Dispose();
		}

		public Texture2D Texture {
			get { return _texture; }
			set {
				_texture = value;
				ContentSize = _texture.ContentSize;
				if (!_texture.HasPremultipliedAlpha) {;
					BlendFunc = new BlendFunc(All.SrcAlpha, All.OneMinusSrcAlpha);
				}
				
				OpacityModifyRgb = _texture.HasPremultipliedAlpha;
			}
		}
		
		public byte Opacity {
			get { 
				return Color.A; 
			}
			set {
				_color.A = value;
			}
		}

		public void SetRgb(byte r, byte g, byte b) {;
			_color.R = r;
			_color.G = g;
			_color.B = b;
		}
		
		public override void Draw() {
			GL.EnableClientState(All.VertexArray);
			GL.EnableClientState(All.TextureCoordArray);
			GL.Enable(All.Texture2D);
			GL.Color4(Color.R, Color.G, Color.B, Color.A);
			
			bool newBlend = false;
			
			if (!BlendFunc.IsDefault) {
				newBlend = true;
				GL.BlendFunc(BlendFunc.Src, BlendFunc.Dst);
			}
			
			Texture.DrawAtPoint(PointF.Empty);
			
			if (newBlend) {
				GL.BlendFunc(BlendFunc.DefaultBlendSrc, BlendFunc.DefaultBlendDst);
			}
			
			GL.Color4(255, 255, 255, 255);
			
			GL.Disable(All.Texture2D);
			GL.DisableClientState(All.VertexArray);
			GL.DisableClientState(All.TextureCoordArray);
		}

		public void Dispose() {
			if (_texture != null) {
				_texture.Dispose();
				GC.SuppressFinalize(this);;
			}
		}
	}
}
