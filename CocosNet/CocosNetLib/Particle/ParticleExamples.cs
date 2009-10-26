using System;
using CocosNet.Base;
using System.Drawing;

namespace CocosNet.Particle {


	public class ParticleFireworks : PointParticleSystem {

		public ParticleFireworks() : base(1500) {
			
			Duration = -1;
			Gravity = new PointF(0, -90);
			
			Angle = 90;
			AngleVar = 20;
			
			RadialAccel = 0;
			RadialAccelVar = 0;
			
			Speed = 180;
			SpeedVar = 50;
			
			SetPosition(160, 160);
			
			Life = 3.5f;
			LifeVar = 1;
			
			EmissionRate = TotalParticles / Life;
			
			StartColor = ColorF.FromRGBA(0.5f, 0.5f, 0.5f, 1f);
			StartColorVar = ColorF.FromRGBA(0.5f, 0.5f, 0.5f, 0.1f);
			EndColor = ColorF.FromRGBA(0.1f, 0.1f, 0.1f, 0.2f);
			EndColorVar = ColorF.FromRGBA(0.1f, 0.1f, 0.1f, 0.2f);
			
			StartSize = 8f;
			StartSizeVar = 2f;
			EndSize = ParticleStartSizeEqualToEndSize;
			
			Texture = TextureMgr.Instance.AddImage("fire.png");
			
			BlendAdditive = false;
		}
	}

	public class ParticleFire : PointParticleSystem {
		public ParticleFire() : base(250) {
			// duration
			Duration = ParticleDurationInfinity;
			
			// gravity
			Gravity = PointF.Empty;
			
			// angle
			Angle = 90;
			AngleVar = 10;
			
			// radial acceleration
			RadialAccel = 0;
			RadialAccelVar = 0;
			
			// emitter position
			SetPosition(160, 60);
			PosVar = new PointF(40, 20);
			
			// life of particles
			Life = 3;
			LifeVar = 0.25f;
			
			// speed of particles
			Speed = 60;
			SpeedVar = 20;
			
			// size, in pixels
			StartSize = 54f;
			StartSizeVar = 10f;
			EndSize = ParticleStartSizeEqualToEndSize;
			
			// emits per frame
			EmissionRate = TotalParticles / Life;
			
			// color of particles
			StartColor = ColorF.FromRGBA(0.76f, 0.25f, 0.12f, 1f);
			StartColorVar = ColorF.FromRGBA(0, 0, 0, 0);
			EndColor = ColorF.FromRGBA(0, 0, 0, 1);
			EndColorVar = ColorF.FromRGBA(0, 0, 0, 0);
			
			Texture = TextureMgr.Instance.AddImage("fire.png");
			
			// additive
			BlendAdditive = true;
		}
	}

	public class ParticleSun : PointParticleSystem {
		public ParticleSun() : base(350) {
			BlendAdditive = true;
			Duration = ParticleDurationInfinity;
			
			Gravity = Point.Empty;
			
			Angle = 90;
			AngleVar = 360;
			
			RadialAccel = 0;
			RadialAccelVar = 0;
			
			SetPosition(160, 240);
			PosVar = PointF.Empty;
			
			Life = 1;
			LifeVar = 0.5f;
			
			Speed = 20;
			SpeedVar = 5;
			
			StartSize = 30f;
			StartSizeVar = 10f;
			EndSize = ParticleStartSizeEqualToEndSize;
			
			EmissionRate = TotalParticles / Life;
			
			StartColor = ColorF.FromRGBA(0.76f, 0.25f, 0.12f, 1f);
			StartColorVar = ColorF.FromRGBA(0, 0, 0, 0);
			EndColor = ColorF.FromRGBA(0, 0, 0, 1);
			EndColorVar = ColorF.FromRGBA(0, 0, 0, 0);
			
			Texture = TextureMgr.Instance.AddImage("fire.png");
		}
	}

