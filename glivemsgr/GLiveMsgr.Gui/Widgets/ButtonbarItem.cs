// ButtonbarItem.cs created with MonoDevelop
// User: ricki at 02:05 02/25/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public class ButtonbarItem : Gtk.ToggleButton
	{
		
		private Gtk.Menu _menu = null;
		
		public ButtonbarItem (string text)
		{
			Relief = ReliefStyle.None;
			
			Child = new Label (text);
		}
		
		public void Popup ()
		{
			if (_menu != null)
				_menu.Popup (null, 
					null, 
					menuPositionFunc, 
					IntPtr.Zero, 
					1, 0);
		}
		
		private void menuPositionFunc (Gtk.Menu menu, 
			out int x, out int y, 
			out bool pushin)
		{
			x = 0;
			y = 0;
			pushin = false;
			
			
			if (_menu != null) {
				Gdk.ModifierType mask;
				GdkWindow.GetRootOrigin (out x, out y);
				x += Allocation.X;
				y += Allocation.Height;
				pushin = true;
			}
		}
		
		protected override void OnToggled ()
		{
			if (Active)
				Popup ();
			
			base.OnToggled ();
		}
		
		public Gtk.Menu Menu {
			get { return _menu; }
			set { 
				_menu = value;
				_menu.Hidden += delegate {Active = false; };
			}
		}

	}
}
