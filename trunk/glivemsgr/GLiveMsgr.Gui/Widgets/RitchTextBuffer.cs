
using System;
using Gtk;
using System.Collections.Generic;

namespace GLiveMsgr.Gui
{
	
	
	public class RitchTextBuffer : Gtk.TextBuffer
	{
		private TextTag tag_hidden;
		
		public RitchTextBuffer () : base (new TextTagTable ())
		{
			tag_hidden = new TextTag ("tag_hidden");
			tag_hidden.Weight = Pango.Weight.Heavy;
			tag_hidden.Foreground = "#FF0000";
			tag_hidden.Invisible = true;
			
			base.TagTable.Add (tag_hidden);
		}
	}
}
