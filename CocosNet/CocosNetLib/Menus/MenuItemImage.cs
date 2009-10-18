// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosNet.Sprites;

namespace CocosNet.Menus {
	public class MenuItemImage : MenuItemSprite {
		public MenuItemImage(string normal, string selected) : this(normal, selected, null) {
		}
		
		public MenuItemImage(string normal, string selected, string disabled) 
			: base(normal == null ? null : new Sprite(normal),
					selected == null ? null : new Sprite(selected),
				disabled == null ? null : new Sprite(disabled)) {
		}
	}
}
