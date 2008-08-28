using System;
using System.Collections.Generic;
using System.Text;

namespace System.Net.Protocols.Msnp.Core
{
	
	public class Switchboard
	{
		
		private SBSessionCollection _sessions; 
		
		public Switchboard ()
		{
			_sessions = new SBSessionCollection ();
		}
		
		public SBSessionCollection Sessions {
			get { return _sessions; }
		}
	}
}
