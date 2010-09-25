using System;
using System.Diagnostics;

namespace CocosNet.Layers {
	public class TimedScene : Scene {
		private float _timespan;
		private Scheduler.Timer _timer;
		
		public event EventHandler TimeElapsed;
		
		private void OnTick(object sender, TickEventArgs e) {
			_timespan -= e.Delta;
			
			if (_timespan < 0) {
				Debug.Assert(TimeElapsed != null);
				TimeElapsed(this, EventArgs.Empty);
			}
		}
		
		public TimedScene(float timespan) {
			_timer = new Scheduler.Timer(Scheduler.Timer.LastPriority);
			_timer.Tick += OnTick;
			
			_timespan = timespan;
		}
		
		public override void OnEnter() {
			base.OnEnter();
			
			Scheduler.Instance.Schedule(_timer);
		}
		
		public override void OnExit() {
			Scheduler.Instance.Unschedule(_timer);
		}
	}
}

