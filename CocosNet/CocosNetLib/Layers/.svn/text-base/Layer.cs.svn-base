using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosNet.Support;
using CocosNet.Base;

namespace CocosNet.Layers {
	public class Layer : CocosNode, ITargetedTouchDelegate {
		private bool _isTouchEnabled;
		
		public bool IsTouchEnabled {
			get {
				return _isTouchEnabled;
			}
			set {
				_isTouchEnabled = value;
				
				if (_isTouchEnabled) {
					RegisterWithTouchDispatcher();
				} else {
					TouchDispatcher.Instance.RemoveDelegate(this);
				}
			}
		}

		public virtual void RegisterWithTouchDispatcher() {
		}
		#region TargetedTouchDelegate implementation

		public bool TouchBegan(MonoTouch.UIKit.UITouch touch, MonoTouch.UIKit.UIEvent evnt) {
			throw new System.NotImplementedException();
		}

		public void TouchMoved(MonoTouch.UIKit.UITouch touch, MonoTouch.UIKit.UIEvent evnt) {
			throw new System.NotImplementedException();
		}
		public void TouchEnded(MonoTouch.UIKit.UITouch touch, MonoTouch.UIKit.UIEvent evnt) {
			throw new System.NotImplementedException();
		}
		public void TouchCancelled(MonoTouch.UIKit.UITouch touch, MonoTouch.UIKit.UIEvent evnt) {
			throw new System.NotImplementedException();
		}
		#endregion
		
		
		
	}
}
