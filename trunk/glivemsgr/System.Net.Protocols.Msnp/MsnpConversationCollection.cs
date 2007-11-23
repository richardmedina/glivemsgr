
using System;
using System.Collections.Generic;
using System.Net.Protocols.Msnp;

using RickiLib.Types;

namespace System.Net.Protocols.Msnp
{
	
	
	public class MsnpConversationCollection : 
		RickiLib.Types.WatchedCollection <MsnpConversation>
	{
		
		public MsnpConversationCollection ()
		{
		}
		
		public new void Add (MsnpConversation conv)
		{
			base.Add (conv);
		}
		
		public new void Remove (MsnpConversation conv)
		{
			base.Remove (conv);
		}
	}
}
