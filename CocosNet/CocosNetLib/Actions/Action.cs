// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using CocosNet.Base;

namespace CocosNet.Actions {
	public abstract class Action : ICloneable {
		public const int ActionTagInvalid = -1;

		public CocosNode Target { get; set; }
		public int Tag { get; set; }

		public Action() {
			Target = null;
			Tag = ActionTagInvalid;
		}

		public override string ToString() {
			return string.Format("{0}, Tag: {1}", base.ToString(), Tag);
		}

		public abstract object Clone();

		public abstract void Start();

		public virtual void Stop() {
		}

		public virtual bool IsDone {
			get { return true; }
		}

		public virtual void Step(float t) {
		}

		public virtual void Update(float t) {
		}

		public abstract Action Reverse();
	}

	public class RepeatForever : Action {
		private IntervalAction _repeatedAction;

		public RepeatForever(IntervalAction repeatedAction) {
			if (repeatedAction == null) {
				throw new ArgumentNullException("repeatedAction");
			}
			
			_repeatedAction = repeatedAction;
		}

		public override object Clone() {
			return new RepeatForever(_repeatedAction);
		}

		public override void Start() {
			_repeatedAction.Target = Target;
			_repeatedAction.Start();
		}

		public override void Step(float t) {
			_repeatedAction.Step(t);
			if (_repeatedAction.IsDone) {
				_repeatedAction.Start();
			}
		}

		public override bool IsDone {
			get { return false; }
		}

		public override Action Reverse() {
			return new RepeatForever(_repeatedAction.Reverse() as IntervalAction);
		}
		
	}

	public abstract class FiniteTimeAction : Action {
		public float Duration { get; set; }
	}

	public abstract class IntervalAction : FiniteTimeAction {
		private float _elapsed;
		private bool _firstTick;

		public IntervalAction(float duration) {
			if (duration == 0) {
				duration = 1E-08f;
			}
			
			Duration = duration;
		}

		public override bool IsDone {
			get { return _elapsed >= Duration; }
		}


		public float Elapsed {
			get { return _elapsed; }
		}

		public override void Step(float dt) {
			if (_firstTick) {
				_firstTick = false;
				_elapsed = 0;
			} else {
				_elapsed += dt;
			}
			
			Update(Math.Min(1, Elapsed / Duration));
		}

		public override void Start() {
			_elapsed = 0;
			_firstTick = true;
		}
	}

	public class Sequence : IntervalAction {
		public static Sequence Construct(params FiniteTimeAction[] actions) {
			if (actions == null) {
				throw new ArgumentNullException("actions");
			}
			
			if (actions.Length < 2) {
				throw new ArgumentException("Must construct sequence with at least 2 actions", "actions");
			}
			
			FiniteTimeAction prev = actions[0];
			
			for (int i = 1; i < actions.Length; ++i) {
				FiniteTimeAction now = actions[i];
				prev = new Sequence(prev, now);
			}
			
			return prev as Sequence;
		}

		private FiniteTimeAction[] _actions;
		private float _split;
		private int _last;

		public Sequence(FiniteTimeAction actionOne, FiniteTimeAction actionTwo) : base(0) {
			if (actionOne == null) {
				throw new ArgumentNullException("actionOne");
			}
			
			if (actionTwo == null) {
				throw new ArgumentNullException("actionTwo");
			}
			
			_actions = new FiniteTimeAction[] {
				actionOne,
				actionTwo
			};
			
			Duration = actionOne.Duration + actionTwo.Duration;
		}

		public override void Start() {
			base.Start();
			foreach (Action action in _actions) {
				action.Target = Target;
			}
			
			_split = _actions[0].Duration / Duration;
			_last = -1;
		}

		public override void Update(float t) {
			int found = 0;
			float newTime = 0;
			
			if (t >= _split) {
				found = 1;
				if (_split == 1) {
					newTime = 1;
				} else {
					newTime = (t - _split) / (1 - _split);
				}
			} else {
				found = 0;
				if (_split != 0) {
					newTime = t / _split;
				} else {
					newTime = 1;
				}
			}
			
			if (_last == -1 && found == 1) {
				_actions[0].Start();
				_actions[0].Update(1f);
				_actions[0].Stop();
			}
			
			if (_last != found) {
				if (_last != -1) {
					_actions[_last].Update(1f);
					_actions[_last].Stop();
				}
				_actions[found].Start();
			}
			
			_actions[found].Update(newTime);
			_last = found;
		}

		public override Action Reverse() {
			return new Sequence(_actions[1].Reverse() as FiniteTimeAction, _actions[0].Reverse() as FiniteTimeAction);
		}

		public override object Clone() {
			Sequence newSequence = new Sequence(_actions[0], _actions[1]);
			newSequence._split = _split;
			newSequence._last = _last;
			
			return newSequence;
		}
		
	}

	public class MoveTo : IntervalAction {
		protected PointF _endPosition;
		protected PointF _startPosition;
		protected PointF _delta;

		public MoveTo(float duration, PointF position) : base(duration) {
			_endPosition = position;
		}

		public override void Start() {
			base.Start();
			
			_startPosition = Target.Position;
			_delta = new PointF(_endPosition.X - _startPosition.X, _endPosition.Y - _startPosition.Y);
		}

		public override void Update(float t) {
			PointF pos = Target.Position;
			pos.X = _startPosition.X + (_delta.X * t);
			pos.Y = _startPosition.Y + (_delta.Y * t);
			Target.Position = pos;
		}

		public override object Clone() {
			MoveTo newMoveTo = new MoveBy(Duration, _endPosition);
			newMoveTo._delta = _delta;
			
			return newMoveTo;
		}
		
		public override Action Reverse() {
			// artistic license not found in Cocos2D iPhone
			
			MoveTo reversed = new MoveTo(Duration, _startPosition);
			reversed._endPosition = _startPosition;
			
			return reversed;
		}

	}

	public class MoveBy : MoveTo {
		public MoveBy(float duration, PointF moveByAmount) : base(duration, PointF.Empty) {
			_delta = moveByAmount;
		}

		public override void Start() {
			PointF tmp = _delta;
			base.Start();
			_delta = tmp;
		}

		public override object Clone() {
			MoveBy newMoveBy = new MoveBy(Duration, _delta);
			
			return newMoveBy;
		}
		
		public override Action Reverse() {
			return new MoveBy(Duration, new PointF(-_delta.X, -_delta.Y));
		}

	}
}
