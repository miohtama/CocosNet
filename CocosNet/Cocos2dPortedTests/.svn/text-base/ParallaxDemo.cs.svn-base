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

namespace Cocos2dPortedTests {
    public abstract class ParallaxDemo : Layer {
        protected ParallaxDemo() {
            SizeF s = Director.Instance.WinSize;

            Label label = new Label(ToString(), "Arial", 32);
            AddChild(label, 1);
            label.Position = new PointF(s.Width / 2, s.Height - 50);

		
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

        private void OnBack(object sender, EventArgs e) {
        }

        private void OnForward(object sender, EventArgs e) {
        }

        private void OnRestart(object sender, EventArgs e) {
        }
    }
}
