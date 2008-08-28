using System;

namespace System.Net.Protocols.Msnp.Core
{


	public enum MsnpCommandType
	{
		//Authentication Commands
		CVR,
		USR,
		VER,
		//		XFR,
		//Notification Commands
		SYN,
		LSG,
		LST,
		CHG,
		FLN,
		ILN,
		NLN,
		CHL,
		RNG,
		XFR,
		REA,
		PNG,
		// Dispatch Commands
		MSG,
		
		Unknown
	}
}
