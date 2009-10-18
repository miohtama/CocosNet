// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using System.Drawing;
using OpenTK.Graphics.ES11;
using System.Runtime.InteropServices;

namespace CocosNet.Base {
	public class Colors {
		static Colors() {
			Color w = new Color();
			w.R = w.G = w.B = w.A = 255;
			White = w;
		}
		
		public static readonly Color White;
	
		public static Color New(byte r, byte g, byte b, byte a) {
			Color c = new Color();
			c.R = r;
			c.G = g;
			c.B = g;
			c.A = a;
			
			return c;
		}
	}
	
	// 4 bytes
	[StructLayout(LayoutKind.Explicit)]
	public struct Color {
		[FieldOffset(0)] public byte R;
		[FieldOffset(1)] public byte G;
		[FieldOffset(2)] public byte B;
		[FieldOffset(3)] public byte A;
	}
	
	// 8 bytes
	[StructLayout(LayoutKind.Explicit)]
	public struct Vertex2F {
		[FieldOffset(0)] public float X;
		[FieldOffset(4)] public float Y;		
	}
	
	//  12 bytes
	[StructLayout(LayoutKind.Explicit)]
	public struct Vertex3F {
		[FieldOffset(0)] public float X;
		[FieldOffset(4)] public float Y;
		[FieldOffset(8)] public float Z;
	}
	
	// 8 bytes
	[StructLayout(LayoutKind.Explicit)]
	public struct Tex2F {
		[FieldOffset(0)] public float U;
		[FieldOffset(4)] public float V;
	}
	
	// 16 bytes
	[StructLayout(LayoutKind.Explicit)]
	public struct PointSprite {
		[FieldOffset(0)] public Vertex2F Position;
		[FieldOffset(8)] public Color Color;
		[FieldOffset(12)] public float Size;
	}
	
	// 32 bytes
	[StructLayout(LayoutKind.Explicit)]
	public struct Quad2 {
		[FieldOffset(0)] public Vertex2F TL;
		[FieldOffset(8)] public Vertex2F TR;
		[FieldOffset(16)] public Vertex2F BL;
		[FieldOffset(24)] public Vertex2F BR;
	}
	
	// 96 bytes
	[StructLayout(LayoutKind.Explicit)]
	public struct Quad3 {
		[FieldOffset(0)] public Vertex3F TL;
		[FieldOffset(24)] public Vertex3F TR;
		[FieldOffset(48)] public Vertex3F BL;
		[FieldOffset(72)] public Vertex3F BR;	
	}
		
	// 20 bytes
	[StructLayout(LayoutKind.Explicit)]
	public struct GLPoint2F {
		[FieldOffset(0)] public Vertex2F Vertex;
		[FieldOffset(8)] public Color Color;
		[FieldOffset(12)] public Tex2F TexCoords;
	}
	
	// 24 bytes
	[StructLayout(LayoutKind.Explicit)]
	public struct GLPoint3F {
		[FieldOffset(0)] public Vertex3F Vertex;
		[FieldOffset(12)] public Color Color;
		[FieldOffset(16)] public Tex2F TexCoords;
	}
		
	// 80 bytes
	[StructLayout(LayoutKind.Explicit)]
	public struct GLPointQuad2F {
		[FieldOffset(0)] public GLPoint2F TL;
		[FieldOffset(20)] public GLPoint2F TR;
		[FieldOffset(40)] public GLPoint2F BL;
		[FieldOffset(60)] public GLPoint2F BR;
	}
	
	// 96 bytes
	[StructLayout(LayoutKind.Explicit)]
	public struct GLPointQuad3F {
		[FieldOffset(0)] public GLPoint3F TL;
		[FieldOffset(24)] public GLPoint3F TR;
		[FieldOffset(48)] public GLPoint3F BL;
		[FieldOffset(72)] public GLPoint3F BR;
	}

	public struct BlendFunc {
		public All Src;
		public All Dst;

		public BlendFunc(All src, All dst) : this() {
			Src = src;
			Dst = dst;
		}
	}		
	
//	internal static class QuadHelper {
//		public static void SetGLPointQuad3F_TL_TexCoords_U(ref GLPointQuad3F quad, float value) {
//			Tex2F t = quad.TL.TexCoords;
//			t.U = value;
//			quad.TL.TexCoords = t;
//			quad.TL.TexCoords.U = 34;
//		}
//		
//		public static void SetGLPointQuad3F_TL_TexCoords_V(ref GLPointQuad3F quad, float value) {
//			
//		}
//		
//		public static void SetGLPointQuad3F_TR_TexCoords_U(ref GLPointQuad3F quad, float value) {
//			
//		}
//		
//		public static void SetGLPointQuad3F_TR_TexCoords_V(ref GLPointQuad3F quad, float value) {
//			
//		}
//		
//		public static void SetGLPointQuad3F_BL_TexCoords_U(ref GLPointQuad3F quad, float value) {
//			
//		}
//		
//		public static void SetGLPointQuad3F_BL_TexCoords_V(ref GLPointQuad3F quad, float value) {
//			
//		}
//		
//		public static void SetGLPointQuad3F_BR_TexCoords_U(ref GLPointQuad3F quad, float value) {
//			
//		}
//		
//		public static void SetGLPointQuad3F_BR_TexCoords_V(ref GLPointQuad3F quad, float value) {
//			
//		}		
//		
//		public static void SetGLPointQuad3F_TL_Vertex_X(ref GLPointQuad3F quad, float value) {
//		}
//		
//		public static void SetGLPointQuad3F_TL_Vertex_Y(ref GLPointQuad3F quad, float value) {
//		}
//		
//		public static void SetGLPointQuad3F_TL_Vertex_Z(ref GLPointQuad3F quad, float value) {
//		}		
//		
//		public static void SetGLPointQuad3F_TR_Vertex_X(ref GLPointQuad3F quad, float value) {
//		}
//		
//		public static void SetGLPointQuad3F_TR_Vertex_Y(ref GLPointQuad3F quad, float value) {
//		}
//		
//		public static void SetGLPointQuad3F_TR_Vertex_Z(ref GLPointQuad3F quad, float value) {
//		}		
//		
//		public static void SetGLPointQuad3F_BL_Vertex_X(ref GLPointQuad3F quad, float value) {
//		}
//		
//		public static void SetGLPointQuad3F_BL_Vertex_Y(ref GLPointQuad3F quad, float value) {
//		}
//		
//		public static void SetGLPointQuad3F_BL_Vertex_Z(ref GLPointQuad3F quad, float value) {
//		}				
//		
//		public static void SetGLPointQuad3F_BR_Vertex_X(ref GLPointQuad3F quad, float value) {
//		}
//		
//		public static void SetGLPointQuad3F_BR_Vertex_Y(ref GLPointQuad3F quad, float value) {
//		}
//		
//		public static void SetGLPointQuad3F_BR_Vertex_Z(ref GLPointQuad3F quad, float value) {
//		}			
//	}
}
