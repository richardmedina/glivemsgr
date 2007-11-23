
using System;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public class SimpleContactList : Gtk.TreeView
	{
		private Gtk.TreeModel model;
		
		public SimpleContactList()
		{
			model = new TreeStore (typeof (string), 
				typeof (string));
			
			base.Model = model;
			
			base.AppendColumn (
				"Contact", 
				new CellRendererText (),
				"text", 
				0);
		}
		
	//	public void Add (MsnContact contact)
	//	{
			
	//	}
		
		
	}
}
