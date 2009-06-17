// GLiveMsgrAboutDialog.cs created with MonoDevelop
// User: ricki at 14:10 11/20/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public class GLiveMsgrAboutDialog : Gtk.AboutDialog
	{
		
		public GLiveMsgrAboutDialog ()
		{
			base.Authors = new string [] {
				"Ricardo Medina Lopez <ricki@dana-ide.org>"
			};
			
			base.Name ="GNOME Live Messenger";
			base.Version = "0.1a";
			base.Website = "http://messenger.dana-ide.org";
			base.Comments = "GNOME Live Messenger is a simple MSNP " +
				"Network client. Is my favorite hobbie since " +
				"Santo and Blue Daemon Figthers :-).\n\n" +
				"Enjoy it!";
			
			base.License = "GNU/GPL";
			
			//base.LogoIconName = Stock.GoUp;
			base.Logo = Gdk.Pixbuf.LoadFromResource ("messenger.png");
		}
	}
}
