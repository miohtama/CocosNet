// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Collections.Generic;

namespace CocosNet.Support {
	public abstract class TouchHandler {	
		public int Priority { get; set; }
		
		public TouchHandler(int priority) {
			Priority = priority;
		}

		public abstract bool TouchesBegan (List<UITouch> touches, UIEvent evnt);
		public abstract bool TouchesMoved (List<UITouch> touches, UIEvent evnt);
		public abstract bool TouchesEnded (List<UITouch> touches, UIEvent evnt);
		public abstract bool TouchesCancelled (List<UITouch> touches, UIEvent evnt);
		
		public abstract object Delegate { get; }
	}
	
	public class TargetedTouchHandler : TouchHandler {
		private ITargetedTouchDelegate _delegate;
		private List<UITouch> _claimedTouches;
		
		private delegate void TouchDelegateMethod(UITouch touch, UIEvent evnt);
		
		private void UpdateKnownTouches(List<UITouch> touches, UIEvent evnt, TouchDelegateMethod method, bool unclaim) {
			List<UITouch> touchesCopy = new List<UITouch>(touches);
			
			foreach (UITouch touch in touchesCopy) {
				if (_claimedTouches.Contains(touch)) {
					method(touch, evnt);
				}
				
				if (unclaim) {
					_claimedTouches.Remove(touch);
				}
				
//				if (SwallowsTouches) {
//					touches.Remove(touch);
//				}
			}
		}
		
		public override object Delegate {
			get {
				return _delegate;
			}
		}

		
		public TargetedTouchHandler(ITargetedTouchDelegate @delegate, int priority, bool swallows)
			: base(priority) {
			if (@delegate == null) {
				throw new ArgumentNullException("@delegate");
			}
			
			_delegate = @delegate;
			SwallowsTouches = swallows;
			_claimedTouches = new List<UITouch>();
		}
		
		public bool SwallowsTouches { get; set; }
		
		public IList<UITouch> ClaimedTouches {
			get {
				return _claimedTouches;
			}
		}
		
		
		public override bool TouchesBegan(List<UITouch> touches, UIEvent evnt) {
			List<UITouch> touchesCopy = new List<UITouch>(touches);
			
			foreach (UITouch touch in touchesCopy) {
				bool touchWasClaimed = _delegate.TouchBegan(touch, evnt);
				
				if (touchWasClaimed) {
					_claimedTouches.Add(touch);
					if (SwallowsTouches) {
						touches.Remove(touch);
					}
				}
			}
			
			return false;
		}
		
		public override bool TouchesMoved(List<UITouch> touches, UIEvent evnt) {
			UpdateKnownTouches(touches, evnt, _delegate.TouchMoved, false);
			return false;
		}
		
		public override bool TouchesEnded(List<UITouch> touches, UIEvent evnt) {
			UpdateKnownTouches(touches, evnt, _delegate.TouchEnded, true);
			return false;
		}
		
		public override bool TouchesCancelled(List<UITouch> touches, UIEvent evnt) {
			UpdateKnownTouches(touches, evnt, _delegate.TouchCancelled, true);
			return false;
		}
	}
}
