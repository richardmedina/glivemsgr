
using System;

namespace System.Net.Protocols.Msnp.Core
{
	
	public delegate void DataArrivedHandler (object sender,
        DataArrivedArgs args);
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
