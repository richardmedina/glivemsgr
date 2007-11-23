
using System;
using Gtk;
using RickiLib.Widgets;

namespace GLiveMsgr.Gui
{
	
	
	public class RitchAnchorFileExchange : IRitchAnchor
	{
		private Gtk.TextChildAnchor anchor;
		private Gtk.TextMark mark;
		private Gtk.TextIter iter;
		private Gtk.EventBox eventbox;
		
		private Gtk.Notebook notebook;
		
//		private string filename;
//		private long filesize;
//		private Gdk.Pixbuf pixbufPreview;
		
		public RitchAnchorFileExchange()
		{
			eventbox = new EventBox ();
			notebook = new Notebook ();
			notebook.ShowTabs = false;
			
			AppendPage1 (notebook);
			
			eventbox.Add (notebook);
		}
		
		private void AppendPage1 (Notebook notebook)
		{
			Gtk.HBox hbox = new HBox (false, 0);
			Gtk.VBox vbox = new VBox (false, 0);
			
			vbox.PackStart (
				Factory.Label ("<b>File sending request. You want to accept?</b>"),
				false, false, 0);
			
			hbox.PackStart (vbox, false, false, 0);
			
			notebook.AppendPage (hbox, new Label ());
			
		}
				
		public Gtk.TextChildAnchor Anchor {
			get { return anchor; }
			set { anchor = value; }
		}
		
		public Gtk.TextMark Mark {
			get { return mark; }
			set { mark = value; }
		}
		
		public Gtk.TextIter Iter {
			get { return iter; }
			set { iter = value; }
		}
		
		public Gtk.Widget Widget {
			get { return eventbox; }
		}
		
		public RitchAnchorType Type {
			get { return RitchAnchorType.FileExchange; }
		}
	}
}
