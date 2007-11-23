
using System;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public class FixedEntry : Gtk.Frame
	{
		private Gtk.Entry entry;
		
		public FixedEntry()
		{
			base.ShadowType = Gtk.ShadowType.In;
		
			entry = new Entry ();
			entry.HasFrame = false;
			entry.ModifyBase (StateType.Normal, 
				Theme.GdkColorFromCairo (Theme.BaseColor));
			entry.ModifyText (StateType.Normal,
				Theme.GdkColorFromCairo (Theme.TextColor));
			
			
			base.Add (entry);
		}
		
		public Gtk.Entry Entry {
			get {
				return entry;
			}
		}
	}
}
