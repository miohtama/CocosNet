// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using MonoTouch.UIKit;

namespace CocosNet.Support {
	public interface ITargetedTouchDelegate {
		bool TouchBegan(UITouch touch, UIEvent evnt);
		void TouchMoved(UITouch touch, UIEvent evnt);
		void TouchEnded(UITouch touch, UIEvent evnt);
		void TouchCancelled(UITouch touch, UIEvent evnt);
	}
}
