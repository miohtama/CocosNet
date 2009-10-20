// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK.Graphics.ES11;
using System.Diagnostics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.OpenGLES;
using System.ComponentModel;
using MonoTouch.ObjCRuntime;
using System.Threading;
using CocosNet.Labels;
using CocosNet.Layers;
using CocosNet.Support;

namespace CocosNet {

	public enum DeviceOrientation {
		Portrait,
		PortraitUpsideDown,
		LandscapeLeft,
		LandscapeRight
	}

	public enum DepthBufferFormat {
		DepthBufferNone,
		DepthBuffer16,
		DepthBuffer24
	}

	public class Director {
		public const int DefaultFPS = 60;

		private static Director _instance = new Director();

		public static Director Instance {
			get { return _instance; }
		}

		private WeakReference _nextScene;
		private Stack<Scene> _sceneStack;
		private int _frames;
		private float _dt;
		private float _accumDt;
		private float _frameRate;
		private LabelAtlas _fpsLabel;

		private NSTimer _animationTimer;

		private double _animationInterval;
		private double _oldAnimationInterval;
		private DateTime _lastUpdate;
		private Texture2DPixelFormat _pixelFormat;
		private DepthBufferFormat _depthBufferFormat;
		private DeviceOrientation _deviceOrientation;

		private Director() {
			_pixelFormat = Texture2DPixelFormat.RGB565;
			_depthBufferFormat = DepthBufferFormat.DepthBufferNone;
			RunningScene = null; 
			_oldAnimationInterval = AnimationInterval = 1.0 / DefaultFPS;
			
			_sceneStack = new Stack<Scene>(10);
			_nextScene = new WeakReference(null);
			
			DeviceOrientation = DeviceOrientation.Portrait;
			
			IsDisplayFPS = false;
			_frames = 0;
			
			IsPaused = false;
		}

		private void ShowFPS() {
			++_frames;
			_accumDt += _dt;
			
			if (_accumDt > 0.1f) {
				_frameRate = _frames / _accumDt;
				_frames = 0;
				_accumDt = 0;
				
				_fpsLabel.Text = string.Format("{0:0.00}", _frameRate);
			}
		
			_fpsLabel.Draw();
		}

		private void SetAlphaBlending(bool @on) {
			if (@on) {
				GL.Enable(All.Blend);
				GL.BlendFunc(All.BlendSrc, All.BlendDst);
			} else {
				GL.Disable(All.Blend);
			}
		}

		private void SetTexture2D(bool @on) {
			if (@on) {
				GL.Enable(All.Texture2D);
			} else {
				GL.Disable(All.Texture2D);
			}
		}

		private void SetDepthTest(bool @on) {
			if (@on) {
				GL.ClearDepth(1f);
				GL.Enable(All.DepthTest);
				GL.DepthFunc(All.Lequal);
				GL.Hint(All.PerspectiveCorrectionHint, All.Nicest);
			} else {
				GL.Disable(All.DepthTest);
			}
		}

		private void InitGLDefaultValues() {
			Debug.Assert(OpenGLView != null, "InitGLDefaultValues; Should only be called after the OpenGL view was initialized");
			
			SetAlphaBlending(true);
			SetDepthTest(true);
			SetDefaultProjection();
			
			GL.ClearColor(0, 0, 0, 1);
			
			if (_fpsLabel == null) {
				_fpsLabel = new LabelAtlas("000.00", "fps_images.png", 16, 24, '.');
			}
		}

		private void CalculateDeltaTime() {
			DateTime now = DateTime.Now;
			
			if (NextDeltaTimeZero) {
				_dt = 0;
				NextDeltaTimeZero = false;
			} else {
				TimeSpan delta = now - _lastUpdate;
				
				_dt = (float)Math.Max(0, delta.TotalSeconds);
			}
			
			_lastUpdate = now;
		}

