// ClientId.cs created with MonoDevelop
// User: ricki at 09:04 03/21/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace System.Net.Protocols.Msnp
{
	
	public enum ClientIdType {
		WindowsMovile = 1,
		Unknown = 2,
		CanReceiveInk =4,
		CanSendReceiveInk = 8,
		CanVideoConversation = 16,
		MultiPacketMessaging = 32,
		MsnMovil = 64,
		MsnDirect = 128,
		WebBasedMsnClient = 512,
		Msnc1 = 268435456,
		Msnc2 = 536870912,
		Msnc3 = 805306368,
		Msnc5 = 1073741824
	}
}
