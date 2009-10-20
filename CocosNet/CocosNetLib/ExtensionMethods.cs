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

namespace CocosNet {
	public static class ExtensionMethods {
		public static float ToRadians(this float degrees) {
			return degrees * (float)Math.PI / 180.0f;
		}
		
		public static float ToDegrees(this float radians) {
			return radians * 180.0f / (float)Math.PI;
		}
		
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
