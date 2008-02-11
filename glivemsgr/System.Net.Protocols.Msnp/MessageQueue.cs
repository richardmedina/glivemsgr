// MessageQueue.cs created with MonoDevelop
// User: ricki at 00:50 02/11/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace System.Net.Protocols.Msnp
{
	
	
	public class MessageQueue : 
		System.Collections.Generic.Queue <string>
	{
		
		public MessageQueue ()
		{
		}
		
		public new void Enqueue (string message)
		{
			base.Enqueue (message);
		}
		
		public new string Dequeue (string message)
		{
			return base.Dequeue ();
		}
	}
}
