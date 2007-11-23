
using System;
using System.Collections.Generic;

namespace System.Net.Protocols.Msnp
{
	
	
	public class CommandQueue : System.Collections.Generic.Queue <string>
	{
		
		public CommandQueue()
		{
		}
		
		public new void Enqueue (string command)
		{
			base.Enqueue (command);
		}
		
		public new string Dequeue ()
		{
			return base.Dequeue ();
		}
	}
}
