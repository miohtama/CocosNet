
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
		
		public static void Each<T>(this List<T> list, Action<T> action) {
			if (list == null) {
				throw new ArgumentNullException("list");
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
		
		public static SizeF SizeWithFont(this string str, UIFont font) {
			NSString nsstring = new NSString(str);
			Selector selector = new Selector("sizeWithFont:");
			
			return cgsize_objc_msgSend_IntPtr_IntPtr(nsstring.Handle, selector.Handle, font.Handle);
		}
		
		[DllImport(MonoTouch.Constants.ObjectiveCLibrary, EntryPoint = "objc_msgSend")]
		private static extern void void_objc_msgSend_IntPtr_RectangleF_IntPtr_UILineBreakMode_UITextAlignment(IntPtr target, IntPtr selector, RectangleF rect, IntPtr font, UILineBreakMode lineBreak, UITextAlignment alignment);
		public static void DrawInRect(this string str, RectangleF rect, UIFont font, UILineBreakMode lineBreakMode, UITextAlignment alignment) {
			NSString nsstring = new NSString(str);
			Selector selector = new Selector("drawInRect:withFont:lineBreakMode:alignment:");
			
			void_objc_msgSend_IntPtr_RectangleF_IntPtr_UILineBreakMode_UITextAlignment(nsstring.Handle, selector.Handle, rect, font.Handle, lineBreakMode, alignment);
		}
	}
}