		private void MainLoop() {
			try {
				OpenGLView.SetCurrentContext();
				
				GL.Clear((int)(All.ColorBufferBit | All.DepthBufferBit));
				
				CalculateDeltaTime();
				
				if (!IsPaused) {
					Scheduler.Instance.OnTick(_dt);
				}
				
				if (_nextScene.Target != null) {
					SetNextScene();
				}
				
				GL.PushMatrix();
				
				ApplyLandscape();
				
				if (RunningScene != null) {
					RunningScene.Visit();
				}
				
				if (IsDisplayFPS) {
					ShowFPS();
				}
				
				GL.PopMatrix();
				
				OpenGLView.SwapBuffers();
				
			} catch (Exception e) {
				Console.WriteLine("EXCEPTION: " + e.ToString());
			}
		}

		private bool IsOpenGLAttached {
			get { return OpenGLView != null && OpenGLView.Superview != null; }
		}

		private bool InitOpenGlView(UIView view, RectangleF rect) {
			if (IsOpenGLAttached) {
				throw new InvalidOperationException("Can't re-attach the OpenGL View, because it is already attached. Detach it first.");
			}
			
			if (OpenGLView == null) {
				All format = All.Rgb565Oes;
				All depthFormat = 0;
				
				if (PixelFormat == Texture2DPixelFormat.RGBA8888) {
					format = All.Rgba8Oes;
				}
				
				if (DepthBufferFormat == DepthBufferFormat.DepthBuffer16) {
					depthFormat = All.DepthComponent16Oes;
				} else if (DepthBufferFormat == DepthBufferFormat.DepthBuffer24) {
					depthFormat = All.DepthComponent24Oes;
				}
				
				try {
					OpenGLView = new EAGLView(rect, format, depthFormat, false);
				} catch (Exception e) {
					throw new InvalidOperationException("Could not alloc and init the OpenGL View: " + e.ToString());
				}
				
				OpenGLView.SetAutoResizesEaglSurface(true);
			
			} else {
				OpenGLView.Frame = rect;
			}
			
			OpenGLView.TouchDelegate = TouchDispatcher.Instance;
			
			OpenGLView.UserInteractionEnabled = view.UserInteractionEnabled;
			TouchDispatcher.Instance.DispatchEvents = view.UserInteractionEnabled;
			
			OpenGLView.MultipleTouchEnabled = view.MultipleTouchEnabled;
			
			view.AddSubview(OpenGLView);
			
			if (IsOpenGLAttached) {
				InitGLDefaultValues();
				return true;
			}
			
			throw new Exception("Can't attach the OpenGL View.");
		}

		private void Set3DProjection() {
			GL.Viewport(0, 0, (int)OpenGLView.Frame.Size.Width, (int)OpenGLView.Frame.Size.Height);
			GL.MatrixMode(All.Projection);
			GL.LoadIdentity();
			GLU.Perspective(60, (float)OpenGLView.Frame.Size.Width / OpenGLView.Frame.Size.Height, 0.5f, 1500f);
			
			GL.MatrixMode(All.Modelview);
			GL.LoadIdentity();
			GLU.LookAt(OpenGLView.Frame.Size.Width / 2, OpenGLView.Frame.Size.Height / 2, Camera.ZEye, OpenGLView.Frame.Size.Width / 2, OpenGLView.Frame.Size.Height / 2, 0, 0f, 1f, 0f);
		}



		private void SetDefaultProjection() {
			Set3DProjection();
		}

		public SizeF WinSize {
			get {
				SizeF s = OpenGLView.Frame.Size;
				
				if (DeviceOrientation == DeviceOrientation.LandscapeLeft || DeviceOrientation == DeviceOrientation.LandscapeRight) {
					s.Width = OpenGLView.Frame.Size.Height;
					s.Height = OpenGLView.Frame.Size.Width;
				}
				
				return s;
			}
		}

		public SizeF DisplaySize {
			get { return OpenGLView.Frame.Size; }
		}

		public bool Landscape {
			get { return DeviceOrientation == DeviceOrientation.LandscapeLeft; }
			set {
				if (value) {
					DeviceOrientation = DeviceOrientation.LandscapeLeft;
				} else {
					DeviceOrientation = DeviceOrientation.Portrait;
				}
			}
		}

