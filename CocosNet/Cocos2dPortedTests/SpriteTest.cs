using System;
using CocosNet.Base;
using CocosNet.Sprites;
using CocosNet.Layers;
using CocosNet.Menus;
using CocosNet.Labels;
using System.Drawing;
using CocosNet;
using Color=CocosNet.Base.Color;
using CocosNet.Actions;

namespace Cocos2dPortedTests {
	public abstract class SpriteDemo : Layer, ICloneable {
		private static int _sceneIndex = 0;

		private static readonly SpriteDemo[] Transitions = {
			new SpriteManual(),
			new SpriteMove()
//			new SpriteRotate(),
//			new SpriteScale(),
//			new SpriteJump(),
//			new SpriteBezier(),
//			new SpriteBlink(),
//			new SpriteFade(),
//			new SpriteTint(),
//			new SpriteAnimate(),
//			new SpriteSequence(),
//			new SpriteSpawn(),
//			new SpriteReverse(),
//			new SpriteDelayTime(),
//			new SpriteRepeat(),
//			new SpriteCallFunc(),
//			new SpriteReverseSequence(),
//			new SpriteReverseSequence2(),
//			new SpriteOrbit()
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

		public SpriteDemo() {
			_grossini = new Sprite("grossini.png");
			_tamara = new Sprite("grossinis_sister1.png");
			
			AddChild(_grossini);
			AddChild(_tamara);
			
			SizeF s = Director.Instance.WinSize;
			
			_grossini.Position = new PointF(60, s.Height / 3);
			_tamara.Position = new PointF(60, 2 * s.Height / 3);
			
			Label label = new Label(ToString(), "Arial", 32);
			AddChild(label);
			
			label.Position = new PointF(s.Width / 2f, s.Height - 50);
			
			MenuItemImage item1 = new MenuItemImage("b1.png", "b2.png");
			item1.Click += OnBack;
			
			MenuItemImage item2 = new MenuItemImage("r1.png", "r2.png");
			item2.Click += OnRestart;
			
			MenuItemImage item3 = new MenuItemImage("f1.png", "f2.png");
			item3.Click += OnForward;
			
			Menu menu = new Menu(item1, item2, item3);
			
			menu.Position = PointF.Empty;
			item1.Position = new PointF(s.Width / 2 - 100, 30);
			item2.Position = new PointF(s.Width / 2, 30);
			item3.Position = new PointF(s.Width / 2 + 100, 30);
			
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
			_tamara.Position = new PointF(100, 70);
			_tamara.Opacity = 128;
			
			_grossini.Rotation = 120;
			_grossini.Opacity = 128;
			_grossini.Position = new Point(240, 160);
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
}
