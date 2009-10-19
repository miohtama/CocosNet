// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using CocosNet.Base;
using CocosNet.Sprites;
using CocosNet.Layers;
using CocosNet.Menus;
using CocosNet.Labels;
using System.Drawing;
using CocosNet;
using CocosNet.Actions;
using Color = CocosNet.Base.Color;
using Action = CocosNet.Actions.Action;

namespace Cocos2dPortedTests {
	public abstract class SpriteDemo : Layer, ICloneable {
		private static int _sceneIndex = 0;

		private static readonly SpriteDemo[] Transitions = {
			new SpriteManual(),
			new SpriteMove(),
			new SpriteRotate(),
			new SpriteScale(),
			new SpriteJump(),
			new SpriteBezier(),
			new SpriteBlink(),
			new SpriteFade(),
			new SpriteTint(),
//			new SpriteAnimate(),
			new SpriteSequence(),
			new SpriteSpawn(),
			new SpriteReverse(),
			new SpriteDelayTime(),
			new SpriteRepeat(),
//			new SpriteCallFunc(),
			new SpriteReverseSequence(),
			new SpriteReverseSequence2(),
			new SpriteOrbit()
		};

		private static CocosNode NextAction() {
			++_sceneIndex;
			_sceneIndex = _sceneIndex % Transitions.Length;
			return Transitions[_sceneIndex].Clone() as CocosNode;
		}

		private static CocosNode BackAction() {
			--_sceneIndex;
			if (_sceneIndex < 0) {
				_sceneIndex = Transitions.Length - 1;
			}
			
			return Transitions[_sceneIndex].Clone() as CocosNode;
		}

		private static CocosNode RestartAction() {
			return Transitions[_sceneIndex].Clone() as CocosNode;
		}

		protected Sprite _grossini;
		protected Sprite _tamara;

		protected void CenterSprites() {
			SizeF s = Director.Instance.WinSize;
			
			_grossini.SetPosition(s.Width / 3, s.Height / 2);
			_tamara.SetPosition(2 * s.Width / 3, s.Height / 2);
		}

		public SpriteDemo() {
			_grossini = new Sprite("grossini.png");
			_tamara = new Sprite("grossinis_sister1.png");
			
			AddChild(_grossini);
			AddChild(_tamara);
			
			SizeF s = Director.Instance.WinSize;
			
			_grossini.SetPosition(60, s.Height / 3);
			_tamara.SetPosition(60, 2 * s.Height / 3);
			
			Label label = new Label(ToString(), "Arial", 32);
			AddChild(label);
			
			label.SetPosition(s.Width / 2f, s.Height - 50);
			
			MenuItemImage item1 = new MenuItemImage("b1.png", "b2.png");
			item1.Click += OnBack;
			
			MenuItemImage item2 = new MenuItemImage("r1.png", "r2.png");
			item2.Click += OnRestart;
			
			MenuItemImage item3 = new MenuItemImage("f1.png", "f2.png");
			item3.Click += OnForward;
			
			Menu menu = new Menu(item1, item2, item3);
			
			menu.SetPosition(PointF.Empty);
			item1.SetPosition(480 / 2 - 100, 30);
			item2.SetPosition(480 / 2, 30);
			item3.SetPosition(480 / 2 + 100, 30);
			
			AddChild(menu, 1);
		}

		public abstract object Clone();


		private void OnBack(object sender, EventArgs e) {
			Director.Instance.ReplaceScene(new Scene(BackAction()));
		}

		private void OnForward(object sender, EventArgs e) {
			Director.Instance.ReplaceScene(new Scene(NextAction()));
		}

		private void OnRestart(object sender, EventArgs e) {
			Director.Instance.ReplaceScene(new Scene(RestartAction()));
		}
	}

	public class SpriteManual : SpriteDemo {
		public override void OnEnter() {
			base.OnEnter();
			
			_tamara.ScaleX = 2.5f;
			_tamara.ScaleY = -1f;
			_tamara.SetPosition(100, 70);
			_tamara.Opacity = 128;
			
			_grossini.Rotation = 120;
			_grossini.Opacity = 128;
			_grossini.SetPosition(240, 160);
			_grossini.Color = Colors.New(255, 0, 0, 255);
		}

