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
