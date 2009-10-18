// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CocosNet {
    public class GridBase {
	
		public bool Active { get; set; }
		
		public void BeforeDraw() {
		}
		
		public void AfterDraw(Camera camera) {
		}
    }
}
