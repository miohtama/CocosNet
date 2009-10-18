// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using MonoTouch.UIKit;
using OpenTK.Platform;
using MonoTouch.OpenGLES;
using CocosNet;
using CocosNet.Layers;
using CocosNet.Support;
using Color = CocosNet.Base.Color;

namespace Cocos2dPortedTests {
	public partial class AppDelegate : UIApplicationDelegate {
		static void Main(string[] args) {
			using (var c = Utilities.CreateGraphicsContext(EAGLRenderingAPI.OpenGLES1)) {
				UIApplication.Main(args, null, "AppDelegate");
			}
		}

		public override void FinishedLaunching(UIApplication app) {
			window.BackgroundColor = UIColor.Black;
			
			window.UserInteractionEnabled = true;
			window.MultipleTouchEnabled = false;
			
			
			Director.Instance.DeviceOrientation = DeviceOrientation.LandscapeLeft;
			Director.Instance.AnimationInterval = 1.0 / 60.0;
			//Director.Instance.IsDisplayFPS = true;
			
			Director.Instance.AttachInView(window);
			
			window.MakeKeyAndVisible();

			// To run a different test, instantiate a different class here
			// SpriteTest -- SpriteManual
			// ParallaxTest -- Parallax1
			Scene scene = new Scene(new Parallax1());
			
			Director.Instance.RunScene(scene);
		}

		// This method is required in iPhoneOS 3.0
		public override void OnActivated(UIApplication application) {
		}
	}
}
