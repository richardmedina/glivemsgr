
using System;
using Gtk;

using System.Net.Protocols;
using System.Net.Protocols.Msnp;

namespace GLiveMsgr.Gui
{
	
	
	public class ConversationWidget : Gtk.VBox
	{
		private Gtk.HBox hbox;
		private Gtk.VBox vbox;
		
		private ConversationHeaderWidget header;
		//private ConversationToolbar toolbar;
		
		private ConversationChatWidget chatWidget;
		private ConversationPicturesWidget displayPictures;
		
		private ArrowButton _arrowButton;
		
		private ConversationStatusbar statusbar;
		
		private MsnpConversation _conversation;
		
		private int messageTime = -1;
		
		public ConversationWidget (MsnpConversation conv)
		{
			_conversation = conv;
			header = new ConversationHeaderWidget (
				_conversation.Account.Username,
				_conversation.Account.Alias);
				
			//toolbar = new ConversationToolbar ();
			chatWidget = new ConversationChatWidget (_conversation);
			displayPictures = new ConversationPicturesWidget ();
			_arrowButton = new ArrowButton ();
			_arrowButton.ArrowType = ArrowType.Right;
			_arrowButton.Clicked += arrowButton_Clicked;
			statusbar = new ConversationStatusbar ();
			
			hbox = new HBox (false, 0);
			vbox = new VBox (false, 0);
			
			//vbox.PackStart (chatWidget);
			
			hbox.PackStart (chatWidget);
			hbox.PackStart (displayPictures, false, false, 5);
			
			Gtk.VBox vbox2 = new VBox (false, 0);
			vbox2.PackStart (_arrowButton, false, false, 0);
			hbox.PackStart (vbox2, false, false, 0);
			
			base.ModifyBg (StateType.Normal, 
				Theme.GdkColorFromCairo (Theme.BgColor));
				
			//vbox.PackStart (header, false, false, 0);
			//vbox.PackStart (toolbar, false, false, 0);
			
			vbox.PackStart (hbox);
			vbox.PackEnd (statusbar, false, false, 0);

			_conversation.Typing += conversation_Typing;
			base.PackStart (vbox);
		}
		
		private void conversation_Typing (object sender, TypingArgs args)
		{
			RickiLib.Widgets.Utils.RunOnGtkThread (delegate {
				if (messageTime < 0) {
					statusbar.Push (1, 
						string.Format ("{0} writing message",
							args.Buddy.Alias));
					messageTime = 2;
					GLib.Timeout.Add (1000, dec_messageTime);
				}
			});
		}
		
		private bool dec_messageTime ()
		{
			if (messageTime -- == 0) {
				statusbar.Pop (1);
				return false;
			}
			
			return true;
		}
		
		
		private void conversation_Started (object sender,EventArgs args)
		{
			Console.WriteLine ("ConversationStarted");
			RickiLib.Widgets.Utils.RunOnGtkThread (delegate {
				this.statusbar.Push (1, "Connected");
			});
		}
		
		private void arrowButton_Clicked (object sender, EventArgs args)
		{
			if (_arrowButton.ArrowType == ArrowType.Left) {
				displayPictures.Show ();
				_arrowButton.ArrowType = ArrowType.Right;
			}
			else {
				displayPictures.Hide ();
				_arrowButton.ArrowType = ArrowType.Left;
			}
		}
		
		public ConversationChatWidget ChatWidget {
			get { return this.chatWidget; }
		}
	}
}
