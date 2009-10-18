// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosNet.Base;
using CocosNet.Support;

namespace CocosNet.Sprites {
    public class Sprite : TextureNode {
		public Sprite(Texture2D tex) {
			Texture = tex;
		}
		
        public Sprite(string fileName) {
        		Texture = TextureMgr.Instance.AddImage(fileName);	
        }
    }
}
