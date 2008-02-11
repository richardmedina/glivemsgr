
using System;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public class FixedComboBoxEntry : FixedEntry
	{
		private string [] _emails;
		
		public FixedComboBoxEntry (string [] emails) :
			base (new Image (Stock.Info, IconSize.Menu), null)
		{
			_emails = emails;
			AccountsMenu accountsMenu = new AccountsMenu (emails);
			accountsMenu.Selected += accountsMenu_Selected;
			Menu = accountsMenu;
		}
		
		private void accountsMenu_Selected (object sender, 
			AccountSelectionArgs args)
		{
			Entry.Text = args.Username;
		}
		
		public string [] Emails {
			get { return _emails; }
		}		
	}
	
	public delegate void AccountSelectionHandler (object sender, 
		AccountSelectionArgs args);
	
	public class AccountSelectionArgs : System.EventArgs
	{
		private string _username;
		private string _password;
		
		public AccountSelectionArgs (string username, string password)
		{
			_username = username;
			_password = password;
		}
		
		public string Username {
			get { return _username; }
		}
		
		public string Password {
			get { return _password; }
		}
	}
	
	public class AccountsMenu : Gtk.Menu 
	{
		private Gtk.MenuItem [] items;
		
		private string [] _accounts;
		
		private event AccountSelectionHandler _selected;
		
		public AccountsMenu (string [] accounts)
		{
			_accounts = accounts;
			
			items = new Gtk.MenuItem [_accounts.Length];
			
			for (int i = 0; i < _accounts.Length; i ++) {
				items [i] = new MenuItem (accounts [i]);
				items [i].Activated += item_activated;
				Append (items [i]);
			}
			Append (new SeparatorMenuItem ());
			Gtk.ImageMenuItem itemAdd = new ImageMenuItem (Stock.Add, null);
			itemAdd.Activated += item_activated;
			
			Append (itemAdd);
			
			_selected = onSelected;
			
			ShowAll ();
		}
		
		protected virtual void OnSelected (string username, string password)
		{
			_selected (this, new AccountSelectionArgs (username, password));
		}
		
		private void item_activated (object sender, EventArgs args)
		{
			for (int i = 0; i < items.Length; i ++) {
				if (items [i] == sender) {
					OnSelected (_accounts [i], string.Empty);
					return;
				}
			}
			
			OnSelected (string.Empty, string.Empty);
		}
		
		private void onSelected (object sender, 
			AccountSelectionArgs args)
		{
		}
		
		public event AccountSelectionHandler Selected {
			add { _selected += value; }
			remove { _selected -= value; }
		}
	}

}
