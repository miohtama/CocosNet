// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using System.Drawing;
using OpenTK.Graphics.ES11;
using System.Runtime.InteropServices;

//
// The types in this file are types that get sent over to OpenGL via 
// methods like GL.VertexPointer, GL.ColorPointer, etc. 
// So it is important that they are defined in a manner that makes finding
// the fields via pointers and offsets possible. So all types in here
// use simple fields instead of properties, and explicit layout
//
// These types in CocosNet have very different naming than their Cocos2D counterparts.
// In Cocos2D they are defined in ccTypes.h, with names like _ccV2F_C4F_T2F_Quad.
//
// An example of usage of these types can be found in TextureAtlas.Draw()
//

namespace CocosNet.Base {
	public class Colors {
		static Colors() {
			Color w = new Color();
			w.R = w.G = w.B = w.A = 255;
			White = w;
			
			Color black = new Color();
			black.R = black.G = black.B = 0;
			black.A = 255;
			Black = black;
		}
		
		public static readonly Color Black;
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

	// Types throughout Cocos, such as CocosNode, have a BlendFunc
	// object that defines how they are blended when rendered. It's
	// a simple coupling of source and destination blending values.
	// It doesn't quite act like the other types in this file, but Cocos2D
	// defines ccBlendFunc in ccTypes.h, so I placed it here too.
	public struct BlendFunc {
		public const All DefaultBlendSrc = All.One;
		public const All DefaultBlendDst = All.OneMinusSrcAlpha;
		public static readonly BlendFunc DefaultBlendFunc = new BlendFunc(DefaultBlendSrc, DefaultBlendDst);
		
		public All Src;
		public All Dst;

		public BlendFunc(All src, All dst) : this() {
			Src = src;
			Dst = dst;
		}
		
		public bool IsDefault {
			get {
				return Src == DefaultBlendSrc && Dst == DefaultBlendDst;
			}
		}
	}		
}
