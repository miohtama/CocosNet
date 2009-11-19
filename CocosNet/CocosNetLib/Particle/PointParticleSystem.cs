using System;
using OpenTK.Graphics.ES11;
using CocosNet.Base;
using System.Drawing;
using CocosNet.Support;
using CocosNet.Vector;

namespace CocosNet.Particle {


	public class PointParticleSystem : ParticleSystem {
		private int _verticesId;
		private PointSprite[] _vertices;

		protected override void Step(float dt) {
			if (Active && EmissionRate != 0f) {
				float rate = 1f / EmissionRate;
				_emitCounter += dt;
				
				while (!IsFull && _emitCounter > rate) {
					AddParticle();
					_emitCounter -= rate;
				}
				
				_elapsed += dt;
				
				if (Duration != -1 && _elapsed >= Duration) {
					StopSystem();
				}
			}
			
			_particleIndex = 0;
			
			PointF absolutePosition = Position;
			
			while (_particleIndex < ParticleCount) {
				Particle p = _particles[_particleIndex];
				
				if (p.life > 0) {
					PointF tmp = PointF.Empty;
					PointF radial = PointF.Empty;
					PointF tangential = PointF.Empty;
					
					if (p.pos.X != 0 || p.pos.Y != 0) {
						radial = p.pos.NormalizeFast();
					}
					
					tangential = radial;
					
					//radial = radial.Multiply(p.radialAccel);
					radial.X *= p.radialAccel;
					radial.Y *= p.radialAccel;
					
					float newy = tangential.X;
					tangential.X = -tangential.Y;
					tangential.Y = newy;
					//tangential = tangential.Multiply(p.tangentialAccel);
					tangential.X *= p.tangentialAccel;
					tangential.Y *= p.tangentialAccel;
					
					//tmp = radial.Add(tangential).Add(Gravity);
					tmp.X = radial.X + tangential.X + Gravity.X;
					tmp.Y = radial.Y + tangential.Y + Gravity.Y;
					
					//tmp = tmp.Multiply(dt);
					tmp.X *= dt;
					tmp.Y *= dt;
					
					//p.dir = p.dir.Add(tmp);
					p.dir.X += tmp.X;
					p.dir.Y += tmp.Y;
					
					//tmp = p.dir.Multiply(dt);
					tmp.X = p.dir.X * dt;
					tmp.Y = p.dir.Y * dt;
					
					//p.pos = p.pos.Add(tmp);
					p.pos.X += tmp.X;
					p.pos.Y += tmp.Y;
					
					p.color.R += p.deltaColor.R * dt;
					p.color.G += p.deltaColor.G * dt;
					p.color.B += p.deltaColor.B * dt;
					p.color.A += p.deltaColor.A * dt;
					
					p.size += p.deltaSize * dt;
					p.size = Math.Max(0, p.size);
					
					p.life -= dt;
					
					PointF newPos = p.pos;
					
					if (PositionType == PositionType.Free) {
						//newPos = absolutePosition.Subtract(p.startPos);
						newPos.X = absolutePosition.X - p.startPos.X;
						newPos.Y = absolutePosition.Y - p.startPos.Y;
						
						//newPos = p.pos.Subtract(newPos);
						newPos.X = p.pos.X - newPos.X;
						newPos.Y = p.pos.Y - newPos.Y;
					}
					
					_vertices[_particleIndex].Position.X = newPos.X;
					_vertices[_particleIndex].Position.Y = newPos.Y;
					_vertices[_particleIndex].Size = p.size;
					_vertices[_particleIndex].Color = p.color;
					
					++_particleIndex;
				} else {
					// live < 0
					Particle tmp = _particles[_particleIndex];
					_particles[_particleIndex] = _particles[ParticleCount - 1];
					_particles[ParticleCount - 1] = tmp;
					
					ParticleCount -= 1;
					
					if (ParticleCount == 0 && AutoRemoveOnFinish) {
						throw new NotImplementedException("Don't know how to unschedule an AutoRemove particle system");
					}
				}
			}
			
			GL.BindBuffer(All.ArrayBuffer, _verticesId);
			GL.BufferData(All.ArrayBuffer, (IntPtr)(PointSprite.SizeOf * ParticleCount), _vertices, All.DynamicDraw);
			GL.BindBuffer(All.ArrayBuffer, 0);
		}


		public PointParticleSystem(int numberOfParticles) : base(numberOfParticles) {
			_vertices = new PointSprite[TotalParticles];
			
			GL.GenBuffers(1, ref _verticesId);
		}

		public override void Draw() {
			GL.Enable(All.Texture2D);
			GL.BindTexture(All.Texture2D, Texture.Name);
			
			GL.Enable(All.PointSpriteOes);
			GL.TexEnv(All.PointSpriteOes, All.CoordReplaceOes, (int)All.True);
			
			GL.BindBuffer(All.ArrayBuffer, _verticesId);
			GL.EnableClientState(All.VertexArray);
			GL.VertexPointer(2, All.Float, PointSprite.SizeOf, IntPtr.Zero);
			
			GL.EnableClientState(All.ColorArray);
			GL.ColorPointer(4, All.Float, PointSprite.SizeOf, (IntPtr)(PointSprite.ColorOffset));
			
			GL.EnableClientState(All.PointSizeArrayOes);
			GL.Oes.PointSizePointer(All.Float, PointSprite.SizeOf, (IntPtr)(PointSprite.SizeOffset));
			
			
			bool newBlend = false;
			if (BlendAdditive) {
				GL.BlendFunc(All.SrcAlpha, All.One);
			} else if (!BlendFunc.IsDefault) {
				newBlend = true;
				GL.BlendFunc(BlendFunc.Src, BlendFunc.Dst);
			}
			
			GL.DrawArrays(All.Points, 0, _particleIndex);
			
			// restore blend state
			if (BlendAdditive || newBlend) {
				GL.BlendFunc(BlendFunc.DefaultBlendSrc, BlendFunc.DefaultBlendDst);
			}
			
			GL.BindBuffer(All.ArrayBuffer, 0);
			
			GL.DisableClientState(All.PointSizeArrayOes);
			GL.DisableClientState(All.ColorArray);
			GL.DisableClientState(All.VertexArray);
			GL.Disable(All.Texture2D);
			GL.Disable(All.PointSpriteOes);
		}
	}
}