	public class ParticleGalaxy : PointParticleSystem {
		public ParticleGalaxy() : base(200) {
			Duration = ParticleDurationInfinity;
			
			Gravity = PointF.Empty;
			
			Angle = 90;
			AngleVar = 360;
			
			Speed = 60;
			SpeedVar = 10;
			
			RadialAccel = -80;
			RadialAccelVar = 0;
			
			TangentialAccel = 80;
			TangentialAccelVar = 0;
			
			SetPosition(160, 240);
			PosVar = PointF.Empty;
			
			Life = 4;
			LifeVar = 1;
			
			StartSize = 37;
			StartSizeVar = 10;
			EndSize = ParticleStartSizeEqualToEndSize;
			
			EmissionRate = TotalParticles / Life;
			
			StartColor = ColorF.FromRGBA(0.12f, 0.25f, 0.76f, 1f);
			StartColorVar = ColorF.FromRGBA(0, 0, 0, 0);
			EndColor = ColorF.FromRGBA(0, 0, 0, 1);
			EndColorVar = ColorF.FromRGBA(0, 0, 0, 0);
			
			Texture = TextureMgr.Instance.AddImage("fire.png");
			
			BlendAdditive = true;
		}
	}

	public class ParticleFlower : PointParticleSystem {
		public ParticleFlower() : base(250) {
			Duration = ParticleDurationInfinity;
			
			Gravity = PointF.Empty;
			
			Angle = 90;
			AngleVar = 360;
			
			Speed = 80;
			SpeedVar = 10;
			
			RadialAccel = -60;
			RadialAccelVar = 0;
			
			TangentialAccel = 15;
			TangentialAccelVar = 0;
			
			SetPosition(160, 240);
			PosVar = PointF.Empty;
			
			Life = 4;
			LifeVar = 1;
			
			StartSize = 30;
			StartSizeVar = 10;
			EndSize = ParticleStartSizeEqualToEndSize;
			
			EmissionRate = TotalParticles / Life;
			
			StartColor = ColorF.FromRGBA(0.5f, 0.5f, 0.5f, 1);
			StartColorVar = ColorF.FromRGBA(0.5f, 0.5f, 0.5f, 0.5f);
			EndColor = ColorF.FromRGBA(0, 0, 0, 1);
			EndColorVar = ColorF.FromRGBA(0, 0, 0, 0);
			
			Texture = TextureMgr.Instance.AddImage("fire.png");
			
			BlendAdditive = true;
		}
	}

	public class ParticleMeteor : PointParticleSystem {
		public ParticleMeteor() : base(150) {
			Duration = ParticleDurationInfinity;
			
			Gravity = new PointF(-200, 200);
			
			Angle = 90;
			AngleVar = 360;
			
			Speed = 15;
			SpeedVar = 5;
			
			RadialAccel = 0;
			RadialAccelVar = 0;
			
			TangentialAccel = 0;
			TangentialAccelVar = 0;
			
			SetPosition(160, 240);
			PosVar = PointF.Empty;
			
			Life = 2;
			LifeVar = 1;
			
			StartSize = 60;
			StartSizeVar = 10;
			EndSize = ParticleStartSizeEqualToEndSize;
			
			EmissionRate = TotalParticles / Life;
			
			StartColor = ColorF.FromRGBA(0.2f, 0.4f, 0.7f, 1);
			StartColorVar = ColorF.FromRGBA(0, 0, 0.2f, 0.1f);
			EndColor = ColorF.FromRGBA(0, 0, 0, 1);
			EndColorVar = ColorF.FromRGBA(0, 0, 0, 0);
			
			Texture = TextureMgr.Instance.AddImage("fire.png");
			
			BlendAdditive = true;
		}
	}

