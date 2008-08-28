// PassportArrivedHandler.cs created with MonoDevelop
// User: ricki at 1:21 PM 8/24/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace System.Net.Protocols.Msnp
{
	
	public delegate void PassportArrivedHandler (object sender, 
		PassportArrivedArgs args);
	
	public class PassportArrivedArgs
	{
		private string _content;
		
		public PassportArrivedArgs (string content)
		{
			_content = content;
		}
		
		public string Content {
			get { return _content; }
		}
	}
}