		public Scene RunningScene { get; private set; }

		public double AnimationInterval {
			get { return _animationInterval; }
			set {
				_animationInterval = value;
				if (_animationTimer != null) {
					StopAnimation();
					StartAnimation();
				}
			}
		}

		public bool IsDisplayFPS { get; set; }

		public EAGLView OpenGLView { get; private set; }

		public Texture2DPixelFormat PixelFormat {
			get { return _pixelFormat; }
			set {
				if (IsOpenGLAttached) {
					throw new InvalidOperationException("Can't change the pixel format after the director was initialized");
				}
				
				_pixelFormat = value;
			}
		}

		public DepthBufferFormat DepthBufferFormat {
			get { return _depthBufferFormat; }
			set {
				if (IsOpenGLAttached) {
					throw new InvalidOperationException("Can't change depth buffer format after the director was initialized");
				}
				
				_depthBufferFormat = value;
			}
		}

		public bool NextDeltaTimeZero { get; set; }

		public DeviceOrientation DeviceOrientation {
			get { return _deviceOrientation; }
			set {
				//if (value != _deviceOrientation) {
				_deviceOrientation = value;
				switch (_deviceOrientation) {
					case DeviceOrientation.Portrait:
					case DeviceOrientation.PortraitUpsideDown:
						UIApplication.SharedApplication.SetStatusBarOrientation(UIInterfaceOrientation.Portrait, false);
						break;
					case DeviceOrientation.LandscapeLeft:
						UIApplication.SharedApplication.SetStatusBarOrientation(UIInterfaceOrientation.LandscapeRight, false);
						break;
					case DeviceOrientation.LandscapeRight:
						UIApplication.SharedApplication.SetStatusBarOrientation(UIInterfaceOrientation.LandscapeLeft, false);
						break;
					default:
						throw new InvalidEnumArgumentException("DeviceOrientation", (int)_deviceOrientation, typeof(DeviceOrientation));
					
				}
				//}
			}
		}

		public void ApplyLandscape() {
			switch (DeviceOrientation) {
				case DeviceOrientation.Portrait:
					// do nothing
					break;
				case DeviceOrientation.PortraitUpsideDown:
					// upside down
					GL.Translate(160, 240, 0);
					GL.Rotate(180, 0, 0, 1);
					GL.Translate(-160, -240, 0);
					break;
				case DeviceOrientation.LandscapeRight:
					GL.Translate(160, 240, 0);
					GL.Rotate(90, 0, 0, 1);
					GL.Translate(-240, -160, 0);
					break;
				case DeviceOrientation.LandscapeLeft:
					GL.Translate(160, 240, 0);
					GL.Rotate(-90, 0, 0, 1);
					GL.Translate(-240, -160, 0);
					break;
			}
		}

		public bool IsPaused { get; private set; }

		public bool Detach() {
			if (!IsOpenGLAttached) {
				throw new InvalidOperationException("Can't detach the OpenGL View, because it is not attached. Attach it first.");
			}
			
			OpenGLView.RemoveFromSuperview();
			
			if (!IsOpenGLAttached) {
				return true;
			}
			
			throw new InvalidOperationException("Can't detach the OpenGL View, it is still attached to the superview.");
		}

		public bool AttachInWindow(UIWindow window) {
			return InitOpenGlView(window, window.Bounds);
		}

		public bool AttachInView(UIView view) {
			return InitOpenGlView(view, view.Bounds);
		}

		public void RunScene(Scene scene) {
			if (scene == null) {
				throw new ArgumentNullException("scene");
			}
			
			if (RunningScene != null) {
				throw new InvalidOperationException("You can't run a scene if another Scene is running. Use ReplaceScene or PushScene instead");
			}
			
			PushScene(scene);
			StartAnimation();
		}

		public void ReplaceScene(Scene scene) {
			if (scene == null) {
				throw new ArgumentNullException("scene");
			}
			
			_sceneStack.Pop();
			_sceneStack.Push(scene);
			_nextScene.Target = scene;
		}

