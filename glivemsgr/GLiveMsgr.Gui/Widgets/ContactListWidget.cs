
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
			
			BorderWidth = 10;
			
			Spacing = 5;
			
			header = new ContactListHeader (account);
			aliasButton = new AliasChangeButton ();
			list = new ContactList (account);
						
			ScrolledWindow scrolled = new ScrolledWindow ();
			scrolled.ShadowType = ShadowType.EtchedOut;
			scrolled.Add (list);
			
			PackStart (header, false, false, 0);
			PackStart (aliasButton, false, false, 0);
			PackStart (scrolled);
			ShowAll ();
		}
				
		public ContactList List {
			get { return list; }
		}
		
		public MsnpAccount Account {
			get { return account; }
		}
	}
}
