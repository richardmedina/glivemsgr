
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
				//contactListWidget = new ContactListWidget (account);
				//notebook.AppendPage (contactListWidget,
				//new Label ("ContactList"));

				//contactListWidget.List.PopulateList ();
				notebook.Page = 1;
				});
			notify.WakeupMain ();
			/*
			foreach (MsnpContact contact in account.Buddies) {
				Debug.WriteLine ("{0} Alias {1} State {2}, Mask {3}, Groups : {4}",
					contact.Username, contact.Alias, contact.State,contact.ListsMask, contact.Groups.Count);
				foreach (MsnpGroup group in contact.Groups)
					Debug.WriteLine ("\t{0} with id {0}", group.Name, group.Id);
			}
			*/
		}

/*				
		private bool connect ()
		{
			int ret = account.Login ();
			Console.WriteLine ("Login returns: {0}", ret);
			
			return ret == 0;
		}
		
		private void oper_Terminated (object sender, 
			GtkOperationTerminatedArgs args)
		{
			//if (args.RetVal)
			//	notebook.Page = 1;
		}
*/		
		public MsnpAccount Account {
			get { return account; }
		}
		
		public Gtk.Notebook Notebook {
			get { return notebook; }
		}
	}
}
