
using System;
using System.Net.Protocols.Msnp;

namespace GLiveMsgr.Gui
{
	
	
	public class ContactListItem
	{
		private object obj;
		private ContactListItemType type;
		
		
		public ContactListItem (MsnpContact contact) : 
			this (contact, ContactListItemType.Buddy)
		{
		}
		
		public ContactListItem (MsnpGroup group) :
			this (group, ContactListItemType.Group)
		{
		}
		
		public ContactListItem (string message) :
			this (message, ContactListItemType.Message)
		{
		}
			
		private ContactListItem (object obj, ContactListItemType type)
		{
			this.obj = obj;
			this.type = type;
		}
		
		public object Obj {
			get { return obj; }
		}
		
		public ContactListItemType Type {
			get { return type; }
		}
	}
}
