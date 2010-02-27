// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using System.Collections.Generic;

namespace CocosNet {
	public class Scheduler {
		#region singleton support
		private static Scheduler _instance = new Scheduler();

		public static Scheduler Instance {
			get { return _instance; }
		}
		#endregion singleton support

		#region private Timer class
		public class Timer {
			public const int LastPriority = int.MaxValue;
			
			private float _elapsed = -1;

			public event EventHandler<TickEventArgs> Tick;
			public int Priority { get; set; }
			public float Interval { get; set; }

			public Timer() : this(0) {
			}
			
			public Timer(int priority) : this(priority, 0) {
			}
			
			public Timer(int priority, float interval) {
				Priority = priority;
				Interval = interval;
			}
			
			internal void OnTick(TickEventArgs e) {
				if (_elapsed == -1) {
					_elapsed = 0;
				}
				
				_elapsed += e.Delta;
				
				
				if (_elapsed >= Interval) {
					_elapsed = 0;
					if (Tick != null) {
						Tick(this, e);
					}
				}
			}
		}
		#endregion private Timer class

		private List<Scheduler.Timer> _timersToAdd;
		private List<Scheduler.Timer> _timersToRemove;
		private List<Scheduler.Timer> _timers;
		private float _elapsed;
		private int _tickNumber;

		private void RemovePendingTimers() {
			foreach (Timer timer in _timersToRemove) {
				_timers.Remove(timer);
			}
			
			_timersToRemove.Clear();
		}

		private void AddPendingTimers() {
			foreach (Timer timer in _timersToAdd) {
				int pri = timer.Priority;
				
				int i = 0;
				for (i = 0; i < _timers.Count; ++i) {
					if (_timers[i].Priority > pri) {
						break;
					}
				}
				
				_timers.Insert(i, timer);
			}
			
			_timersToAdd.Clear();
		}

		private Scheduler() {
			TimeScale = 1f;
			_timersToAdd = new List<Scheduler.Timer>();
			_timersToRemove = new List<Scheduler.Timer>();
			_timers = new List<Scheduler.Timer>();
		}

		/// <summary>
		/// Modifies the time of all scheduled callbacks.
		/// Use a value less than 1.0 to cause a "slow motion" effect,
		/// and a value greater than 1.0 to cause a "fast forward" effect.
		/// Effects everything that is scheduled.
		/// </summary>
		public float TimeScale { get; set; }

		/// <summary>
		/// Called by the Director to advance scheduling forward by one frame.
		/// Generally speaking, never call this method unless you have a specific reason.
		/// </summary>
		/// <param name="dt">
		/// A <see cref="System.Single"/> indicating how much time has passed since the last frame
		/// </param>
		public void OnTick(float dt) {
			dt *= TimeScale;
			
			_elapsed += dt;
			
			RemovePendingTimers();
			AddPendingTimers();
			
			TickEventArgs te = new TickEventArgs(dt, _elapsed, _tickNumber++);
			
			foreach (Timer timer in _timers) {
				timer.OnTick(te);
			}
		}

		public void Schedule(Timer timer) {
			if (timer == null) {
				throw new ArgumentNullException("timer");
			}
			
			if (_timersToAdd.Contains(timer) || _timers.Contains(timer)) {
				throw new ArgumentException("Timer has already been added", "timer");
			}
			
			if (_timersToRemove.Contains(timer)) {
				_timersToRemove.Remove(timer);
			} else {
				_timersToAdd.Add(timer);
			}
		}

		public void Unschedule(Timer timer) {
			if (timer == null) {
				throw new ArgumentNullException("timer");
			}
			
			if (_timersToRemove.Contains(timer) || !_timers.Contains(timer)) {
				throw new ArgumentException("Timer has already been removed (or was never added in the first place)", "timer");
			}
			
			if (_timersToAdd.Contains(timer)) {
				_timersToAdd.Remove(timer);
			} else {
				_timersToRemove.Add(timer);
			}
		}

		public void UnscheduleAll() {
			_timersToAdd.Clear();
			_timersToRemove.Clear();
			_timers.Clear();
		}
	}

	public class TickEventArgs : EventArgs {
		public float Delta { get; private set; }
		public float TotalTime { get; private set; }
		public int TickNumber { get; private set; }

		public TickEventArgs(float dt, float total, int tickNumber) {
			Delta = dt;
			TotalTime = total;
			TickNumber = tickNumber;
		}
	}
}
