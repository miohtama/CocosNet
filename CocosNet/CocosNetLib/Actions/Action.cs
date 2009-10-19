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
			Target.SetPosition(pos.X, pos.Y);
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

	public class RotateTo : IntervalAction {
		private float _originalAngle;
		private float _startAngle;
		private float _angle;

		public RotateTo(float duration, float angle) : base(duration) {
			_originalAngle = _angle = angle;
		}

		public override object Clone() {
			return new RotateTo(Duration, _originalAngle);
		}

		public override Action Reverse() {
			throw new NotImplementedException("RotateTo can't be reversed");
		}

		public override void Start() {
			base.Start();
			
			// necessary?
			//			if (_startAngle > 0) {
			//				_startAngle = _startAngle % 360.0f;
			//			} else {
			//				_startAngle = _startAngle % -360.0f;
			//			}
			
			_startAngle = Target.Rotation;
			_angle -= _startAngle;
			if (_angle > 180f) {
				_angle = -360f + _angle;
			}
			if (_angle < -180f) {
				_angle += 360f;
			}
		}

		public override void Update(float t) {
			Target.Rotation = _startAngle + (_angle * t);
		}
	}

	public class RotateBy : IntervalAction {
		private float _originalAngle;
		private float _angle;
		private float _startAngle;

		public RotateBy(float duration, float angle) : base(duration) {
			_angle = angle;
		}

		public override object Clone() {
			return new RotateBy(Duration, _originalAngle);
		}

		public override Action Reverse() {
			return new RotateBy(Duration, -_originalAngle);
		}

		public override void Start() {
			base.Start();
			_startAngle = Target.Rotation;
		}

		public override void Update(float t) {
			Target.Rotation = _startAngle + (_angle * t);
		}
	}

	public class ScaleTo : IntervalAction {
		protected float _startScaleX;
		protected float _startScaleY;
		protected float _endScaleX;
		protected float _endScaleY;
		protected float _deltaX;
		protected float _deltaY;

		public ScaleTo(float duration, float scale) : base(duration) {
			_endScaleX = _endScaleY = scale;
		}

		public ScaleTo(float duration, float scaleX, float scaleY) : base(duration) {
			_endScaleX = scaleX;
			_endScaleY = scaleY;
		}

		public override object Clone() {
			return new ScaleTo(Duration, _endScaleX, _endScaleY);
		}

		public override void Start() {
			base.Start();
			
			_startScaleX = Target.ScaleX;
			_startScaleY = Target.ScaleY;
			
			_deltaX = _endScaleX - _startScaleX;
			_deltaY = _endScaleY - _startScaleY;
		}

		public override void Update(float t) {
			Target.ScaleX = _startScaleX + (_deltaX * t);
			Target.ScaleY = _startScaleY + (_deltaY * t);
		}

		public override Action Reverse() {
			throw new NotImplementedException("Cannot reverse a ScaleTo");
		}
		
	}

	public class ScaleBy : ScaleTo {
		public ScaleBy(float duration, float scale) : this(duration, scale, scale) {
		}

		public ScaleBy(float duration, float scaleX, float scaleY) : base(duration, scaleX, scaleY) {
		}

		public override void Start() {
			base.Start();
			
			_deltaX = _startScaleX * _endScaleX - _startScaleX;
			_deltaY = _startScaleY * _endScaleY - _startScaleY;
		}

		public override Action Reverse() {
			return new ScaleBy(Duration, 1 / _endScaleX, 1 / _endScaleY);
		}
		
	}

	public class JumpBy : IntervalAction {
		protected PointF _startPosition;
		protected PointF _delta;
		private float _height;
		private int _jumps;

		public JumpBy(float duration, PointF position, float height, int jumps) : base(duration) {
			_delta = position;
			_height = height;
			_jumps = jumps;
		}

		public override object Clone() {
			return new JumpBy(Duration, _delta, _height, _jumps);
		}

		public override void Start() {
			base.Start();
			_startPosition = Target.Position;
		}

		public override void Update(float t) {
			float y = _height * (float)Math.Abs(Math.Sin(t * (float)Math.PI * _jumps));
			y += _delta.Y * t;
			
			float x = _delta.X * t;
			Target.SetPosition(_startPosition.X + x, _startPosition.Y + y);
		}

		public override Action Reverse() {
			return new JumpBy(Duration, new PointF(-_delta.X, -_delta.Y), _height, _jumps);
		}
	}

	public class JumpTo : JumpBy {
		public JumpTo(float duration, PointF position, float height, int jumps) : base(duration, position, height, jumps) {
		}

		public override void Start() {
			base.Start();
			_delta = new PointF(_delta.X - _startPosition.X, _delta.Y - _startPosition.Y);
		}
		
	}

	public class BezierBy : IntervalAction {
		private static float BezierAt(float a, float b, float c, float d, float t) {
			return (float)(Math.Pow(1 - t, 3) * a + 3 * t * (Math.Pow(1 - t, 2)) * b + 3 * Math.Pow(t, 2) * (1 - t) * c + Math.Pow(t, 3) * d);
		}

		private BezierConfig _config;
		private PointF _startPosition;

		public BezierBy(float duration, BezierConfig config) : base(duration) {
			_config = config;
		}

		public override object Clone() {
			return new BezierBy(Duration, _config);
		}

		public override void Start() {
			base.Start();
			_startPosition = Target.Position;
		}

		public override void Update(float t) {
			float xa = _config.StartPosition.X;
			float xb = _config.ControlPoint1.X;
			float xc = _config.ControlPoint2.X;
			float xd = _config.EndPosition.X;
			
			float ya = _config.StartPosition.Y;
			float yb = _config.ControlPoint1.Y;
			float yc = _config.ControlPoint2.Y;
			float yd = _config.EndPosition.Y;
			
			float x = BezierAt(xa, xb, xc, xd, t);
			float y = BezierAt(ya, yb, yc, yd, t);
			
			Target.SetPosition(_startPosition.X + x, _startPosition.Y + y);
		}

		public override Action Reverse() {
			return new BezierBy(Duration, _config.Negate());
		}
	}
	
	public class Blink : IntervalAction {
		private uint _times;
		
		public Blink(float duration, uint times) : base(duration) {
			_times = times;
		}
		
		public override object Clone() {
			return new Blink(Duration, _times);
		}

		public override void Update (float t) {
			float slice = 1.0f / _times;
			float m = t % slice;
			Target.Visible = m > (slice / 2.0f);
		}

		public override Action Reverse() {
			return Clone() as Blink;
		}
	}
	
	public class FadeIn : IntervalAction {
		public FadeIn(float duration) : base(duration) {
		}
		
		public override void Update(float t) {
			Target.Opacity = (byte)(255 * t);
		}
		
		public override Action Reverse() {
			return new FadeOut(Duration);
		}
		
		public override object Clone() {
			return new FadeIn(Duration);
		}
	}

	public class FadeOut : IntervalAction {
		public FadeOut(float duration) : base(duration) {
		}
		
		public override void Update(float t) {
			Target.Opacity = (byte)(255 * (1.0f - t));
		}
		
		public override Action Reverse() {
			return new FadeIn(Duration);
		}
		
		public override object Clone() {
			return new FadeOut(Duration);
		}
	}
}
