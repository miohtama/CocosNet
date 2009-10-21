// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using CocosNet;
using CocosNet.Labels;
using CocosNet.Layers;
using CocosNet.Menus;
using CocosNet.Sprites;
using CocosNet.Tiles;
using CocosNet.Actions;
using CocosNet.Support;
using CocosNet.Base;

namespace Cocos2dPortedTests {
	public abstract class ParallaxDemo : TestBase {
		private static readonly ParallaxDemo[] MyScenes = {
			new Parallax1(),
			new Parallax2()
		};

		protected override ICloneable[] Scenes {
			get { return MyScenes; }
		}
		
	}

	public class Parallax1 : ParallaxDemo {
		protected const int ParallaxNodeTag = 5;

		public Parallax1() {
			Sprite cocosImage = new Sprite("powered.png");
			
			cocosImage.Scale = 2.5f;
			cocosImage.AnchorPoint = PointF.Empty;
			
			// TileMapAtlas is deprecated in Cocos2D in favor of TMXTiledMap, 
			// so probably not going to bother implementing TileMapAtlas
			
			// TODO: place a TMXTiledMap here instead
			
			//TileMapAtlas tileMap = new TileMapAtlas("tiles.png", "levelMap.tga", 16, 16);
			//			
			//			tileMap.AnchorPoint = PointF.Empty;
			
			Sprite background = new Sprite("background.png");
			
			background.Scale = 1.5f;
			background.AnchorPoint = PointF.Empty;
			
			ParallaxNode voidNode = new ParallaxNode();
			
			voidNode.AddChild(background, -1, new PointF(0.4f, 0.5f), PointF.Empty);
			//			voidNode.AddChild(tileMap, 1, new PointF(2.2f, 1f), new PointF(0, -200));
			voidNode.AddChild(cocosImage, 2, new PointF(3f, 2.5f), new PointF(200, 800));
			
			MoveBy goUp = new MoveBy(4, new PointF(0, -500));
			MoveBy goDown = goUp.Reverse() as MoveBy;
			MoveBy goForward = new MoveBy(8, new PointF(-1000, 0));
			MoveBy goBack = goForward.Reverse() as MoveBy;
			Sequence sequence = Sequence.Construct(goUp, goForward, goDown, goBack);
			
			voidNode.RunAction(new RepeatForever(sequence));
			
			AddChild(voidNode, 0, ParallaxNodeTag);
		}

		public override string ToString() {
			// 2 children, for now
			return "Parallax: parent and 2 children";
		}

		public override object Clone() {
			return new Parallax1();
		}
	}

	public class Parallax2 : Parallax1 {
		public Parallax2() {
			ParallaxNode voidNode = GetChildByTag(ParallaxNodeTag) as ParallaxNode;
			
			voidNode.StopAllActions();
		}

		public override void OnEnter() {
			IsTouchEnabled = true;
		}

		public override void OnExit() {
			IsTouchEnabled = false;
		}


		public override void RegisterWithTouchDispatcher() {
			TouchDispatcher.Instance.AddTargetedDelegate(this, 0, true);
		}

		public override bool TouchBegan(MonoTouch.UIKit.UITouch touch, MonoTouch.UIKit.UIEvent evnt) {
			return true;
		}

		public override void TouchEnded(MonoTouch.UIKit.UITouch touch, MonoTouch.UIKit.UIEvent evnt) {
			
		}

		public override void TouchCancelled(MonoTouch.UIKit.UITouch touch, MonoTouch.UIKit.UIEvent evnt) {
			
		}

		public override void TouchMoved(MonoTouch.UIKit.UITouch touch, MonoTouch.UIKit.UIEvent evnt) {
			PointF touchLocation = touch.LocationInView(touch.View);
			PointF prevLocation = touch.PreviousLocationInView(touch.View);
			
			touchLocation = Director.Instance.ConvertCoordinate(touchLocation);
			prevLocation = Director.Instance.ConvertCoordinate(prevLocation);
			
			PointF diff = touchLocation.Subtract(prevLocation);
			
			CocosNode node = GetChildByTag(ParallaxNodeTag);
			
			node.SetPosition(node.Position.Add(diff));
		}

		public override string ToString() {
			return "Parallax: drag screen";
		}

		public override object Clone() {
			return new Parallax2();
		}
	}
}
