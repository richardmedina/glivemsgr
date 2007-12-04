
using System;
using System.Threading;
using Gtk;
using System.Net.Protocols;
using System.Net.Protocols.Msnp;

namespace GLiveMsgr.Gui
{
	
	
	public class MainWidget : Gtk.VBox
	{
		private Gtk.Notebook notebook;
		private LogonWidget logonWidget;
		private ContactListWidget contactListWidget;
		
		private MsnpAccount account;
		
		public MainWidget (MsnpAccount account)
		{
			this.account = account;
			this.account.Started += account_Started;
			//account.ConversationRequest += account_ConversationRequest;
			notebook = new Notebook ();
			notebook.ShowBorder = true;
			notebook.ShowTabs = false;
			notebook.ModifyBg (StateType.Normal,
				Theme.GdkColorFromCairo (Theme.BaseColor));
			
			logonWidget = new LogonWidget (account);
			contactListWidget = new ContactListWidget (account);
			//logonWidget.LogonData.ButtonConnect.Clicked += 
			//	ButtonConnect_Clicked;
			
			
			notebook.AppendPage (logonWidget,
				new Label ("Account Information"));
			
			notebook.AppendPage (contactListWidget,
				new Label ("ContactList"));
			
			base.PackStart (notebook);
		}
				
		private void account_Started (object sender, EventArgs args)
		{
			ThreadNotify notify = new ThreadNotify (
				delegate {
					notebook.Page = 1;
				});
			notify.WakeupMain ();
		}

		public MsnpAccount Account {
			get { return account; }
		}
		
		public Gtk.Notebook Notebook {
			get { return notebook; }
		}
	}
}
