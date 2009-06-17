
using System;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	// INFO. This widget can be extended..
	public class LogonHeaderWidget : Gtk.VBox
	{
		private Gtk.HBox _hbox;
		
		public LogonHeaderWidget ()
		{
			//AddEvents ((int) Gdk.EventMask.ButtonPressMask);
						
			HeightRequest = 50;
		}		
	}
}
