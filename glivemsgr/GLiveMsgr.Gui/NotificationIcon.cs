// NotificationIcon.cs created with MonoDevelop
// User: ricki at 13:45 11/12/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Net.Protocols.Msnp;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public class NotificationIcon : Gtk.StatusIcon
	{
		private MainWindow _window;
		
		private NotificationMenu _menu;
		
		private int _window_x;
		private int _window_y;
		private int _window_width;
		private int _window_height;
		
		public NotificationIcon (MainWindow window)
		{
			_window = window;
			_window.Account.Started += account_Started;
			_window.Account.Terminated += account_Terminated;
			
			Stock = Gtk.Stock.GoUp;
			 
						
			_menu = new NotificationMenu ();
			_menu.ItemShowHide.Activated += delegate { WindowShowHide (); };
			_menu.ItemDisconnect.Activated += itemDisconnect_Activated;
			_menu.ItemQuit.Activated += menu_ItemQuit_Activated;
			_menu.ShowAll ();
		}

		public void WindowShowHide ()
		{
			if (_window.Visible) {
				pushWindowGeometry ();
				_window.Hide ();
			}
			else {
				_window.Present ();
				popWindowGeometry ();
			}
		}
		
		protected override void OnPopupMenu (uint button, uint activate_time)
		{
			if (button == 3) {
				_menu.Popup ();
			}
			base.OnPopupMenu (button, activate_time);
		}

		
		protected override void OnActivate ()
		{
			WindowShowHide ();
			base.OnActivate ();
		}
		
		private void account_Terminated (object sender, EventArgs args)
		{
			_menu.ItemDisconnect.Sensitive = false;
		}
		
		private void account_Started (object sender, EventArgs args)
		{
			_menu.ItemDisconnect.Sensitive = true;
		}
		
		private void itemDisconnect_Activated (object sender, EventArgs args)
		{
			if (_window.Account.Logged)
				_window.Account.Logout ();
		}
		
		private void menu_ItemQuit_Activated (object sender, EventArgs args)
		{
			_window.Quit ();
		}
		
		private void pushWindowGeometry ()
		{
			_window.GetPosition (out _window_x, out _window_y);
			_window.GetSize (out _window_width, out _window_height);
		}
		
		private void popWindowGeometry ()
		{
			_window.GdkWindow.MoveResize (
				_window_x,
				_window_y,
				_window_width,
				_window_height);
		}
		
		private class NotificationMenu : Gtk.Menu 
		{
			private Gtk.ImageMenuItem itemShowHide;
			private Gtk.ImageMenuItem itemDisconnect;
			private Gtk.ImageMenuItem itemPrefs;
			private Gtk.ImageMenuItem itemAbout;
			private Gtk.ImageMenuItem itemQuit;
			
			public NotificationMenu ()
			{
				itemShowHide = new ImageMenuItem ("Mostrar/Ocultar ventana principal");
				itemDisconnect = new ImageMenuItem (Gtk.Stock.Disconnect, null);
				itemDisconnect.Sensitive = false;
				itemPrefs = new ImageMenuItem (Gtk.Stock.Preferences, null);
				itemAbout = new ImageMenuItem (Gtk.Stock.About, null);
				itemAbout.Activated += itemAbout_Activated;
				itemQuit = new ImageMenuItem (Gtk.Stock.Quit, null);
				
				base.Append (itemShowHide);
				base.Append (new Gtk.SeparatorMenuItem ());
				base.Append (itemDisconnect);
				base.Append (itemPrefs);
				base.Append (itemAbout);
				base.Append (new Gtk.SeparatorMenuItem ());
				base.Append (itemQuit);
			}
			
			private void itemAbout_Activated (object sender, EventArgs args)
			{
				GLiveMsgrAboutDialog dialog = 
					new GLiveMsgr.Gui.GLiveMsgrAboutDialog ();
								
				dialog.Run ();
				dialog.Destroy ();
			}
			
			public ImageMenuItem ItemShowHide {
				get { return itemShowHide; }
			}
			
			public ImageMenuItem ItemDisconnect {
				get { return itemDisconnect; }
			}
			
			public ImageMenuItem ItemQuit {
				get { return itemQuit; }
			}
		}
	}
}
