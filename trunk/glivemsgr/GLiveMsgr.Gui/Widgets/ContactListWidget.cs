
using System;
using Gtk;
using RickiLib.Widgets;
using System.Net.Protocols;
using System.Net.Protocols.Msnp;

namespace GLiveMsgr.Gui
{
	
	
	public class ContactListWidget : Gtk.VBox
	{
		private ContactListHeader header;
		private ContactList list;
		private AliasChangeButton aliasButton;
		
		private MsnpAccount account;
		
		public ContactListWidget (MsnpAccount account)
		{
			this.account = account;
			this.account.AliasChanged += account_AliasChanged;
			this.account.StateChanged += account_StateChanged;
			
			BorderWidth = 10;
			
			Spacing = 5;
			
			header = new ContactListHeader (account);
			aliasButton = new AliasChangeButton ();
			aliasButton.EditableLabel.Changed += aliasButton_EditableLabel_Changed;
			aliasButton.StateMenu.Changed += aliasButton_StateMenu_Changed;
			
			list = new ContactList (account);
						
			ScrolledWindow scrolled = new ScrolledWindow ();
			scrolled.ShadowType = ShadowType.EtchedOut;
			scrolled.Add (list);
			
			PackStart (header, false, false, 0);
			PackStart (aliasButton, false, false, 0);
			PackStart (scrolled);
			ShowAll ();
		}
		
		private void account_AliasChanged (object sender, EventArgs args)
		{
			RickiLib.Widgets.Utils.RunOnGtkThread (delegate {
				aliasButton.EditableLabel.Text = account.Alias;
			});
		}
		
		private void account_StateChanged (object sender, EventArgs args)
		{
			RickiLib.Widgets.Utils.RunOnGtkThread (delegate {
				aliasButton.StateMenu.State = account.State;
			});
		}
		
		private void aliasButton_EditableLabel_Changed (object sender, EventArgs args)
		{
			account.ChangeAlias (aliasButton.EditableLabel.Text);
		}
		
		private void aliasButton_StateMenu_Changed (object sender, EventArgs args)
		{
			//account.ChangeState (aliasButton.StateMenu.State);
		}
				
		public ContactList List {
			get { return list; }
		}
		
		public MsnpAccount Account {
			get { return account; }
		}
	}
}
