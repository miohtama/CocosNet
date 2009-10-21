// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using System.Collections.Generic;
using CocosNet.Base;

namespace CocosNet.Actions {
	class ActionManager {
		private class HashElement {
			public List<Action> Actions = new List<Action>();
			public CocosNode Target;
		}
		
		private Dictionary<CocosNode, HashElement> _hash;
		
		private static ActionManager _instance = new ActionManager();

		public static ActionManager Instance {
			get {
				return _instance;
			}
		}

		private ActionManager() {
			Scheduler.Instance.Tick += Tick;
			_hash = new Dictionary<CocosNode, HashElement>();
		}
		
		private void Tick(object sender, TickEventArgs e) {
			foreach (HashElement element in _hash.Values) {
				if (element.Target.IsRunning) {
					for (int i = 0; i < element.Actions.Count; ++i) {
						Action action = element.Actions[i];
						action.Step(e.Delta);
						
						if (action.IsDone) {
							element.Actions.Remove(action);
							--i;
						}
					}
				}
			}
		}
		
		public void AddAction(Action action, CocosNode target) {
			if (action == null) {
				throw new ArgumentNullException("action");
			}
			if (target == null) {
				throw new ArgumentNullException("target");
			}
			
			HashElement element = null;
			if (_hash.ContainsKey(target)) {
				element = _hash[target];
			} else {
				element = new HashElement();
				element.Target = target;
				_hash.Add(target, element);
			}
			
			element.Actions.Add(action);
			action.Target = target;
			action.Start();
		}
		
		public void RemoveAction(Action action) {
			if (action != null && _hash.ContainsKey(action.Target)) {
				HashElement element = _hash[action.Target];
				
				element.Actions.Remove(action);
				
				if (element.Actions.IsEmpty()) {
					_hash.Remove(action.Target);
				}
			}
		}
		
		public void RemoveAllActionsForTarget(CocosNode target) {
			if (target != null && _hash.ContainsKey(target)) {
				_hash.Remove(target);
			}
		}
	}
}
