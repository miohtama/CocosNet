// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace CocosNet.Support {
	public class TouchDispatcher :  IEAGLTouchDelegate {
		private static TouchDispatcher _instance = new TouchDispatcher();
		
		public static TouchDispatcher Instance {
			get {
				return _instance;
			}
		}
		
		private List<TouchHandler> _touchHandlers;
		
		private void AddHandler(TouchHandler handler) {
			int i = 0;
			foreach (TouchHandler h in _touchHandlers) {
				if (h.Priority < handler.Priority) {
					++i;
				}
				
				if (object.Equals(h.Delegate, handler.Delegate)) {
					return;
				}
			}
			_touchHandlers.Insert(i, handler);
		}
		
		private TouchDispatcher() {
			_touchHandlers = new List<TouchHandler>();
			DispatchEvents = true;
		}
	
		public bool DispatchEvents { get; set; }
		
		public void AddTargetedDelegate(ITargetedTouchDelegate del, int priority, bool swallowsTouches) {
			TouchHandler handler = new TargetedTouchHandler(del, priority, swallowsTouches);
			AddHandler(handler);
		}

		public void RemoveDelegate(object del) {
			if (del == null) {
				throw new ArgumentNullException("del");
			}
			
			foreach (TouchHandler handler in _touchHandlers) {
				if (object.Equals(handler.Delegate, del)) {
					_touchHandlers.Remove(handler);
					break;
				}
			}
		}
		
		public void RemoveAllDelegates() {
			_touchHandlers.Clear();
		}
		
		public void TouchesBegan(NSSet touchSet, UIEvent evnt) {
			if (DispatchEvents) {
				List<TouchHandler> handlers = new List<TouchHandler>(_touchHandlers);
				
				// Make full-aot aware of the needed ICollection<UITouch> types
				ICollection<UITouch> touches_col = (ICollection<UITouch>)touchSet.ToArray<UITouch>();
				
				#pragma warning disable 0219
				// this is a tiny hack, make sure the AOT compiler knows about
				// UICollection.Count, as it will need it when we instantiate the list below
				int touches_count = touches_col.Count;
				#pragma warning restore 0219
				
				List<UITouch> touches = new List<UITouch>(touches_col);
				
				foreach (TouchHandler handler in handlers) {
					if (handler.TouchesBegan(touches, evnt)) {
						break;
					}
					if (touches.IsEmpty()) {
						break;
					}
				}
			}
		}
		
		public void TouchesMoved(NSSet touchSet, UIEvent evnt) {
			if (DispatchEvents) {
				List<TouchHandler> handlers = new List<TouchHandler>(_touchHandlers);
				// Make full-aot aware of the needed ICollection<UITouch> types
				ICollection<UITouch> touches_col = (ICollection<UITouch>)touchSet.ToArray<UITouch>();

			
				#pragma warning disable 0219
				// this is a tiny hack, make sure the AOT compiler knows about
				// UICollection.Count, as it will need it when we instantiate the list below
				int touches_count = touches_col.Count;
				#pragma warning restore 0219
				
				List<UITouch> touches = new List<UITouch>(touches_col);
				
				foreach (TouchHandler handler in handlers) {
					if (handler.TouchesMoved(touches, evnt)) {
						break;
					}
					if (touches.IsEmpty()) {
						break;
					}
				}
			}
		}
		
		public void TouchesEnded(NSSet touchSet, UIEvent evnt) {
			if (DispatchEvents) {
				List<TouchHandler> handlers = new List<TouchHandler>(_touchHandlers);
				// Make full-aot aware of the needed ICollection<UITouch> types
				ICollection<UITouch> touches_col = (ICollection<UITouch>)touchSet.ToArray<UITouch>();
				
				#pragma warning disable 0219
				// this is a tiny hack, make sure the AOT compiler knows about
				// UICollection.Count, as it will need it when we instantiate the list below
				int touches_count = touches_col.Count;
				#pragma warning restore 0219
				
				List<UITouch> touches = new List<UITouch>(touches_col);
				
				foreach (TouchHandler handler in handlers) {
					if (handler.TouchesEnded(touches, evnt)) {
						break;
					}
					if (touches.IsEmpty()) {
						break;
					}
				}
			}
		}
		
		public void TouchesCancelled(NSSet touchSet, UIEvent evnt) {
			if (DispatchEvents) {
				List<TouchHandler> handlers = new List<TouchHandler>(_touchHandlers);
				// Make full-aot aware of the needed ICollection<UITouch> types
				ICollection<UITouch> touches_col = (ICollection<UITouch>)touchSet.ToArray<UITouch>();
				
				#pragma warning disable 0219
				// this is a tiny hack, make sure the AOT compiler knows about
				// UICollection.Count, as it will need it when we instantiate the list below
				int touches_count = touches_col.Count;
				#pragma warning restore 0219
				
				List<UITouch> touches = new List<UITouch>(touches_col);
				
				foreach (TouchHandler handler in handlers) {
					if (handler.TouchesCancelled(touches, evnt)) {
						break;
					}
					if (touches.IsEmpty()) {
						break;
					}
				}
			}
		}
	}
}
