// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using CocosNet.Base;
using OpenTK.Graphics.ES11;
using System.Drawing;

namespace CocosNet.Labels {
	public class LabelAtlas : AtlasNode {
		private string _text;
		private char _mapStartChar;

		private void SetText(string text) {
			_text = text;
			
			UpdateAtlasValues();
			
			ContentSize = new SizeF(_text.Length * _itemWidth, _itemHeight);
			
		}

		public string Text {
			get { return _text; }
			set { SetText(value); }
		}
		
		protected override void UpdateAtlasValues() {
			GLPointQuad3F quad = new GLPointQuad3F();
			
			for (int i = 0; i < _text.Length; i++) {
				char a = (char)(_text[i] - _mapStartChar);
				float row = (a % _itemsPerRow) * _texStepX;
				float col = (a / _itemsPerRow) * _texStepY;
				
				quad.TL.TexCoords.U = row;
				quad.TL.TexCoords.V = col;
				quad.TR.TexCoords.U = row + _texStepX;
				quad.TR.TexCoords.V = col;
				quad.BL.TexCoords.U = row;
				quad.BL.TexCoords.V = col + _texStepY;
				quad.BR.TexCoords.U = row + _texStepX;
				quad.BR.TexCoords.V = col + _texStepY;
				
				quad.BL.Vertex.X = (int)(i * _itemWidth);
				quad.BL.Vertex.Y = 0;
				quad.BL.Vertex.Z = 0f;
				quad.BR.Vertex.X = (int)(i * _itemWidth + _itemWidth);
				quad.BR.Vertex.Y = 0;
				quad.BR.Vertex.Z = 0f;
				quad.TL.Vertex.X = (int)(i * _itemWidth);
				quad.TL.Vertex.Y = (int)(_itemHeight);
				quad.TL.Vertex.Z = 0f;
				quad.TR.Vertex.X = (int)(i * _itemWidth + _itemWidth);
				quad.TR.Vertex.Y = (int)(_itemHeight);
				quad.TR.Vertex.Z = 0f;
				
				TextureAtlas.Quads[i] = quad;
			}
		}


		public LabelAtlas(string text, string charMapFile, int itemWidth, int itemHeight, char mapStartChar) : base(charMapFile, itemWidth, itemHeight, text.Length) {
			_mapStartChar = mapStartChar;
		}

		public override void Draw() {
			GL.EnableClientState(All.VertexArray);
			GL.EnableClientState(All.TextureCoordArray);
			
			GL.Enable(All.Texture2D);
			
			GL.Color4(Color.R, Color.G, Color.B, Color.A);
			
			bool newBlend = false;
			if (BlendFunc.Src != All.BlendSrc || BlendFunc.Dst != All.BlendDst) {
				newBlend = true;
				GL.BlendFunc(BlendFunc.Src, BlendFunc.Dst);
			}
			
			TextureAtlas.DrawQuads(_text.Length);
			
			if (newBlend) {
				GL.BlendFunc(All.BlendSrc, All.BlendDst);
			}
			
			GL.Color4(255, 255, 255, 255);
			
			GL.Disable(All.Texture2D);
			
			GL.DisableClientState(All.VertexArray);
			GL.DisableClientState(All.TextureCoordArray);
			
		}
	}
}
