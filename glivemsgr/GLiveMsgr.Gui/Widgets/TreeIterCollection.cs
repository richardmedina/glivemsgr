// TreeIterCollection.cs created with MonoDevelop
// User: ricki at 11:24 a 22/10/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using RickiLib.Types;

namespace GLiveMsgr.Gui
{
	
	
	public class TreeIterCollection : 
		RickiLib.Types.WatchedCollection<Gtk.TreeIter>
	{
		
		public new void Add (Gtk.TreeIter iter)
		{
			base.Add (iter);
		}
			
		public new void Remove (Gtk.TreeIter iter)
		{
			base.Remove (iter);
		}
	}
}
