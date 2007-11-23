// NotificationIcon.cs created with MonoDevelop
// User: ricki at 13:45 11/12/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public class NotificationIcon : GLiveMsgr.Gui.TrayIcon
	{
		private MainWindow window;
		
		private Gtk.EventBox eventBox;
		
		private NotificationMenu menu;
		
		public NotificationIcon (MainWindow window) : base ("GNOME Live Messenger")
		{
			this.window = window;
			
			eventBox = new Gtk.EventBox ();
			
			eventBox.Add (new Image (Stock.GoUp, IconSize.Button));
			eventBox.ButtonPressEvent += eventBox_ButtonPressEvent;
			
			menu = new NotificationMenu ();
			menu.ItemShowHide.Activated += delegate { WindowShowHide (); };
			menu.ItemQuit.Activated += menu_ItemQuit_Activated;
			menu.ShowAll ();
			
			base.Add (eventBox);
		}
		private void eventBox_ButtonPressEvent (object sender, ButtonPressEventArgs args)
		{
			if (args.Event.Button == 3)
				menu.Popup ();
			else
				WindowShowHide ();
		}
		
		private void menu_ItemQuit_Activated (object sender, EventArgs args)
		{
			window.Quit ();
		}
		
		public void WindowShowHide ()
		{
			if (window.Visible)
				window.Hide ();
			else {
				window.Show ();
			//	window.GdkWindow.Raise ();
			}
		}
		
		private class NotificationMenu : Gtk.Menu 
		{
			private Gtk.ImageMenuItem itemShowHide;
			private Gtk.ImageMenuItem itemPrefs;
			private Gtk.ImageMenuItem itemAbout;
			private Gtk.ImageMenuItem itemQuit;
			
			public NotificationMenu ()
			{
				itemShowHide = new ImageMenuItem ("Mostrar/Ocultar ventana principal");
				itemPrefs = new ImageMenuItem (Stock.Preferences, null);
				itemAbout = new ImageMenuItem (Stock.About, null);
				itemAbout.Activated += itemAbout_Activated;
				itemQuit = new ImageMenuItem (Stock.Quit, null);
				
				base.Append (itemShowHide);
				base.Append (new Gtk.SeparatorMenuItem ());
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
			
			public ImageMenuItem ItemQuit {
				get { return itemQuit; }
			}
		}
	}
}