		public override object Clone() {
			return new SpriteManual();
		}

		public override string ToString() {
			return "Manual Transformation";
		}
		
	}

	public class SpriteMove : SpriteDemo {
		public override void OnEnter() {
			base.OnEnter();
			
			SizeF s = Director.Instance.WinSize;
			
			var actionTo = new MoveTo(2, new PointF(s.Width - 40, s.Height - 40));
			var actionBy = new MoveBy(2, new PointF(80, 80));
			
			_tamara.RunAction(actionTo);
			_grossini.RunAction(new Sequence(actionBy, actionBy.Reverse() as IntervalAction));
		}

		public override object Clone() {
			return new SpriteMove();
		}

		public override string ToString() {
			return "MoveTo / MoveBy";
		}
		
	}

	public class SpriteRotate : SpriteDemo {
		public override void OnEnter() {
			base.OnEnter();
			
			CenterSprites();
			
			var actionTo = new RotateTo(2, 45);
			var actionTo2 = new RotateTo(2, -45);
			var actionTo0 = new RotateTo(2, 0);
			_tamara.RunAction(new Sequence(actionTo, actionTo0));
			
			var actionBy = new RotateBy(2, 360);
			var actionByBack = actionBy.Reverse() as RotateBy;
			_grossini.RunAction(new Sequence(actionBy, actionByBack));
			
			
			Sprite kathia = new Sprite("grossinis_sister2.png");
			AddChild(kathia);
			kathia.SetPosition(240, 160);
			kathia.RunAction(new Sequence(actionTo2, actionTo0.Clone() as RotateTo));
		}

		public override object Clone() {
			return new SpriteRotate();
		}

		public override string ToString() {
			return "RotateTo / RotateBy";
		}
	}

	public class SpriteScale : SpriteDemo {
		public override void OnEnter() {
			base.OnEnter();
			
			CenterSprites();
			
			ScaleTo actionTo = new ScaleTo(2, 0.5f);
			ScaleBy actionBy = new ScaleBy(2, 2);
			ScaleBy actionByBack = actionBy.Reverse() as ScaleBy;
			
			_tamara.RunAction(actionTo);
			_grossini.RunAction(new Sequence(actionBy, actionByBack));
		}

		public override string ToString() {
			return "ScaleTo / ScaleBy";
		}

		public override object Clone() {
			return new SpriteScale();
		}
	}

	public class SpriteJump : SpriteDemo {
		public override void OnEnter() {
			base.OnEnter();
			
			JumpTo actionTo = new JumpTo(2, new PointF(300, 300), 50, 4);
			JumpBy actionBy = new JumpBy(2, new PointF(300, 0), 50, 4);
			JumpBy actionByBack = actionBy.Reverse() as JumpBy;
			
			_tamara.RunAction(actionTo);
			_grossini.RunAction(new Sequence(actionBy, actionByBack));
		}

		public override string ToString() {
			return "JumpTo / JumpBy";
		}

		public override object Clone() {
			return new SpriteJump();
		}
	}

	public class SpriteBezier : SpriteDemo {

		public override void OnEnter() {
			base.OnEnter();
			
			SizeF s = Director.Instance.WinSize;
			
			//
			// startPosition can be any coordinate, but since the movement
			// is relative to the Bezier curve, make it (0,0)
			//
			
			// sprite 1
			BezierConfig bezier = new BezierConfig();
			bezier.StartPosition = PointF.Empty;
			bezier.ControlPoint1 = new PointF(0, s.Height / 2);
			bezier.ControlPoint2 = new PointF(300, -s.Height / 2);
			bezier.EndPosition = new PointF(300, 100);
			
			BezierBy bezierForward = new BezierBy(3, bezier);
			BezierBy bezierBack = bezierForward.Reverse() as BezierBy;
			Sequence seq = new Sequence(bezierForward, bezierBack);
			RepeatForever rep = new RepeatForever(seq);
			
			
			// sprite 2
			BezierConfig bezier2 = new BezierConfig();
			bezier2.StartPosition = PointF.Empty;
			bezier2.ControlPoint1 = new PointF(100, s.Height / 2);
			bezier2.ControlPoint2 = new PointF(200, -s.Height / 2);
			bezier2.EndPosition = new PointF(300, 0);
			
			BezierBy bezierForward2 = new BezierBy(3, bezier2);
			BezierBy bezierBack2 = bezierForward2.Reverse() as BezierBy;
			Sequence seq2 = new Sequence(bezierForward2, bezierBack2);
			RepeatForever rep2 = new RepeatForever(seq2);
			
			
			_grossini.RunAction(rep);
			_tamara.RunAction(rep2);
		}

