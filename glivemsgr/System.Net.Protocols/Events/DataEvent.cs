// ConversationDataEvent.cs created with MonoDevelop
// User: ricki at 01:36 a 22/10/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace System.Net.Protocols
{
	
	public delegate void DataEventHandler (object sender,
		DataEventArgs args);
		
	public class DataEventArgs : System.EventArgs
	{
		private Buddy buddy;
		private string data;
		
		public DataEventArgs (Buddy buddy, string data)
		{
			this.data = data;
			this.buddy =buddy;
		}
		
		public DataEventArgs (string data) : this (null, data)
		{
		}
		
		public Buddy Buddy {
			get { return buddy; }
		}
		
		public string Data {
			get { return data; }
		}
	}
}
