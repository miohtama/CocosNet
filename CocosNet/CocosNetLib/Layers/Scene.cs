// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using CocosNet.Base;

namespace CocosNet.Layers {
	public class Scene : CocosNode {

		public Scene() {
			SizeF s = Director.Instance.WinSize;
			
			IsRelativeAnchorPoint = false;
			AnchorPoint = new PointF(0.5f, 0.5f);
			ContentSize = s;
		}

		public Scene(CocosNode singleChild) : this() {
			if (singleChild == null) {
				throw new ArgumentNullException("singleChild");
			}
			
			AddChild(singleChild);
		}		
	}
}
