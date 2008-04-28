
using System;
using System.Threading;
using Gtk;
using System.Net.Protocols;
using System.Net.Protocols.Msnp;
using RickiLib.Types;

namespace GLiveMsgr.Gui
{
	
	public class MainWidget : Gtk.VBox
	{
		private Gtk.Notebook _notebook;
		private LogonWidget _logonWidget;
		private ContactListWidget _contactListWidget;
		
		private WindowCollection windows;
		
		private MsnpAccount _account;
		
		public MainWidget (MsnpAccount account)
		{
			_account = account;
			_account.Started += account_Started;
			_account.Terminated += account_Terminated;
			
			windows = new WindowCollection ();
			//account.ConversationRequest += account_ConversationRequest;
			_account.Conversations.Added += account_ConversationAdded;
			_account.Conversations.Removed += account_ConversationRemoved;
			
			_notebook = new Notebook ();
			_notebook.ShowBorder = false;
			_notebook.ShowTabs = false;
			_notebook.ModifyBg (StateType.Normal,
				Theme.GdkColorFromCairo (Theme.BaseColor));
			
			_logonWidget = new LogonWidget (account);
			_contactListWidget = new ContactListWidget (account);
			//logonWidget.LogonData.ButtonConnect.Clicked += 
			//	ButtonConnect_Clicked;
			
			
			_notebook.AppendPage (_logonWidget,
				new Label ("Account Information"));
			
			_notebook.AppendPage (_contactListWidget,
				new Label ("ContactList"));
			
			PackStart (_notebook);
		}
				
		private void account_Started (object sender, EventArgs args)
		{
			ThreadNotify notify = new ThreadNotify (
				delegate {
					_notebook.Page = 1;
				});
			notify.WakeupMain ();
		}
		
		private void account_ConversationAdded (object sender, 
			WatchedCollectionEventArgs <MsnpConversation> args)
		{
			args.Instance.Activated += conversation_Activated;
			Console.WriteLine ("Conversation created MainWidget");
			RickiLib.Widgets.Utils.RunOnGtkThread (delegate {
				ConversationWindow window = new ConversationWindow (args.Instance);
				windows.Add (window);
			});
		}
				
		private void account_ConversationRemoved (object sender,
			WatchedCollectionEventArgs <MsnpConversation> args)
		{
			Console.WriteLine ("MainWidget.ConversationRemoved");
			RickiLib.Widgets.Utils.RunOnGtkThread (delegate {
				foreach (ConversationWindow window in windows) {
					if (window.Conversation == args.Instance &&
						window.Visible == false) {
						windows.Remove (window);
						window.Destroy ();
						break;
					}
				}
			});
		}
		
		private void account_Terminated (object sender, EventArgs args)
		{
			RickiLib.Widgets.Utils.RunOnGtkThread (delegate {
				_logonWidget.LogonData.Reset ();
				_notebook.Page = 0;
			});
		}
		
		private void conversation_Activated (object sender,
			EventArgs args)
		{
			Console.WriteLine ("Conversation with activated");
			//	((MsnpConversation) sender).RemoteContact);
			RickiLib.Widgets.Utils.RunOnGtkThread (delegate {
				foreach (ConversationWindow window in windows) {
					if (window.Conversation == sender) {
						if (!window.Visible) {
							window.Show ();
						}
					}
				}
			});
		}

		public MsnpAccount Account {
			get { return _account; }
		}
		
		public Gtk.Notebook Notebook {
			get { return _notebook; }
		}
	}
}
