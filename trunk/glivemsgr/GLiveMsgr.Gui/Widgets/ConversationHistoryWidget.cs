
using System;
using Gtk;
using RickiLib.Widgets;

using System.Net.Protocols;
using System.Net.Protocols.Msnp;

namespace GLiveMsgr.Gui
{
	
	
	public class ConversationHistoryWidget : CustomScrolledWindow
	{
		private RitchTextView view;
		
		private MsnpConversation conversation;
		
		public ConversationHistoryWidget (MsnpConversation conv)
		{
			this.conversation = conv;
			ShadowType = Gtk.ShadowType.In;
			view = new RitchTextView ();
			view.Editable = false;
			view.CanFocus = false;
			view.Buffer.InsertText += view_Buffer_InsertText;
			//view.SizeAllocated += view_SizeAllocated;
			
			base.ModifyBg (StateType.Normal,
				Theme.GetGdkColor (Theme.ConversationBackground));
			
			base.Add (view);
			this.conversation.DataSent += conversation_DataSent;
			conversation.DataGet += conversation_DataGet;
		}
		
		private void conversation_DataGet (object sender, DataEventArgs args)
		{
			// Updating GUI from ThreadNotify because DataReceived is launched from other thread
			Buddy buddy = args.Buddy;
			string remote_nickname = string.Empty;
			
			if (buddy != null) {
				remote_nickname = buddy.Alias;
			}
			
			new ThreadNotify (delegate {
				AppendText (string.Format (
					"{0} says:\n{1}\n\n",
					remote_nickname,
					args.Data));
			}).WakeupMain ();
		}
		
		private void conversation_DataSent (object sender, DataEventArgs args)
		{
			new ThreadNotify (delegate {
			AppendText (string.Format (
				"{0} says:\n{1}\n\n",
				conversation.Account.Alias,
				args.Data));
			}).WakeupMain ();
		}
		
		public void AppendText (string text)
		{
			Gtk.TextIter iter = view.Buffer.EndIter;
			
			view.Buffer.Insert (
				ref iter,
				text);
				
			view.Buffer.MoveMark (view.Buffer.InsertMark, view.Buffer.EndIter);
			
			view.ScrollToMark (view.Buffer.InsertMark, 0.4, true, 0.0, 1.0);
		}
		
		private void view_Buffer_InsertText (object sender,
			InsertTextArgs args)
		{
			//view.ScrollToIter (args.Pos, 0, false, 0, 0);
		}
		
		
//		private void view_SizeAllocated (object sender,
//			SizeAllocatedArgs args)
//		{
//			view.Scroll
//		}
		
		public RitchTextView RitchTextView {
			get {
				return view;
			}
		}
	}
}
