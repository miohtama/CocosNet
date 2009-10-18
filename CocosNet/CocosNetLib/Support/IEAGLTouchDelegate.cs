// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace CocosNet.Support {
	public interface IEAGLTouchDelegate {
		void TouchesBegan(NSSet touches, UIEvent evnt);
		void TouchesEnded(NSSet touches, UIEvent evnt);
		void TouchesMoved(NSSet touches, UIEvent evnt);
		void TouchesCancelled(NSSet touches, UIEvent evnt);
	}
}
