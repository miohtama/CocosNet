// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using System.Collections.Generic;
using MonoTouch.UIKit;
using CocosNet.Support;

namespace CocosNet {


	public class TextureMgr {
		private static TextureMgr _instance;
		
		public static TextureMgr Instance {
			get {
				if (_instance == null) {
					_instance = new TextureMgr();
				}
				
				return _instance;
			}
		}
		
		private object _lock;
		private Dictionary<string, Texture2D> _cache;
		
		private TextureMgr() {
			_lock = new object();
			_cache = new Dictionary<string, Texture2D>();
		}
		
		public void RemoveAllTextures() {
			_cache.Clear();
		}
		
		public Texture2D AddImage(string fileName) {
			if (fileName == null) {
				throw new ArgumentNullException("fileName");
			}
			
			Texture2D tex = null;
			
			lock (_lock) {
				if (_cache.ContainsKey(fileName)) {
					tex = _cache[fileName];
				} else {
					UIImage image = UIImage.FromFile(fileName);
					if (image == null) {
						throw new ArgumentException("Could not find image: " + fileName);
					}
					
					tex = new Texture2D(image);
					_cache.Add(fileName, tex);
				}
			}
			
			return tex;
		}
	}
}
