// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using CocosNet.Actions;
using CocosNet.Sprites;
using CocosNet.Tiles;

namespace Cocos2dPortedTests {
	public class Parallax1 : ParallaxDemo {
		public Parallax1() {
			Sprite cocosImage = new Sprite("powered.png");
			
			cocosImage.Scale = 2.5f;
			cocosImage.AnchorPoint = PointF.Empty;
			
			TileMapAtlas tileMap = new TileMapAtlas("tiles.png", "levelMap.tga", 16, 16);
			
			tileMap.AnchorPoint = PointF.Empty;
			
			Sprite background = new Sprite("background.png");
			
			background.Scale = 1.5f;
			background.AnchorPoint = PointF.Empty;
			
			ParallaxNode voidNode = new ParallaxNode();
			
			voidNode.AddChild(background, -1, new PointF(0.4f, 0.5f), PointF.Empty);
			voidNode.AddChild(tileMap, 1, new PointF(2.2f, 1f), new PointF(0, -200));
			voidNode.AddChild(cocosImage, 2, new PointF(3f, 2.5f), new PointF(200, 800));
			
			var goUp = new MoveBy(4, new PointF(0, -500));
			var goDown = goUp.Reverse() as MoveBy;
			var goForward = new MoveBy(8, new PointF(-1000, 0));
			var goBack = goForward.Reverse() as MoveBy;
			var sequence = Sequence.Construct(goUp, goForward, goDown, goBack);
			
			voidNode.RunAction(new RepeatForever(sequence));
			
			AddChild(voidNode);
		}

		public override string ToString() {
			return "Parallax: parent and 3 children";
		}
	}
}
