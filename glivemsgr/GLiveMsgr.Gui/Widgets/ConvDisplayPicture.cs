
using System;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public class ConvDisplayPicture : Gtk.Viewport
	{
		private Gtk.VBox vbox;
		private Gtk.HBox hbox;
		private Gtk.EventBox eventbox;
		
		private Gtk.Arrow arrow;
		
		private DisplayPictureWidget displayPicture;
		
		
		public ConvDisplayPicture()
		{
			vbox = new VBox (false, 0);
			vbox.BorderWidth = 5;
			hbox = new HBox (false, 0);
			hbox.BorderWidth = 5;
			
			eventbox = new EventBox ();
			
			displayPicture = new DisplayPictureWidget ();
			displayPicture.ShadowType = ShadowType.EtchedIn;
			
			arrow = new Arrow (ArrowType.Down, ShadowType.EtchedOut);
			
			hbox.PackStart (vbox, true, false, 0);
			
			vbox.PackStart (displayPicture, false, false, 0);
			
			Gtk.HBox arrow_hbox = new HBox (false, 0);
			
			
			arrow_hbox.PackEnd (arrow, false, false, 0);
			vbox.PackStart (arrow_hbox);
			
			eventbox.Add (hbox);
			
			base.Add (eventbox);
			
			base.HeightRequest = displayPicture.Image.Pixbuf.Height + 40;
			
			eventbox.ModifyBg (StateType.Normal,
				Theme.GetGdkColor (System.Drawing.Color.White));
			base.ModifyBg (StateType.Normal, 
				Theme.GetGdkColor (Theme.ConversationBackground));
		}
		
		public DisplayPictureWidget PictureWidget {
			get {
				return displayPicture;
			}
		}
	}
}
