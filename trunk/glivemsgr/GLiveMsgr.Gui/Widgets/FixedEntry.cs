
using System;
using Gtk;
using RickiLib.Widgets;

namespace GLiveMsgr.Gui
{
	
	
	public class FixedEntry : RickiLib.Widgets.SearchEntry
	{
		private Gtk.Entry entry;
		
		public FixedEntry () :
			base (null, null)
		{
		}
		
		public FixedEntry (Gtk.Image image, Gtk.Menu menu) : 
			base (image, menu)
		{
			base.ShadowType = Gtk.ShadowType.In;
		/*
			entry = new Entry ();
			entry.HasFrame = false;
			entry.ModifyBase (StateType.Normal, 
				Theme.GdkColorFromCairo (Theme.BaseColor));
			entry.ModifyText (StateType.Normal,
				Theme.GdkColorFromCairo (Theme.TextColor));
			
			
			base.Add (entry);
		*/
		}
		
		public Gtk.Entry Entry {
			get {
				return base.Entry;
			}
		}
	}
}
