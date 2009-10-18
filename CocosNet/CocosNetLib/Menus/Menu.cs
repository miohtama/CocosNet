// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;
using System.Drawing;
using CocosNet.Base;
using CocosNet.Layers;
using CocosNet.Menus;
using CocosNet.Support;
using Color = CocosNet.Base.Color;

namespace CocosNet.Menus {
	public class Menu : Layer, ITargetedTouchDelegate {
		enum MenuState {
			Waiting,
			TrackingTouch
		}

		public const int DefaultPadding = 5;

		private MenuState _state;
		private MenuItem _selectedItem;
		private Color _color;

		private MenuItem ItemForTouch(UITouch touch) {
			PointF touchLocation = touch.LocationInView(touch.View);
			touchLocation = Director.Instance.ConvertCoordinate(touchLocation);
			
			foreach (MenuItem item in Children) {
				PointF local = item.ConvertToNodeSpace(touchLocation);
				
				RectangleF r = item.Rect;
				r.X = 0;
				r.Y = 0;
				
				if (r.Contains(local)) {
					return item;
				}
			}
			
			return null;
		}

		public Color Color {
			get { return _color; }
			set {
				_color = value;
				foreach (MenuItem item in Children) {
					item.Color = _color;
				}
			}
		}

		public Menu(params MenuItem[] items) {
			IsTouchEnabled = true;
			SizeF s = Director.Instance.WinSize;
			IsRelativeAnchorPoint = false;
			
			AnchorPoint = new PointF(0.5f, 0.5f);
			ContentSize = s;
			
			RectangleF r = UIApplication.SharedApplication.StatusBarFrame;
			DeviceOrientation orientation = Director.Instance.DeviceOrientation;
			
			if (orientation == DeviceOrientation.LandscapeLeft || orientation == DeviceOrientation.LandscapeRight) {
				s.Height -= r.Width;
			} else {
				s.Height -= r.Height;
			}
			
			Position = new PointF(s.Width / 2f, s.Height / 2f);
			
			int z = 0;
			
			foreach (MenuItem item in items) {
				AddChild(item, z);
				++z;
			}
			
			_state = MenuState.Waiting;
			_selectedItem = null;
		}

		public override CocosNode AddChild(CocosNode child, int z, int tag) {
			if (!(child is MenuItem)) {
				throw new ArgumentException("Can only add menu items to a menu", "child");
			}
			
			return base.AddChild(child, z, tag);
		}

		public void AlignItemsVertically() {
			AlignItemsVertically(DefaultPadding);
		}

		public void AlignItemsVertically(float padding) {
			float height = -padding;
			foreach (MenuItem item in Children) {
				height += item.ContentSize.Height * item.ScaleY + padding;
			}
			
			float y = height / 2f;
			
			foreach (MenuItem item in Children) {
				item.Position = new PointF(0, y - item.ContentSize.Height * item.ScaleY / 2f);
				y -= item.ContentSize.Height * item.ScaleY * padding;
			}
		}

		public void AlignItemsHorizontally() {
			AlignItemsHorizontally(DefaultPadding);
		}

		public void AlignItemsHorizontally(float padding) {
			float width = -padding;
			foreach (MenuItem item in Children) {
				width += item.ContentSize.Width * item.ScaleX + padding;
			}
			
			float x = -width / 2f;
			
			foreach (MenuItem item in Children) {
				item.Position = new PointF(x + item.ContentSize.Width * item.ScaleX / 2f, 0);
				x += item.ContentSize.Width * item.ScaleX + padding;
			}
		}

		#region Touch Events

		public override void RegisterWithTouchDispatcher() {
			TouchDispatcher.Instance.AddTargetedDelegate(this, int.MinValue + 1, true);
		}

		public bool TouchBegan(UITouch touch, UIEvent evnt) {
			if (_state != MenuState.Waiting) {
				return false;
			}
			
			_selectedItem = ItemForTouch(touch);
			
			if (_selectedItem != null) {
				_selectedItem.OnSelected();
				_state = MenuState.TrackingTouch;
				
				return true;
			}
			
			return false;
		}

		public void TouchMoved(UITouch touch, UIEvent evnt) {
			Debug.Assert(_state == MenuState.TrackingTouch, "Menu.TouchEnded, invalid state");
			
			MenuItem currentItem = ItemForTouch(touch);
			
			if (currentItem != _selectedItem) {
				if (_selectedItem != null) {
					_selectedItem.OnUnselected();
				}
				
				_selectedItem = currentItem;
				_selectedItem.OnSelected();
			}
		}

		public void TouchEnded(UITouch touch, UIEvent evnt) {
			Debug.Assert(_state == MenuState.TrackingTouch, "Menu.TouchEnded, invalid state");
			
			if (_selectedItem != null) {
				_selectedItem.OnUnselected();
				_selectedItem.Activate();
			}
			
			_state = MenuState.Waiting;
		}

		public void TouchCancelled(UITouch touch, UIEvent evnt) {
			Debug.Assert(_state == MenuState.TrackingTouch, "Menu.TouchCancelled, invalid state");
			
			if (_selectedItem != null) {
				_selectedItem.OnUnselected();
			}
			
			_state = MenuState.Waiting;
		}
		
		#endregion Touch Events
	}
}
