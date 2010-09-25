// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using CocosNet.Support;
using System.Drawing;
using CocosNet.Actions;

namespace CocosNet.Layers {
	public abstract class TransitionScene : Scene {
		protected float _duration;
		protected Scene _inScene;
		protected Scene _outScene;
		protected bool _inSceneOnTop;
		
		protected virtual void SetSceneOrder() {
			_inSceneOnTop = true;
		}
		
		protected virtual void SetNewScene() {
			Director.Instance.ReplaceScene(_inScene);
			TouchDispatcher.Instance.DispatchEvents = true;
		}
		
		protected TransitionScene(float duration, Scene inScene) {
			if (duration < 0) {
				throw new ArgumentException("duration must be positive", "duration");
			}
			
			if (inScene == null) {
				throw new ArgumentNullException("inScene");
			}
			
			_inScene = inScene;
			_inScene.Visible = true;
			
			_outScene = Director.Instance.RunningScene;
			
			if (ReferenceEquals(_inScene, _outScene)) {
				throw new InvalidOperationException("TransitionScene: the in and out scenes must be different");
			}
			
			TouchDispatcher.Instance.DispatchEvents = false;
			
			SetSceneOrder();
		}
		
		public override void OnEnter() {
			base.OnEnter();
			_inScene.OnEnter();
		}
		
		public override void OnExit() {
			base.OnExit();
			_outScene.OnExit();
			
			_inScene.OnEnterTransitionDidFinish();
		}
		
		public override void Draw() {
			if (_inSceneOnTop) {
				_outScene.Visit();
				_inScene.Visit();
			} else {
				_inScene.Visit();
				_outScene.Visit();
			}
		}
			
		protected void Finish() {
			_inScene.Visible = true;
			_inScene.SetPosition(PointF.Empty);
			_inScene.Scale = 1.0f;
			_inScene.Rotation = 0;
			_inScene.Camera.Restore();
			
			_outScene.Visible = false;
			_outScene.SetPosition(PointF.Empty);
			_outScene.Scale = 1.0f;
			_outScene.Rotation = 0;
			_outScene.Camera.Restore();
			
			SetNewScene();
		}
	}
	
	public class MoveInLeftTransition : TransitionScene {
		private void InitScenes() {
			_inScene.SetPosition(new PointF(-Director.Instance.DisplaySize.Width, 0));	
		}
		
		public MoveInLeftTransition(float duration, Scene inScene)
			: base(duration, inScene) {
		}
		
		public override void OnEnter() {
			base.OnEnter();
			
			InitScenes();
			
			IntervalAction action = new MoveTo(_duration, PointF.Empty);
			
			_inScene.RunAction(new Sequence(action, new CallFunc(Finish)));
		}	
	}
}
