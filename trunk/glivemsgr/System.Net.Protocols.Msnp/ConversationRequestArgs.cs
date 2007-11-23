// conversationRequestArgs.cs created with MonoDevelop
// User: ricki at 12:27 a 08/10/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace System.Net.Protocols.Msnp
{
	
	public delegate void ConversationRequestHandler (object sender,
		ConversationRequestArgs args);
		
	public class ConversationRequestArgs : System.EventArgs
	{
		private MsnpConversation conversation;
		
		public ConversationRequestArgs (MsnpConversation conv)
		{
			this.conversation = conv;
		}
		
		public MsnpConversation Conversation {
			get { return conversation; }
		}
	}
}
