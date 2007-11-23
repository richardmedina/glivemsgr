
using System;

namespace System.Net.Protocols
{
	
	public delegate void DataReceivedHandler (object sender,
		DataReceivedArgs args);
	
	public class DataReceivedArgs : System.EventArgs
	{
		private string data;
		//private Conversation conversation;
		
		public DataReceivedArgs (string data)
		{
			this.data = data;
		}
		
		public string Data {
			get { return data; }
		}
	}
}
