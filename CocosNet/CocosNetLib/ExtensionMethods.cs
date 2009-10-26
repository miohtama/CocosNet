// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using MonoTouch.UIKit;
using System;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Collections;
using CocosNet.Base;

namespace CocosNet {
	public static class ExtensionMethods {
		#region PointF extensions
		
		public static PointF Negate(this PointF p) {
			return new PointF(-p.X, -p.Y);
		}
		
		public static PointF Subtract(this PointF p, PointF other) {
			p.X -= other.X;
			p.Y -= other.Y;
			return p;
		}
		
		public static PointF Add(this PointF p, PointF other) {
			p.X += other.X;
			p.Y += other.Y;
			return p;
		}		
		
		public static PointF Multiply(this PointF p, float multiplier) {
			return new PointF(p.X * multiplier, p.Y * multiplier);
		}
		
		public static float Dot(this PointF p, PointF other) {
			return p.X * other.X + p.Y * other.Y;	
		}
		
		public static float Length(this PointF p) {
			return (float)Math.Sqrt(p.Dot(p));
		}
		
		public static PointF Normalize(this PointF p) {
			return p.Multiply(1.0f / p.Length());	
		}
		
		public static Vertex2F ToVertex2F(this PointF p) {
			Vertex2F v = new Vertex2F();
			v.X = p.X;
			v.Y = p.Y;
			
			return v;
		}
	
		#endregion
		
		#region numeric extensions
		
		public static bool IsOdd(this int number) {
			return (number & 1) == 1;
		}
		
		public static float ToRadians(this float degrees) {
			return degrees * (float)Math.PI / 180.0f;
		}
		
		public static float ToDegrees(this float radians) {
			return radians * 180.0f / (float)Math.PI;
		}
		
		#endregion numeric extensions
		
		#region list extensions
		
		public static void Each<T>(this List<T> list, Action<T> action) {
			if (list == null) {
				throw new ArgumentNullException("list");
			}
			if (action == null) {
				
				throw new ArgumentNullException("action");
			}
			
			foreach (T t in list) {
				action(t);
			}
		}
		
		public static bool IsEmpty(this ICollection col) {
			if (col == null) {
				throw new ArgumentNullException("col");
			}
			
			return col.Count == 0;
		}
		
		#endregion list extension
		
		[DllImport(MonoTouch.Constants.ObjectiveCLibrary, EntryPoint="objc_msgSend")]
		private static extern SizeF cgsize_objc_msgSend_IntPtr_IntPtr(IntPtr target, IntPtr selector, IntPtr font);
		[DllImport(MonoTouch.Constants.ObjectiveCLibrary, EntryPoint="objc_msgSend_stret")]
		private static extern void void_objc_msgSend_stret_SizeF_IntPtr_IntPtr(out SizeF size, IntPtr target, IntPtr selector, IntPtr font);
		
		// FIXME: Use UIView.StringSize instead
		public static SizeF SizeWithFont(this string str, UIFont font) {
			NSString nsstring = new NSString(str);
			Selector selector = new Selector("sizeWithFont:");
			SizeF size;
			
			if (Runtime.Arch == Arch.DEVICE) {
				void_objc_msgSend_stret_SizeF_IntPtr_IntPtr (out size, nsstring.Handle, selector.Handle, font.Handle);
			} else {
				size = cgsize_objc_msgSend_IntPtr_IntPtr(nsstring.Handle, selector.Handle, font.Handle);
			}
			return size;
		}
		
		[DllImport(MonoTouch.Constants.ObjectiveCLibrary, EntryPoint = "objc_msgSend")]
		private static extern SizeF SizeF_objc_msgSend_IntPtr_RectangleF_IntPtr_UILineBreakMode_UITextAlignment(IntPtr target, IntPtr selector, RectangleF rect, IntPtr font, UILineBreakMode lineBreak, UITextAlignment alignment);
		[DllImport(MonoTouch.Constants.ObjectiveCLibrary, EntryPoint = "objc_msgSend_stret")]
		private static extern void void_objc_msgSend_stret_SizeF_IntPtr_RectangleF_IntPtr_UILineBreakMode_UITextAlignment(out SizeF size, IntPtr target, IntPtr selector, RectangleF rect, IntPtr font, UILineBreakMode lineBreak, UITextAlignment alignment);
		
		// FIXME: Use UIView.DrawString
		public static void DrawInRect(this string str, RectangleF rect, UIFont font, UILineBreakMode lineBreakMode, UITextAlignment alignment) {
			NSString nsstring = new NSString(str);
			Selector selector = new Selector("drawInRect:withFont:lineBreakMode:alignment:");
			SizeF size;
			
			if (Runtime.Arch == Arch.DEVICE) {
				void_objc_msgSend_stret_SizeF_IntPtr_RectangleF_IntPtr_UILineBreakMode_UITextAlignment(out size, nsstring.Handle, selector.Handle, rect, font.Handle, lineBreakMode, alignment);
			} else {
				size = SizeF_objc_msgSend_IntPtr_RectangleF_IntPtr_UILineBreakMode_UITextAlignment(nsstring.Handle, selector.Handle, rect, font.Handle, lineBreakMode, alignment);
			}
		}
	}
}
