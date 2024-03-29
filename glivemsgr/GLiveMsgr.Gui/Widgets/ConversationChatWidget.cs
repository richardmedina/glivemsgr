
using System;
using Gtk;
using RickiLib.Widgets;
using System.Net.Protocols.Msnp;

namespace GLiveMsgr.Gui
{
	
	
	public class ConversationChatWidget : Gtk.EventBox
	{			
		private ConversationHistoryWidget history;
		private EntryEditingToolbar toolbar;
		private ConversationEntryWidget entry;
		
		private Gtk.VBox vbox;
		private Gtk.VPaned vpaned;
		
		private MsnpConversation conversation;
		
		public ConversationChatWidget (MsnpConversation conv)
		{
			this.conversation = conv;
			history = new ConversationHistoryWidget (conversation);
			toolbar = new EntryEditingToolbar ();
			entry = new ConversationEntryWidget (conversation);
			
			toolbar.EmoticonBox.Selected += EmoticonBox_Selected;
			
			vpaned = new VPaned ();
			vbox = new VBox (false, 0);
			
			vbox.PackStart (toolbar, false, false, 0);
			vbox.PackStart (entry);
			
			Gtk.Viewport viewport = new Viewport ();
			//ConversationBackground
			viewport.ModifyBg (StateType.Normal,
				Theme.GdkColorFromCairo (Theme.BgColor));
			
			viewport.Add (vbox);
			vpaned.Pack1 (history, true, true);
			vpaned.Pack2 (viewport, false, false );
			
			base.Add (vpaned);
			//ConverationBackground
			base.ModifyBg (StateType.Normal,
				Theme.GdkColorFromCairo (Theme.BgColor));
		}
		
		private void EmoticonBox_Selected (object sender, EmoticonSelectedArgs args)
		{
			entry.RitchTextView.InsertAtCursor (args.Emoticon.Trigger);
		}
		
		public ConversationHistoryWidget History {
			get { return history; }
		}
		
		public ConversationEntryWidget Entry {
			get { return entry; }
		}
	}
}