		public void PushScene(Scene scene) {
			if (scene == null) {
				throw new ArgumentNullException("scene");
			}
			
			_sceneStack.Push(scene);
			_nextScene.Target = scene;
		}

		public void PopScene(Scene scene) {
			if (scene == null) {
				throw new ArgumentNullException("scene");
			}
			
			_sceneStack.Pop();
			if (_sceneStack.IsEmpty()) {
				End();
			} else {
				_nextScene.Target = _sceneStack.Peek();
			}
		}

		public void End() {
			RunningScene.OnExit();
			RunningScene.CleanUp();
			
			_sceneStack.Clear();
			
			TouchDispatcher.Instance.RemoveAllDelegates();
			
			StopAnimation();
			Detach();
		}

		private void SetNextScene() {
			Debug.Assert(_nextScene != null, "_nextScene is null");
			Debug.Assert(_nextScene.Target != null, "Called SetNextScene when there is none (_nextScene.Target is null)");
			
			bool runningIsTransition = RunningScene != null && RunningScene is TransitionScene;
			bool newIsTransition = _nextScene.Target != null && _nextScene.Target is TransitionScene;
			
			if (!newIsTransition && RunningScene != null) {
				RunningScene.OnExit();
			}
			
			RunningScene = _nextScene.Target as Scene;
			_nextScene.Target = null;
			
			if (!runningIsTransition) {
				RunningScene.OnEnter();
				RunningScene.OnEnterTransitionDidFinish();
			}
		}

		public void Pause() {
			if (IsPaused) {
				return;
			}
			
			_oldAnimationInterval = AnimationInterval;
			
			AnimationInterval = 1 / 4.0;
			IsPaused = true;
		}

		public void Resume() {
			if (!IsPaused) {
				return;
			}
			
			AnimationInterval = _oldAnimationInterval;
			IsPaused = false;
			_dt = 0;
		}

		/// <summary>
		/// This class is to enable us to create an NSTimer with sub millisecond precision.
		/// This allows our timer to run at exactly 60fps. TimeSpan is MonoTouch's
		/// means of creating timers, and TimeSpan's best precision is a millisecond. So
		/// With a TimeSpan, the framerate would end up being either 58 or 62, depending
		/// on if you round up or down
		/// </summary>
		/// <remarks>
		/// NSActionDispatcher is an internal MonoTouch class, this class mimics it
		/// </remarks>
		private class MyNSActionDispatcher : NSObject {
			private const string SelectorName = "doAction";
			private NSAction _action;
			
			public MyNSActionDispatcher(NSAction action) {
				if (action == null) {
					throw new ArgumentNullException("action");
				}
				_action = action;
			}
			
			[Export(SelectorName)]
			public void DoAction() {
				_action();
			}
			
			public static Selector Selector {
				get {
					return new Selector(SelectorName);
				}
			}
		}
		
		private void StartAnimation() {
			Debug.Assert(_animationTimer == null, "_animationTimer must be null. Calling StartAnimation twice?");
			_animationTimer = NSTimer.CreateTimer(AnimationInterval, new MyNSActionDispatcher(MainLoop), MyNSActionDispatcher.Selector, null, true);
			NSRunLoop.Main.AddTimer(_animationTimer, "NSDefaultRunLoopMode");
		}

		private void StopAnimation() {
			_animationTimer.Dispose();
			_animationTimer = null;
		}
		
		public PointF ConvertCoordinate(PointF p) {
			float newY = OpenGLView.Frame.Size.Height - p.Y;
			float newX = OpenGLView.Frame.Size.Width - p.X;
			
			PointF ret = PointF.Empty;
			
			switch (DeviceOrientation) {
				case DeviceOrientation.Portrait:
					ret = new PointF(p.X, newY);
					break;
				case DeviceOrientation.PortraitUpsideDown:
					ret = new PointF(newX, p.Y);
					break;
				case DeviceOrientation.LandscapeLeft:
					ret.X = p.Y;
					ret.Y = p.X;
					break;
				case DeviceOrientation.LandscapeRight:
					ret.X = newX;
					ret.Y = newY;
					break;
			}
			
			return ret;
		}
	}
}
