
using System;

namespace System.Net.Protocols
{
	
	
	public class DataArrivedArgs : System.EventArgs
	{
		private string data;
		
		public DataArrivedArgs (string data)
		{
			this.data = data;
		}
		
		public string Data {
			get {
				return data;
			}
		}
	}
}
