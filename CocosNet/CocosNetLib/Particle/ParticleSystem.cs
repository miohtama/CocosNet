
using System;
using CocosNet;
using CocosNet.Base;
using System.Drawing;
using CocosNet.Support;
using Color = CocosNet.Base.Color;

namespace CocosNet.Particle {

	public enum PositionType {
		Free,
		Grouped
	}

	public abstract class ParticleSystem : CocosNode {
		public const int ParticleStartSizeEqualToEndSize = -1;
		public const int ParticleDurationInfinity = -1;

		private static Random _rand = new Random((int)DateTime.Now.Ticks);
		
		/// <summary>
		/// Returns a random float between -1 and 1
		/// </summary>
		private static float Random11() {
			bool negative = _rand.Next(0, 2) == 0;
			
			float randomFloat = (float)_rand.NextDouble();
			
			if (negative) {
				randomFloat *= -1.0f;
			}
			
			return randomFloat;
		}

		#region Particle class
		protected class Particle {
			public PointF pos;
			public PointF startPos;
			public PointF dir;
			public float radialAccel;
			public float tangentialAccel;
			public ColorF color;
			public ColorF deltaColor;
			public float size;
			public float deltaSize;
			public float angle;
			public float deltaAngle;
			public float life;
		}
		#endregion

		protected Particle[] _particles;
		protected float _elapsed;
		protected float _emitCounter;
		protected int _particleIndex;
		

		public bool Active { get; private set; }
		public int ParticleCount { get; protected set; }
		public PointF Gravity { get; set; }
		public float Duration { get; set; }
		public PointF CenterOfGravity { get; set; }
		public PointF PosVar { get; set; }
		public float Life { get; set; }
		public float LifeVar { get; set; }
		public float Angle { get; set; }
		public float AngleVar { get; set; }
		public float Speed { get; set; }
		public float SpeedVar { get; set; }
		public float TangentialAccel { get; set; }
		public float TangentialAccelVar { get; set; }
		public float RadialAccel { get; set; }
		public float RadialAccelVar { get; set; }
		public float StartSize { get; set; }
		public float StartSizeVar { get; set; }
		public float EndSize { get; set; }
		public float EndSizeVar { get; set; }
		public ColorF StartColor { get; set; }
		public ColorF StartColorVar { get; set; }
		public ColorF EndColor { get; set; }
		public ColorF EndColorVar { get; set; }
		public float StartSpin { get; set; }
		public float StartSpinVar { get; set; }
		public float EndSpin { get; set; }
		public float EndSpinVar { get; set; }
		public float EmissionRate { get; set; }
		public int TotalParticles { get; set; }
		public Texture2D Texture { get; set; }
		public BlendFunc BlendFunc { get; set; }
		public bool BlendAdditive { get; set; }
		public PositionType PositionType { get; set; }
		public bool AutoRemoveOnFinish { get; set; }

		protected bool IsFull {
			get { return ParticleCount == TotalParticles; }
		}

		private void InitParticle(Particle particle) {
			//position
			particle.pos.X = (int)(CenterOfGravity.X + PosVar.X * Random11());
			particle.pos.Y = (int)(CenterOfGravity.Y + PosVar.Y * Random11());
			
			// direction
			float a = (Angle + AngleVar * Random11()).ToRadians();
			PointF v = new PointF((float)Math.Cos(a), (float)Math.Sin(a));
			float s = Speed + SpeedVar * Random11();
			v = v.Multiply(s);
			particle.dir = v;
			
			// radial accel
			particle.radialAccel = RadialAccel + RadialAccelVar * Random11();
			
			// tangential accel
			particle.tangentialAccel = TangentialAccel + TangentialAccelVar * Random11();
			
			// life
			float life = Life + LifeVar * Random11();
			particle.life = Math.Max(0, life);
			
			// color
			
			ColorF start = new ColorF();
			start.R = StartColor.R + StartColorVar.R * Random11();
			start.G = StartColor.G + StartColorVar.G * Random11();
			start.B = StartColor.B + StartColorVar.B * Random11();
			start.A = StartColor.A + StartColorVar.A * Random11();
			
			ColorF end = new ColorF();
			end.R = EndColor.R + EndColorVar.R * Random11();
			end.G = EndColor.G + EndColorVar.G * Random11();
			end.B = EndColor.B + EndColorVar.B * Random11();
			end.A = EndColor.A + EndColorVar.A * Random11();
			
			particle.color = start;
			particle.deltaColor.R = (end.R - start.R) / particle.life;
			particle.deltaColor.G = (end.G - start.G) / particle.life;
			particle.deltaColor.B = (end.B - start.B) / particle.life;
			particle.deltaColor.A = (end.A - start.A) / particle.life;

			// size
			float startS = StartSize + StartSizeVar * Random11();
			startS = Math.Max(0, startS);
			
			particle.size = startS;
			
			if (EndSize == ParticleStartSizeEqualToEndSize) {
				particle.deltaSize = 0;
			} else {
				float endS = EndSize + EndSizeVar * Random11();
				particle.deltaSize = (endS - startS) / particle.life;
			}
			
			// angle
			float startA = StartSpin + StartSpinVar * Random11();
			float endA = EndSpin + EndSpinVar * Random11();
			particle.angle = startA;
			particle.deltaAngle = (endA - startA) / particle.life;
			
			particle.startPos = this.Position;
		}

		protected bool AddParticle() {
			if (IsFull) {
				return false;
			}
			
			Particle p = _particles[ParticleCount];
			InitParticle(p);
			
			ParticleCount += 1;
			
			return true;
		}

		private void OnTick(object sender, TickEventArgs e) {
			Step(e.Delta);
		}

		protected abstract void Step(float dt);

		protected ParticleSystem(int totalParticles) {
			TotalParticles = totalParticles;
			
			_particles = new Particle[TotalParticles];
			for (int i = 0; i < _particles.Length; ++i) {
				_particles[i] = new Particle();
			}
			
			Active = true;
			BlendAdditive = false;
			BlendFunc = BlendFunc.DefaultBlendFunc;
			PositionType = PositionType.Free;
			
			AutoRemoveOnFinish = false;
			
			Scheduler.Instance.Tick += OnTick;
		}

		public void StopSystem() {
			Active = false;
			_elapsed = Duration;
			_emitCounter = 0;
		}

		public void ResetSystem() {
			Active = true;
			_elapsed = 0;
			for (_particleIndex = 0; _particleIndex < ParticleCount; ++_particleIndex) {
				_particles[_particleIndex].life = 0;
			}
		}
	}
}
