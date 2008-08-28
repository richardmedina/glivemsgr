// MsnpCommandArrived.cs created with MonoDevelop
// User: ricki at 7:34 PM 8/24/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Net.Protocols.Msnp.Core;

namespace System.Net.Protocols.Msnp.Core
{
	public delegate void MsnpCommandArrivedHandler (object sender,
		MsnpCommandArrivedArgs args);
	
	public class MsnpCommandArrivedArgs
	{
		private MsnpCommand _command;
		
		public MsnpCommandArrivedArgs (MsnpCommand command)
		{
			_command = command;
		}
		
		public MsnpCommand Command {
			get { return _command; }
		}
	}
}
