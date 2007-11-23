
using System;

namespace System.Net.Protocols
{
	
	public delegate void TypingHandler (object sender, TypingArgs args);
	
	public class TypingArgs : System.EventArgs
	{
		private Buddy buddy;
		
		public TypingArgs (Buddy buddy)
		{
			this.buddy = buddy;
		}
		
		public Buddy Buddy {
			get { return buddy; }
		}
	}
}
