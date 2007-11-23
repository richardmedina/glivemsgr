
using System;

namespace System.Net.Protocols.Msnp
{
	
	
	public enum MsnpListType
	{
		Allow		= 1 << 0,
		Block		= 1 << 1,
		Forward		= 1 << 2,
		Reverse		= 1 << 3,
	}
}
