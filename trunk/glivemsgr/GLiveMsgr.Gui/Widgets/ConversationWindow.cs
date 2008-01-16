
using System;
using System.Threading;
using RickiLib.Types;
using RickiLib.Widgets;
using Gtk;

using System.Net.Protocols;
using System.Net.Protocols.Msnp;

namespace GLiveMsgr.Gui
{
	
	public class ConversationWindow : CustomWindow
	{
		private MsnpConversation conversation;

		private ConversationMenu menu;
		private ConversationWidget widget;
		private Gtk.Statusbar statusbar;
		
		
		
		public ConversationWindow (MsnpConversation conv)
		{
			this.conversation = conv;
			this.conversation.Closed += conversation_Closed;
			
			this.conversation.Buddies.Added += conversation_Buddies_Added;
			
			base.Title = "Conversation with ";
			
			
			base.Resize (530, 380);
			
			menu = new ConversationMenu ();
			
			widget = new ConversationWidget (conv);
			statusbar = new Statusbar ();
			
			base.VBox.PackStart (menu, false, false, 0);
			base.VBox.PackStart (widget);
						
			base.ModifyBg (StateType.Normal,
				Theme.GetGdkColor (Theme.ConversationBackground));
			
		}
		
		protected override void OnShown ()
		{
			base.OnShown ();
			Debug.WriteLine ("Shown");
			//conversation.DataReceived += conversation_DataReceived;
		}
		
		protected override bool OnDeleteEvent (Gdk.Event args)
		{
			conversation.Close ();
			base.Hide ();
			base.Destroy ();
			return true;
		}
		
		private void conversation_Closed (object sender, EventArgs args)
		{
			new ThreadNotify ( 
			delegate {
				statusbar.Push (1, "Conversation closed by remote user");
			}).WakeupMain ();
			
			
		}
		private void conversation_Buddies_Added (object sender,
			WatchedCollectionEventArgs<Buddy> args)
		{
			RickiLib.Widgets.Utils.RunOnGtkThread (
				delegate {
					string title = "Conversation with : ";
					
					for (int i = 0; i < conversation.Buddies.Count; i ++) {
						title += conversation.Buddies [i].Username;
						if (i+1 < conversation.Buddies.Count)
							title +=", ";
					}
			
					base.Title = title;
				}
			);
		}
				
		public MsnpConversation Conversation {
			get { return conversation; }
			set { conversation = value; }
		}
	}
}
