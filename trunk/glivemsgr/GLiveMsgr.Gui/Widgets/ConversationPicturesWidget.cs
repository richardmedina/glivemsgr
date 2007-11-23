
using System;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public class ConversationPicturesWidget : Gtk.EventBox
	{
		private Gtk.VBox vbox;
		
		private ConvDisplayPicture myDisplay;
		private ConvDisplayPicture contactDisplay;
		
		public ConversationPicturesWidget()
		{
			vbox = new VBox (false, 0);
			
			myDisplay = new ConvDisplayPicture ();
			contactDisplay = new ConvDisplayPicture ();
			
			vbox.PackStart (myDisplay, false, false, 0);
			vbox.PackEnd (contactDisplay, false, false, 0);
			
			base.Add (vbox);
			
			base.ModifyBg (StateType.Normal,
				Theme.GetGdkColor (Theme.ConversationBackground));
		}
	}
}
