
using System;

namespace System.Net.Protocols.Msnp
{
	
	
	public class MsnpContact : Buddy
	{
		private string personalMessage;
		private MsnpGroupCollection groups;
		private int listsMask;
		
		private MsnpAccount account;
		
		public MsnpContact (string email, string alias) : 
			base (email, alias, (int) MsnpContactState.Offline)
		{
			this.personalMessage = string.Empty;
			this.groups = new MsnpGroupCollection ();
			this.listsMask = 0;
		}
		
		public MsnpContact (string email) : 
			this (email, string.Empty)
		{
			
		}
		
		public override Conversation OpenConversation ()
		{
			//MsnpConversation conv = new MsnpConversation (
			this.Account.SendCommand ("XFR {0} SB", MsnpAccount.TrId);
			
			return null;
		}
		
		public string PersonalMessage {
			get { return personalMessage; }
			set { personalMessage = value; }
			
		}
		
		public MsnpGroupCollection Groups {
			get { return groups; }
			
			//set { groups = value; }
		}
		
		public int ListsMask {
			get { return listsMask;	}
			set { listsMask = value; }
		}
		
		public new MsnpContactState State {
			get { return (MsnpContactState) base.State; }
			set { base.State = (int) value; }
		}
		
		public MsnpAccount Account {
			get { return account; }
			internal set { account = value; }
		}
	}
}
