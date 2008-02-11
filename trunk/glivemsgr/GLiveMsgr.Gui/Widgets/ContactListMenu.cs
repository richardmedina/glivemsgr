// ContactListMenu.cs created with MonoDevelop
// User: ricki at 22:54 01/19/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public class ContactListMenu : Gtk.Menu
	{
		
		private Gtk.MenuItem itemOpenConv;
		private Gtk.MenuItem itemOpenBetaConv;
		
		public ContactListMenu ()
		{
			itemOpenConv = new Gtk.MenuItem ("_Open Conversation");
			itemOpenBetaConv = new Gtk.MenuItem ("Open Beta Conversation");
			
			base.Append (itemOpenConv);
			base.Append (itemOpenBetaConv);
			base.Append (new SeparatorMenuItem ());
			base.Append (new MenuItem ("About.."));
			
			base.ShowAll ();
		}
		
		public Gtk.MenuItem ItemOpenConv {
			get { return itemOpenConv; }
		}
		
		public Gtk.MenuItem ItemOpenBetaConv {
			get { return itemOpenBetaConv; }
		}
	}
}