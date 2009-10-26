// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using Color = CocosNet.Base.Color;
using CocosNet.Support;
using OpenTK.Graphics.ES11;
using System.Drawing;

namespace CocosNet.Base {
	public abstract class AtlasNode : CocosNode {
		protected int _itemWidth;
		protected int _itemHeight;
		protected int _itemsPerColumn;
		protected int _itemsPerRow;
		protected float _texStepX;
		protected float _texStepY;

		public TextureAtlas TextureAtlas { get; set; }
		public Color Color { get; set; }
		public BlendFunc BlendFunc { get; set; }

		public Texture2D Texture {
			get { return TextureAtlas.Texture; }
			set {
				TextureAtlas.Texture = value;
				UpdateBlendFunc();
			}
		}

		private void CalculateMaxItems() {
			SizeF s = TextureAtlas.Texture.ContentSize;
			_itemsPerColumn = Convert.ToInt32(s.Height / _itemHeight);
			_itemsPerRow = Convert.ToInt32(s.Width / _itemWidth);
		}

		private void CalculateTexCoordsSteps() {
			_texStepX = _itemWidth / (float)TextureAtlas.Texture.PixelsWide;
			_texStepY = _itemHeight / (float)TextureAtlas.Texture.PixelsHigh;
		}

		protected abstract void UpdateAtlasValues();

		public AtlasNode(string tileFile, int itemWidth, int itemHeight, int itemsToRender) {
			_itemWidth = itemWidth;
			_itemHeight = itemHeight;
			
			Color = Color.White;
			
			BlendFunc = BlendFunc.DefaultBlendFunc;
			
			TextureAtlas = new TextureAtlas(tileFile, itemsToRender);
			
			UpdateBlendFunc();
			CalculateMaxItems();
			CalculateTexCoordsSteps();
		}

		public void UpdateBlendFunc() {
			if (!TextureAtlas.Texture.HasPremultipliedAlpha) {
				BlendFunc = new BlendFunc(All.SrcAlpha, All.OneMinusSrcAlpha);
			}
		}
	}
}
