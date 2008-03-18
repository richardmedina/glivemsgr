
using System;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public class DisplayPictureWidget : Gtk.Frame
	{
		private Gtk.Image image;
		
		public DisplayPictureWidget () : this (null)
		{
		}
		
		public DisplayPictureWidget (Gdk.Pixbuf pixbuf)
		{
			base.ShadowType = ShadowType.EtchedOut;
			
			if (pixbuf == null)
				pixbuf = Gdk.Pixbuf.LoadFromResource (
					"user_display_picture_default.png"
				);
			
			image = new Gtk.Image (pixbuf);
			//ConversationBackground
			base.ModifyBg (StateType.Normal,
				Theme.GdkColorFromCairo (Theme.BgColor));
			
			base.Add (image);
		}
		
		public Gtk.Image Image {
			get {
				return image;
			}
		}
	}
}
