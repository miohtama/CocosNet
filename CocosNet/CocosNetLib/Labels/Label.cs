// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MonoTouch.UIKit;
using CocosNet.Base;
using CocosNet.Support;

namespace CocosNet.Labels {
    public class Label : TextureNode {
		private SizeF _dimensions;
		private UITextAlignment _alignment;
		private string _fontName;
		private float _fontSize;
		
		public Label(string text, string fontName, float fontSize) {
			_dimensions = SizeF.Empty;
			_fontName = fontName;
			_fontSize = fontSize;
			
			SetText(text);
		}
		
		public void SetText(string text) {
			if (_dimensions.IsEmpty) {
				Texture = new Texture2D(text, _fontName, _fontSize);
			} else {
				Texture = new Texture2D(text, _dimensions, _alignment, _fontName, _fontSize);	
			}
		}
    }
}
