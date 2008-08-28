using System;
using System.Collections.Generic;
using System.Text;

namespace System.Net.Protocols.Msnp.Core
{
	public delegate void NotificationSucessHandler (object sender, 
		NotificationSuccessArgs args);
		
	public class NotificationSuccessArgs : System.EventArgs
	{
		private string _hostname;
		private int _port;
		
		public NotificationSuccessArgs (string hostname, int port)
		{
			_hostname = hostname;
			_port = port;
		}
		
		public string Hostname {
			get { return _hostname; }
		}
		
		public int Port {
			get { return _port; }
		}
	}
}
