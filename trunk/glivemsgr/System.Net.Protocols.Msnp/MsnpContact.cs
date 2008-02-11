
using System;

namespace System.Net.Protocols.Msnp
{
	
	
	public class MsnpContact : Buddy
	{
		private string personalMessage;
		private MsnpGroupCollection groups;
		private int listsMask;
		
		private MsnpAccount _account;
		
		public MsnpContact (string email, string alias) : 
			base (email, alias, (int) MsnpContactState.Offline)
		{
			personalMessage = string.Empty;
			groups = new MsnpGroupCollection ();
			listsMask = 0;
		}
		
		public MsnpContact (string email) : 
			this (email, string.Empty)
		{
			
		}
		
		public override Conversation OpenConversation ()
		{
			//MsnpConversation conv = new MsnpConversation (
			Account.SendCommand ("XFR {0} SB", Account.TrId);
			MsnpConversation conv = new MsnpConversation (_account);
			
			
			return null;
		}
		
		public string PersonalMessage {
			get { return personalMessage; }
			set { personalMessage = value; }
			
		}
		
		public MsnpGroupCollection Groups {
			get { return groups; }
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
			get { return _account; }
			internal set { _account = value; }
		}
	}
}
