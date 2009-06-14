
using System;
using Gtk;

namespace MsnpTesting.Widgets
{
	
	
	public class MainWindow : Gtk.Window
	{
		private Gtk.VBox _vbox;
		private Gtk.HBox _hbox;
		
		private Gtk.Entry _entry_username;
		private Gtk.Entry _entry_password;
		
		
		public MainWindow () : base (string.Empty)
		{
			_vbox = new VBox (false, 0);
			_hbox = new HBox (false, 0);
			
			_entry_username = new Entry ();
			_entry_password = new Entry ();
			_entry_password.Visibility = false;
			
			_hbox.PackStart (new Label ("username"), false, false, 0);
			_hbox.PackStart (_entry_username);
			
			//_vbox.PackStart (_hbox);
			
			//_hbox = new HBox (false, 0);
			
			_hbox.PackStart (new Label ("Password"), false, false, 0);
			_hbox.PackStart (_entry_password);
			
			_vbox.PackStart (_hbox, false, false, 0);
			Add (_vbox);
		}
	}
}
