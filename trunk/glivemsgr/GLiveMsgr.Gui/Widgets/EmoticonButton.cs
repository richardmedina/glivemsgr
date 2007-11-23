
using System;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public class EmoticonButton : Gtk.Button
	{
		private Gtk.Image image;
		
		private Emoticon emoticon;
		
		private static Gtk.Tooltips tips = new Tooltips ();
		
		public EmoticonButton (Emoticon emoticon)
		{
			this.emoticon = emoticon;
			Gdk.Pixbuf pixbuf = new Gdk.Pixbuf (emoticon.Filename);
			
			pixbuf = pixbuf.ScaleSimple (
				17, 17, Gdk.InterpType.Bilinear);
			
			this.image = new Image (pixbuf);
			this.Child = image;
			this.Relief = ReliefStyle.None;
			
			tips.SetTip (this, emoticon.Trigger, emoticon.Trigger);
		}
		
		
		public new Gtk.Image Image {
			get { return image; }
		}
		
		public Emoticon Emoticon {
			get { return emoticon; }
		}
	}
}