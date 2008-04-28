// MsnpObjectHeaderFlags.cs created with MonoDevelop
// User: ricki at 09:10 03/21/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace System.Net.Protocols.Msnp
{
	
	
	public enum MsnpObjectHeaderFlags
	{
		None = 0x00,
		Unknown = 0x01,
		Acknowledgement = 0x02,
		WaitingForAReply = 0x04,
		Error = 0x08,
		Unknown2 = 0x10,
		DpOrCe = 0x20,
		HeWhoGotBye = 0x40,
		HeWhoSentBye = 0x80,
		FileTransfer = 0x1000030
	}
}
