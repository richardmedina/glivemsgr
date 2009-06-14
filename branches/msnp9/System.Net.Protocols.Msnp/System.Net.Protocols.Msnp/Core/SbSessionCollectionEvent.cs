
using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Net.Protocols.Msnp.Core
{
	
	public delegate void SBSessionCollectionEventHandler (object sender,
		SBSessionCollectionEventArgs args);
	
	public class SBSessionCollectionEventArgs : System.EventArgs
	{
		private MsnpSBSession _session;
		
		public SBSessionCollectionEventArgs (MsnpSBSession session)
		{
			_session = session;
		}
		
		public MsnpSBSession Session {
			get { return _session; }
		}
	
	}
}
