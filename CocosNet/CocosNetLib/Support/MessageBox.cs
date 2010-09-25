using System;
using MonoTouch.UIKit;

namespace CocosNet.Support {
	
	public static class MessageBox {

		public static void Show(string title, string message) {
			using (UIAlertView alert = new UIAlertView()) {
				alert.Title = title;
				alert.Message = message;
				
				alert.AddButton("OK");
				
				alert.Show();
			}
		}
	}
}

