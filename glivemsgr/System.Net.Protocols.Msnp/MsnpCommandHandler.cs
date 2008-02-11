// MsnpCommand.cs created with MonoDevelop
// User: ricki at 06:42 01/26/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace System.Net.Protocols.Msnp
{
	
	public delegate void MsnpCommandHandler (
		object sender,
		MsnpCommandArgs Args);
		
	public class MsnpCommandArgs : System.EventArgs
	{
		
		private MsnpCommand _command;
		
		public MsnpCommandArgs (MsnpCommand command)
		{
			_command = command;
		}
		
		public MsnpCommand Command {
			get { return _command; }
		}
	}
}
