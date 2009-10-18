// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;

namespace CocosNet {


	public class Scheduler {
		private static Scheduler _instance;
		
		public static Scheduler Instance {
			get {
				if (_instance == null) {
					_instance = new Scheduler();
				}
				return _instance;
			}
		}
		
		private Scheduler() {
		}
		
		public event EventHandler<TickEventArgs> Tick;
		
		public float TimeScale { get; set; }
		
		public void OnTick(float dt) {
			if (Tick != null) {
				Tick(this, new TickEventArgs(dt));	
			}
		}
	}
	
	public class TickEventArgs : EventArgs {
		public float Delta { get; private set; }
		
		public TickEventArgs(float dt) {
			Delta = dt;
		}
	}
}
