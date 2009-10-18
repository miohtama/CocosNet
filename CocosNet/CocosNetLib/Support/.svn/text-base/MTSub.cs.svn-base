using System;
using cg=MonoTouch.CoreGraphics;
using System.Runtime.InteropServices;

namespace CocosNet.Support {
	public static class MTSub {

		public static class CGAffineTransform {
			
			[DllImport(MonoTouch.Constants.CoreGraphicsLibrary, EntryPoint = "CGAffineTransformConcat")]
			private static extern cg.CGAffineTransform __cgaffinetransformconcat(cg.CGAffineTransform transform, cg.CGAffineTransform other);
			public static cg.CGAffineTransform Concat(cg.CGAffineTransform transform, cg.CGAffineTransform other) {
				return __cgaffinetransformconcat(transform, other);
			}
			
			[DllImport(MonoTouch.Constants.CoreGraphicsLibrary, EntryPoint = "CGAffineTransformInvert")]
			private static extern cg.CGAffineTransform __cgaffinetransforminvert(cg.CGAffineTransform transform);
			public static cg.CGAffineTransform Invert(cg.CGAffineTransform transform) {
				return __cgaffinetransforminvert(transform);
			}
		}
	}
}
