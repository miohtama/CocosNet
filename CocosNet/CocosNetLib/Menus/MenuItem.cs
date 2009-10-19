// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CocosNet.Base;
using Color = CocosNet.Base.Color;

namespace CocosNet.Menus {
    public abstract class MenuItem : CocosNode {
		public event EventHandler Click;
		
		public MenuItem() {
			IsEnabled = true;
			AnchorPoint = new PointF(0.5f, 0.5f);
		}
		
		public RectangleF Rect {
			get { 
				return new RectangleF(Position.X - ContentSize.Width * AnchorPoint.X, 
					Position.Y - ContentSize.Height * AnchorPoint.Y, 
					ContentSize.Width, 
					ContentSize.Height);	
			}
		}
		
		public bool IsEnabled { get; set; }
		
		public Color Color { get; set; }
		
		public void Activate() {
			if (IsEnabled && Click != null) {
				Click(this, EventArgs.Empty);
			}
		}
		
		public abstract void OnSelected();
		public abstract void OnUnselected();
    }
}