	public class ParticleSpiral : PointParticleSystem {
		public ParticleSpiral() : base(500) {
			Duration = ParticleDurationInfinity;
			
			Gravity = PointF.Empty;
			
			Angle = 90;
			AngleVar = 0;
			
			Speed = 150;
			SpeedVar = 0;
			
			RadialAccel = -380;
			RadialAccelVar = 0;
			
			TangentialAccel = 45;
			TangentialAccelVar = 0;
			
			SetPosition(160, 240);
			PosVar = PointF.Empty;
			
			Life = 12;
			LifeVar = 0;
			
			StartSize = 20;
			StartSizeVar = 0;
			EndSize = ParticleStartSizeEqualToEndSize;
			
			EmissionRate = TotalParticles / Life;
			
			StartColor = ColorF.FromRGBA(0.5f, 0.5f, 0.5f, 1f);
			StartColorVar = ColorF.FromRGBA(0.5f, 0.5f, 0.5f, 0);
			EndColor = ColorF.FromRGBA(0.5f, 0.5f, 0.5f, 1f);
			EndColorVar = ColorF.FromRGBA(0.5f, 0.5f, 0.5f, 0);
			
			Texture = TextureMgr.Instance.AddImage("fire.png");
			
			BlendAdditive = true;
		}
	}

	public class ParticleSmoke : PointParticleSystem {
		public ParticleSmoke() : base(200) {
			
		}
	}

	public class ParticleSnow : PointParticleSystem {
		public ParticleSnow() : base(700) {
			Duration = ParticleDurationInfinity;
			
			Gravity = new PointF(0, -1);
			
			Angle = -90;
			AngleVar = 5;
			
			Speed = 5;
			SpeedVar = 1;
			
			RadialAccel = 0;
			RadialAccelVar = 1;
			
			TangentialAccel = 0;
			TangentialAccelVar = 1;
			
			SetPosition(Director.Instance.WinSize.Width / 2, Director.Instance.WinSize.Height + 10);
			PosVar = new PointF(Director.Instance.WinSize.Width / 2f, 0);
			
			Life = 45;
			LifeVar = 15;
			
			StartSize = 10;
			StartSizeVar = 5;
			EndSize = ParticleStartSizeEqualToEndSize;
			
			EmissionRate = 10;
			
			StartColor = ColorF.FromRGBA(1, 1, 1, 1);
			StartColorVar = ColorF.FromRGBA(0, 0, 0, 0);
			EndColor = ColorF.FromRGBA(1, 1, 1, 0);
			EndColorVar = ColorF.FromRGBA(0, 0, 0, 0);
			
			Texture = TextureMgr.Instance.AddImage("fire.png");
			
			BlendAdditive = false;
		}
	}

	public class ParticleRain : PointParticleSystem {
		public ParticleRain() : base(1000) {
			Duration = ParticleDurationInfinity;
			
			Gravity = new PointF(10, -10);
			
			Angle = -90;
			AngleVar = 5;
			
			Speed = 130;
			SpeedVar = 30;
			
			RadialAccel = 0;
			RadialAccelVar = 1;
			
			TangentialAccel = 0;
			TangentialAccelVar = 1;
			
			SetPosition(Director.Instance.WinSize.Width / 2, Director.Instance.WinSize.Height - 200);
			PosVar = new PointF(Director.Instance.WinSize.Width / 2, 0);
			
			Life = 4.5f;
			LifeVar = 0;
			
			StartSize = 6;
			StartSizeVar = 2;
			EndSize = ParticleStartSizeEqualToEndSize;
			
			EmissionRate = 40;
			
			StartColor = ColorF.FromRGBA(0.7f, 0.8f, 1, 1);
			StartColorVar = ColorF.FromRGBA(0, 0, 0, 0);
			EndColor = ColorF.FromRGBA(0.7f, 0.8f, 1f, 0.8f);
			EndColorVar = ColorF.FromRGBA(0, 0, 0, 0);
			
			Texture = TextureMgr.Instance.AddImage("fire.png");
			
			BlendAdditive = true;
		}
	}
}
