using System;
using System.Drawing;

namespace CocosNet {
	
	/// <summary>
	/// MonoTouch offers no high level debugging, so writing
	/// to console is a common method of debugging. So
	/// adding this class to enable
	/// 
	/// C.W("Hello");
	/// 
	/// instead of
	/// 
	/// Console.WriteLine("Hello");
	/// </summary>
	internal static class C {
		public static void W(string msg) {
			Console.WriteLine(msg);
		}
	}
}