		public override object Clone() {
			return new SpriteBezier();
		}

		public override string ToString() {
			return "BezierBy";
		}
	}

	public class SpriteBlink : SpriteDemo {
		public override void OnEnter() {
			base.OnEnter();
			
			CenterSprites();
			
			Blink action1 = new Blink(2, 10);
			Blink action2 = new Blink(2, 5);
			
			_tamara.RunAction(action1);
			_grossini.RunAction(action2);
		}

		public override string ToString() {
			return "Blink";
		}

		public override object Clone() {
			return new SpriteBlink();
		}
	}

	public class SpriteFade : SpriteDemo {
		public override void OnEnter() {
			base.OnEnter();
			
			CenterSprites();
			
			_tamara.Opacity = 0;
			FadeIn action1 = new FadeIn(1);
			FadeOut action1Back = action1.Reverse() as FadeOut;
			
			FadeOut action2 = new FadeOut(1);
			FadeIn action2Back = action2.Reverse() as FadeIn;
			
			_tamara.RunAction(new Sequence(action1, action1Back));
			_grossini.RunAction(new Sequence(action2, action2Back));
		}

		public override object Clone() {
			return new SpriteFade();
		}

		public override string ToString() {
			return "FadeIn / FadeOut";
		}
	}

	public class SpriteTint : SpriteDemo {
		public override void OnEnter() {
			base.OnEnter();
			
			CenterSprites();
			
			TintTo action1 = new TintTo(2, 255, 0, 255);
			TintBy action2 = new TintBy(2, -127, -255, -127);
			TintBy action2Back = action2.Reverse() as TintBy;
			
			_tamara.RunAction(action1);
			_grossini.RunAction(new Sequence(action2, action2Back));
		}

		public override object Clone() {
			return new SpriteTint();
		}

		public override string ToString() {
			return "TintTo / TintBy";
		}
	}

	public class SpriteSequence : SpriteDemo {
		public override void OnEnter() {
			base.OnEnter();
			_tamara.Visible = false;
			
			Sequence action = Sequence.Construct(new MoveBy(2, new PointF(240, 0)), new RotateBy(2, 540));
			
			_grossini.RunAction(action);
		}

		public override object Clone() {
			return new SpriteSequence();
		}

		public override string ToString() {
			return "Sequence: Move + Rotate";
		}
	}

	public class SpriteSpawn : SpriteDemo {
		public override void OnEnter() {
			base.OnEnter();
			_tamara.Visible = false;
			
			Spawn action = Spawn.Construct(new JumpBy(2, new PointF(300, 0), 50, 4), new RotateBy(2, 720));
			
			_grossini.RunAction(action);
		}

		public override object Clone() {
			return new SpriteSpawn();
		}

		public override string ToString() {
			return "Spawn: Jump + Rotate";
		}
	}

	public class SpriteReverse : SpriteDemo {
		public override void OnEnter() {
			base.OnEnter();
			_tamara.Visible = false;
			
			JumpBy jump = new JumpBy(2, new PointF(300, 0), 50, 4);
			Sequence action = Sequence.Construct(jump, jump.Reverse() as FiniteTimeAction);
			
			_grossini.RunAction(action);
		}

		public override object Clone() {
			return new SpriteReverse();
		}

		public override string ToString() {
			return "Reverse an action";
		}
		
	}

