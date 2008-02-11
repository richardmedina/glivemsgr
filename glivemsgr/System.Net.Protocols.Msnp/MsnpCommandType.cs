
using System;

namespace System.Net.Protocols.Msnp
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
		
		Unknown
	}
}
