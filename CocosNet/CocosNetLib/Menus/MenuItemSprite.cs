// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosNet.Base;

namespace CocosNet.Menus {
	public class MenuItemSprite : MenuItem {
		private bool _selected;

		public CocosNode NormalImage { get; set; }
		public CocosNode SelectedImage { get; set; }
		public CocosNode DisabledImage { get; set; }

		public MenuItemSprite(CocosNode normal, CocosNode selected, CocosNode disabled) {
			if (normal == null) {
				throw new ArgumentNullException("normal");
			}
			if (selected == null) {
				throw new ArgumentNullException("selected");
			}
			
			// disabled is allowed to be null
			
			NormalImage = normal;
			SelectedImage = selected;
			DisabledImage = disabled;
			
			ContentSize = NormalImage.ContentSize;
		}

		public override void OnEnter() {
			base.OnEnter();
			
			NormalImage.OnEnter();
			if (!ReferenceEquals(NormalImage, SelectedImage)) {
				SelectedImage.OnEnter();
			}
			
			if (DisabledImage != null) {
				DisabledImage.OnEnter();
			}
		}
		
		public override void OnExit() {
			base.OnExit();
			
			NormalImage.OnExit();
			if (!ReferenceEquals(NormalImage, SelectedImage)) {
				SelectedImage.OnExit();
			}
			
			if (DisabledImage != null) {
				DisabledImage.OnExit();
			}
		}
		
		public override void OnSelected() {
			_selected = true;
		}

		public override void OnUnselected() {
			_selected = true;
		}

		public override void Draw() {		
			if (IsEnabled) {
				if (_selected) {
					SelectedImage.Draw();
				} else {
					NormalImage.Draw();
				}
			} else {
				if (DisabledImage != null) {
					DisabledImage.Draw();
				} else {
					NormalImage.Draw();
				}
			}
		}
	}
}
