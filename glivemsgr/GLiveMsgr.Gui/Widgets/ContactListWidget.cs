
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
		
		private MsnpAccount _account;
		
		public ContactListWidget (MsnpAccount account)
		{
			_account = account;
			_account.AliasChanged += account_AliasChanged;
			_account.StateChanged += account_StateChanged;
			
			BorderWidth = 10;
			
			Spacing = 5;
			
			header = new ContactListHeader (_account);
			//aliasButton = new AliasChangeButton ();
			header.AliasButton.EditableLabel.Changed += aliasButton_EditableLabel_Changed;
			header.AliasButton.StateMenu.Changed += aliasButton_StateMenu_Changed;
			
			list = new ContactList (_account);
						
			ScrolledWindow scrolled = new ScrolledWindow ();
			scrolled.ShadowType = ShadowType.EtchedOut;
			scrolled.Add (list);
			
			PackStart (header, false, false, 0);
			//PackStart (aliasButton, false, false, 0);
			PackStart (scrolled);
			ShowAll ();
		}
		
		private void account_AliasChanged (object sender, EventArgs args)
		{
			RickiLib.Widgets.Utils.RunOnGtkThread (delegate {
				header.AliasButton.EditableLabel.Text = _account.Alias;
			});
		}
		
		private void account_StateChanged (object sender, EventArgs args)
		{
			/*
			RickiLib.Widgets.Utils.RunOnGtkThread (delegate {
				aliasButton.StateMenu.State = _account.State;
			});
			*/
		}
		
		private void aliasButton_EditableLabel_Changed (object sender, EventArgs args)
		{
			_account.ChangeAlias (header.AliasButton.EditableLabel.Text);
		}
		
		private void aliasButton_StateMenu_Changed (object sender, EventArgs args)
		{
			if (_account.Logged) {
				_account.ChangeState (header.AliasButton.StateMenu.State);
			}
		}
				
		public ContactList List {
			get { return list; }
		}
		
		public MsnpAccount Account {
			get { return _account; }
		}
	}
}
