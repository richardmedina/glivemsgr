// MessageStack.cs created with MonoDevelop
// User: ricki at 23:46 01/10/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;

namespace System.Net.Protocols
{
	
	
	public class MessageStack : 
		System.Collections.Generic.Stack <string>
	{
		
		public new void Push (string message)
		{
			base.Push (message);
		}
	}
}
