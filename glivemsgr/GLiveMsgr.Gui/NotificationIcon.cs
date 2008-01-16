// NotificationIcon.cs created with MonoDevelop
// User: ricki at 13:45 11/12/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public class NotificationIcon : Gtk.StatusIcon
	{
		private MainWindow window;
		
		private NotificationMenu menu;
		
		public NotificationIcon (MainWindow window)
		{
			this.window = window;
			this.Stock = Gtk.Stock.GoUp; 
						
			menu = new NotificationMenu ();
			menu.ItemShowHide.Activated += delegate { WindowShowHide (); };
			menu.ItemQuit.Activated += menu_ItemQuit_Activated;
			menu.ShowAll ();
		}
		
		protected override void OnPopupMenu (uint button, uint activate_time)
		{
			if (button == 3)
				menu.Popup ();
			base.OnPopupMenu (button, activate_time);
		}

		
		protected override void OnActivate ()
		{
			WindowShowHide ();
			base.OnActivate ();
		}
		
		private void menu_ItemQuit_Activated (object sender, EventArgs args)
		{
			window.Quit ();
		}
		
		public void WindowShowHide ()
		{
			if (window.Visible) {
				window.Iconify ();
				window.Hide ();
			}
			else {
				if ((window.GdkWindow.State & Gdk.WindowState.Iconified) > 0)
					window.Deiconify ();
				window.Show ();
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
				itemPrefs = new ImageMenuItem (Gtk.Stock.Preferences, null);
				itemAbout = new ImageMenuItem (Gtk.Stock.About, null);
				itemAbout.Activated += itemAbout_Activated;
				itemQuit = new ImageMenuItem (Gtk.Stock.Quit, null);
				
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
