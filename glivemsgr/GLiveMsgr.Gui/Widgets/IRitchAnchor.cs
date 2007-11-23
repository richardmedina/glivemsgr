
using System;

namespace GLiveMsgr.Gui
{
	
	
	public interface IRitchAnchor
	{
		Gtk.TextChildAnchor Anchor { get; set; }
		Gtk.TextMark Mark { get; set; }
		Gtk.TextIter Iter { get;}
		Gtk.Widget Widget { get; }
		RitchAnchorType Type { get; }
	}
}
