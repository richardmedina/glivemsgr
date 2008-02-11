// WindowCollection.cs created with MonoDevelop
// User: ricki at 18:41 01/09/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;
using RickiLib.Types;

namespace GLiveMsgr.Gui
{
	
	
	public class WindowCollection : 
		RickiLib.Types.WatchedCollection <Gtk.Window>
	{
		
		public WindowCollection () : base ("Window Collection")
		{
		}
	}
}
