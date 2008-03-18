// ButtonBar.cs created with MonoDevelop
// User: ricki at 01:11 02/25/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public class Buttonbar : Gtk.HBox
	{
		
		public Buttonbar () : base (false, 0)
		{
			
		}
		
		public void Append (ButtonbarItem item)
		{
			PackStart (item, false, false, 0);
		}
	}
}
