
using System;
using CocosNet;
using CocosNet.Layers;
using CocosNet.Base;
using CocosNet.Labels;
using CocosNet.Menus;
using System.Drawing;

namespace CocosNetTests {
	public abstract class TestBase : ColorLayer, ICloneable {
		private static int _index = 0;
		
		private void OnBack(object sender, EventArgs e) {
			Director.Instance.ReplaceScene(new Scene(GetBackScene()));
		}

		private void OnForward(object sender, EventArgs e) {
			Director.Instance.ReplaceScene(new Scene(GetNextScene()));
		}

		private void OnRestart(object sender, EventArgs e) {
			Director.Instance.ReplaceScene(new Scene(GetRestartScene()));
		}
		
		private CocosNode GetNextScene() {
			++_index;
			_index = _index % Scenes.Length;
			
			
			return Scenes[_index].Clone() as CocosNode;
		}

		private CocosNode GetBackScene() {
			--_index;
			if (_index < 0) {
				_index = Scenes.Length - 1;
			}
			
			return Scenes[_index].Clone() as CocosNode;
		}

		private CocosNode GetRestartScene() {
			return Scenes[_index].Clone() as CocosNode;
		}
		
		protected abstract ICloneable[] Scenes { get; }
		
		public TestBase() {
			SizeF s = Director.Instance.WinSize;
			
			Label label = new Label(ToString(), "Arial", 32);
			AddChild(label, 100);
			
			label.SetPosition(s.Width / 2f, s.Height - 50);
			
			MenuItemImage item1 = new MenuItemImage("b1.png", "b2.png");
			item1.Click += OnBack;
			
			MenuItemImage item2 = new MenuItemImage("r1.png", "r2.png");
			item2.Click += OnRestart;
			
			MenuItemImage item3 = new MenuItemImage("f1.png", "f2.png");
			item3.Click += OnForward;
			
			Menu menu = new Menu(item1, item2, item3);
			
			menu.SetPosition(PointF.Empty);
			
			float windowWidth = Director.Instance.WinSize.Width;
			
			item1.SetPosition(windowWidth / 2 - 100, 30);
			item2.SetPosition(windowWidth / 2, 30);
			item3.SetPosition(windowWidth / 2 + 100, 30);
			
			AddChild(menu, 5000);	
		}
		
		public abstract object Clone();
	}
}
