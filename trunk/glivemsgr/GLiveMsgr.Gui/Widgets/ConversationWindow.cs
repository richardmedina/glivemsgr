
using System;
using System.Threading;
using RickiLib.Types;
using RickiLib.Widgets;
using Gtk;

using System.Net.Protocols;
using System.Net.Protocols.Msnp;

namespace GLiveMsgr.Gui
{
	
	public class ConversationWindow : PopupWindow
	{
		private MsnpConversation conversation;

		private ConversationMenu menu;
		private ConversationWidget widget;
		private Gtk.Statusbar statusbar;
		
		public ConversationWindow (MsnpConversation conv)
		{
			Decorated = false;
			LogoVisible = false;
			conversation = conv;
			conversation.DataGet += conversation_DataGet; 
			
			Gtk.VBox VBox = new VBox (false, 0);
			base.Add (VBox);
			conversation.Buddies.Added += conversation_Buddies_Added;
			
			Title = "Conversation with ";
			
			Resize (530, 380);
			
			menu = new ConversationMenu ();
			
			widget = new ConversationWidget (conv);
			statusbar = new Statusbar ();
			
			VBox.PackStart (new ConversationButtonbar (), false, false, 0);
			VBox.PackStart (widget);
			VBox.ShowAll ();
			
			// ConversationBackground
			//ModifyBg (StateType.Normal,
			//	Theme.GdkColorFromCairo (Theme.BgColor));
			
		}
		
		protected override void OnActivate () 
		{
			base.OnActivate ();
		}
		
		protected override void OnShown ()
		{
			base.OnShown ();
			Debug.WriteLine ("Shown");
			//conversation.DataReceived += conversation_DataReceived;
		}
		
		protected override bool OnDeleteEvent (Gdk.Event args)
		{
			//conversation.Close ();
			base.Hide ();
			return true;
		}
		
		private void conversation_DataGet (object sender, DataEventArgs args)
		{/*
			RickiLib.Widgets.Utils.RunOnGtkThread (delegate {
				if (!Visible)
					Present ();
			});*/
		}
		
		private void conversation_Closed (object sender, EventArgs args)
		{
			
			RickiLib.Widgets.Utils.RunOnGtkThread ( 
			delegate {
				statusbar.Push (1, "Conversation closed by remote user");
			});
			
			
		}
		private void conversation_Buddies_Added (object sender,
			WatchedCollectionEventArgs<Buddy> args)
		{
			RickiLib.Widgets.Utils.RunOnGtkThread (
				delegate {
					string title = string.Empty;
					
					for (int i = 0; i < conversation.Buddies.Count; i ++) {
						title += conversation.Buddies [i].Alias;
						if (i+1 < conversation.Buddies.Count)
							title +=", ";
					}
			
					base.Title = title;
				}
			);
		}
				
		public MsnpConversation Conversation {
			get { return conversation; }
			set { 
				conversation = value;
				conversation.Closed += conversation_Closed;
			}
		}
	}
}
