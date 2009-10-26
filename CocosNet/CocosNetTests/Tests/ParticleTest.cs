
using System;
using CocosNet.Sprites;
using CocosNet.Base;
using CocosNet.Labels;
using System.Drawing;
using CocosNet;
using CocosNet.Actions;
using CocosNet.Particle;
using CocosNet.Support;

namespace CocosNetTests {


	public abstract class ParticleDemo : TestBase {
		private static ParticleDemo[] MyScenes = {
			new DemoFirework(),
			new DemoFire(),
			new DemoSun(),
			new DemoGalaxy(),
			new DemoFlower(),
			new DemoMeteor(),
			new DemoSpiral(),
			//new DemoSmoke(),
			new DemoSnow(),
			new DemoRain()
		};

		protected ParticleSystem _emitter;
		//protected Sprite _background;

		protected override ICloneable[] Scenes {
			get { return MyScenes; }
		}

		protected void SetEmitterPosition() {
			_emitter.SetPosition(200, 120);
		}

		protected void CenterEmitter() {
			_emitter.SetPosition(Director.Instance.WinSize.Width / 2, Director.Instance.WinSize.Height / 2);
		}

		public override void RegisterWithTouchDispatcher() {
			TouchDispatcher.Instance.AddTargetedDelegate(this, 0, false);
		}

		public override void OnExit() {
			TouchDispatcher.Instance.RemoveDelegate(this);
			if (_emitter != null) {
				_emitter.OnExit();
			}
		}


		public override bool TouchBegan(MonoTouch.UIKit.UITouch touch, MonoTouch.UIKit.UIEvent evnt) {
			return true;
		}

		public override void TouchMoved(MonoTouch.UIKit.UITouch touch, MonoTouch.UIKit.UIEvent evnt) {
			TouchEnded(touch, evnt);
		}

		public override void TouchEnded(MonoTouch.UIKit.UITouch touch, MonoTouch.UIKit.UIEvent evnt) {
			Console.WriteLine("asdasd");
			PointF location = touch.LocationInView(touch.View);
			PointF convertedLocation = Director.Instance.ConvertCoordinate(location);
			
			_emitter.SetPosition(convertedLocation);
		}
	}

	public class DemoFirework : ParticleDemo {
		public override void OnEnter() {
			base.OnEnter();
			_emitter = new ParticleFireworks();
			AddChild(_emitter, 10);
			
			_emitter.Texture = TextureMgr.Instance.AddImage("stars.png");
			
			SetEmitterPosition();
		}

		public override string ToString() {
			return "ParticleFireworks";
		}

		public override object Clone() {
			return new DemoFirework();
		}
	}

	public class DemoFire : ParticleDemo {
		public override void OnEnter() {
			base.OnEnter();
			_emitter = new ParticleFire();
			
			AddChild(_emitter, 10);
			_emitter.Texture = TextureMgr.Instance.AddImage("fire.pvr");
			
			SetEmitterPosition();
		}

		public override string ToString() {
			return "ParticleFire";
		}

		public override object Clone() {
			return new DemoFire();
		}
	}

	public class DemoSun : ParticleDemo {
		public override void OnEnter() {
			base.OnEnter();
			_emitter = new ParticleSun();
			AddChild(_emitter, 10);
			
			_emitter.Texture = TextureMgr.Instance.AddImage("fire.pvr");
			
			CenterEmitter();
		}

		public override string ToString() {
			return "ParticleSun";
		}

		public override object Clone() {
			return new DemoSun();
		}
	}

	public class DemoGalaxy : ParticleDemo {
		public override void OnEnter() {
			base.OnEnter();
			_emitter = new ParticleGalaxy();
			AddChild(_emitter, 10);
			
			_emitter.Texture = TextureMgr.Instance.AddImage("fire.pvr");
			
			CenterEmitter();
		}

		public override string ToString() {
			return "ParticleGalaxy";
		}

		public override object Clone() {
			return new DemoGalaxy();
		}
	}

	public class DemoFlower : ParticleDemo {
		public override void OnEnter() {
			base.OnEnter();
			_emitter = new ParticleFlower();
			AddChild(_emitter, 10);
			_emitter.Texture = TextureMgr.Instance.AddImage("stars.png");
			
			CenterEmitter();
		}

		public override string ToString() {
			return "ParticleFlower";
		}

		public override object Clone() {
			return new DemoFlower();
		}
	}

	public class DemoMeteor : ParticleDemo {
		public override void OnEnter() {
			base.OnEnter();
			_emitter = new ParticleMeteor();
			AddChild(_emitter, 10);
			
			_emitter.Texture = TextureMgr.Instance.AddImage("fire.pvr");
			
			CenterEmitter();
		}

		public override string ToString() {
			return "ParticleMeteor";
		}

		public override object Clone() {
			return new DemoMeteor();
		}
	}

	public class DemoSpiral : ParticleDemo {
		public override void OnEnter() {
			base.OnEnter();
			_emitter = new ParticleSpiral();
			AddChild(_emitter, 10);
			
			_emitter.Texture = TextureMgr.Instance.AddImage("fire.pvr");
			
			CenterEmitter();
		}

		public override string ToString() {
			return "ParticleSpiral";
		}

		public override object Clone() {
			return new DemoSpiral();
		}
	}

	public class DemoSmoke : ParticleDemo {
		public override void OnEnter() {
			base.OnEnter();
			_emitter = new ParticleSmoke();
			AddChild(_emitter, 10);
			
			SetEmitterPosition();
		}

		public override string ToString() {
			return "ParticleSmoke";
		}

		public override object Clone() {
			return new DemoSmoke();
		}
	}

	public class DemoSnow : ParticleDemo {
		public override void OnEnter() {
			base.OnEnter();
			_emitter = new ParticleSnow();
			AddChild(_emitter, 10);
			_emitter.Life = 3;
			_emitter.LifeVar = 1;
			
			_emitter.Gravity = new PointF(0, -10);
			_emitter.Speed = 130;
			_emitter.SpeedVar = 30;
			
			_emitter.StartColor = ColorF.FromRGBA(0.9f, 0.9f, 0.9f, 1f);
			_emitter.StartColorVar = ColorF.FromRGBA(0, 0, 0.1f, 0);
			
			_emitter.EmissionRate = _emitter.TotalParticles / _emitter.Life;
			
			_emitter.Texture = TextureMgr.Instance.AddImage("snow.png");
			
			_emitter.SetPosition(Director.Instance.WinSize.Width / 2, Director.Instance.WinSize.Height);
		}

		public override string ToString() {
			return "ParticleSnow";
		}

		public override object Clone() {
			return new DemoSnow();
		}
	}

	public class DemoRain : ParticleDemo {
		public override void OnEnter() {
			base.OnEnter();
			_emitter = new ParticleRain();
			AddChild(_emitter, 10);
			_emitter.Life = 4;
			
			_emitter.Texture = TextureMgr.Instance.AddImage("fire.pvr");
			_emitter.SetPosition(Director.Instance.WinSize.Width / 2, Director.Instance.WinSize.Height);
		}

		public override string ToString() {
			return "ParticleRain";
		}

		public override object Clone() {
			return new DemoRain();
		}
	}
}
