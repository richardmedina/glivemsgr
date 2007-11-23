
using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
namespace System.Net.Protocols.Msnp
{
	
	
	public class MsnpCertificatePolicy : ICertificatePolicy
	{
		
		public bool CheckValidationResult (ServicePoint sp,
			X509Certificate cert, WebRequest req, int error)
		{
			return true;
		}
	}
}
