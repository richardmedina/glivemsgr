
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
			
			base.ModifyBg (StateType.Normal,
				Theme.GetGdkColor (Theme.ConversationBackground));
			
			base.Add (image);
		}
		
		public Gtk.Image Image {
			get {
				return image;
			}
		}
	}
}