	public class SpriteDelayTime : SpriteDemo {
		public override void OnEnter() {
			base.OnEnter();
			
			_tamara.Visible = false;
			MoveBy move = new MoveBy(1, new PointF(150, 0));
			Sequence action = Sequence.Construct(move, new DelayTime(2), move);
			
			_grossini.RunAction(action);
		}

		public override object Clone() {
			return new SpriteDelayTime();
		}

		public override string ToString() {
			return "DelayTime: m + delay + m";
		}
		
	}

	public class SpriteRepeat : SpriteDemo {
		public override void OnEnter() {
			base.OnEnter();
			
			MoveBy a1 = new MoveBy(1, new PointF(150, 0));
			Repeat action1 = new Repeat(new Sequence(new Place(new PointF(60, 60)), a1), 3);
			RepeatForever action2 = new RepeatForever(new Sequence(a1.Clone() as FiniteTimeAction, a1.Reverse() as FiniteTimeAction));
			
			_grossini.RunAction(action1);
			_tamara.RunAction(action2);
		}

		public override object Clone() {
			return new SpriteRepeat();
		}

		public override string ToString() {
			return "Repeat / RepeatForever actions";
		}
	}

	public class SpriteReverseSequence : SpriteDemo {
		public override void OnEnter() {
			base.OnEnter();
			_tamara.Visible = false;
			
			MoveBy move1 = new MoveBy(1, new PointF(250, 0));
			MoveBy move2 = new MoveBy(1, new PointF(0, 50));
			Sequence seq = Sequence.Construct(move1, move2, move1.Reverse() as FiniteTimeAction);
			Sequence action = Sequence.Construct(seq, seq.Reverse() as FiniteTimeAction);
			
			_grossini.RunAction(action);
		}

		public override object Clone() {
			return new SpriteReverseSequence();
		}

		public override string ToString() {
			return "Reverse a sequence";
		}
		
	}

	public class SpriteReverseSequence2 : SpriteDemo {
		public override void OnEnter() {
			base.OnEnter();
			MoveBy move1 = new MoveBy(1, new PointF(250, 0));
			MoveBy move2 = new MoveBy(1, new PointF(0, 50));
			ToggleVisibility tog1 = new ToggleVisibility();
			ToggleVisibility tog2 = new ToggleVisibility();
			Sequence seq = Sequence.Construct(move1, tog1, move2, tog2, move1.Reverse() as FiniteTimeAction);
			Repeat action = new Repeat(Sequence.Construct(seq, seq.Reverse() as FiniteTimeAction), 3);
			
			_grossini.RunAction(action);
			
			MoveBy move_tamara = new MoveBy(1, new PointF(100, 0));
			MoveBy move_tamara2 = new MoveBy(1, new PointF(50, 0));
			Hide hide = new Hide();
			Sequence seq_tamara = Sequence.Construct(move_tamara, hide, move_tamara2);
			Sequence seq_back = seq_tamara.Reverse() as Sequence;
			_tamara.RunAction(Sequence.Construct(seq_tamara, seq_back));
		}

		public override object Clone() {
			return new SpriteReverseSequence2();
		}

		public override string ToString() {
			return "Reverse sequence 2";
		}
	}

	public class SpriteOrbit : SpriteDemo {
		public override void OnEnter() {
			base.OnEnter();
			
			CenterSprites();
			
			OrbitCamera orbit1 = new OrbitCamera(2, 1, 0, 0, 180, 0, 0);
			Sequence action1 = Sequence.Construct(orbit1, orbit1.Reverse() as FiniteTimeAction);
			
			OrbitCamera orbit2 = new OrbitCamera(2, 1, 0, 0, 180, -45, 0);
			Sequence action2 = Sequence.Construct(orbit2, orbit2.Reverse() as FiniteTimeAction);
			
			_grossini.RunAction(orbit1);
			_tamara.RunAction(orbit2);
		}

		public override object Clone() {
			return new SpriteOrbit();
		}

		public override string ToString() {
			return "OrbitCamera action";
		}
	}
}
