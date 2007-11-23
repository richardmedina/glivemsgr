
using System;
using Gtk;

using System.Net.Protocols.Msnp;

namespace GLiveMsgr.Gui
{
	
	
	public class ConversationWidget : Gtk.EventBox
	{
		private Gtk.HBox hbox;
		private Gtk.VBox vbox;
		
		private ConversationHeaderWidget header;
		private ConversationToolbar toolbar;
		
		private ConversationChatWidget chatWidget;
		private ConversationPicturesWidget displayPictures;
		
		private HidePicturesPanel hidePictures;
		
		private MsnpConversation conversation;
		
		public ConversationWidget (MsnpConversation conv)
		{
			this.conversation = conv;
			header = new ConversationHeaderWidget (conversation);
			toolbar = new ConversationToolbar ();
			chatWidget = new ConversationChatWidget (conversation);
			displayPictures = new ConversationPicturesWidget ();
			hidePictures = new HidePicturesPanel ();
			hidePictures.Clicked += hidePictures_Clicked;
			
			hbox = new HBox (false, 0);
			vbox = new VBox (false, 0);
			
			//vbox.PackStart (chatWidget);
			
			hbox.PackStart (chatWidget);
			hbox.PackStart (displayPictures, false, false, 5);
			hbox.PackStart (hidePictures, false, false, 0);
			
			base.ModifyBg (StateType.Normal, 
				Theme.GdkColorFromCairo (Theme.SelectedBgColor));
				
			vbox.PackStart (header, false, false, 0);
			vbox.PackStart (toolbar, false, false, 0);
			
			vbox.PackStart (hbox);
			
			base.Add (vbox);
		}
		
		private void hidePictures_Clicked (object sender, EventArgs args)
		{
			if (hidePictures.IsLeftArrow)
				displayPictures.Hide ();
			else
				displayPictures.Show ();
		}
		
		public ConversationChatWidget ChatWidget {
			get { return this.chatWidget; }
		}
	}
}
