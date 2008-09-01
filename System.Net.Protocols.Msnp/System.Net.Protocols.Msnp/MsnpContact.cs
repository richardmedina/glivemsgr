// MsnpContact.cs created with MonoDevelop
// User: ricki at 6:09 PM 8/24/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

using System.Net.Protocols.Msnp.Core;

namespace System.Net.Protocols.Msnp
{
	
	
	public class MsnpContact
	{
		private string _username;
		private string _alias;
		
		private MsnpDispatchServer _dispatch;
		
		public MsnpContact (MsnpDispatchServer dispatch, string username) :
			this (dispatch, username, string.Empty)
		{
		}
		
		public MsnpContact (
			MsnpDispatchServer dispatch,
			string username, 
			string alias)
		{
			_username = username;
			_alias = alias;
			_dispatch = dispatch;
		}
		
		public string Username {
			get { return _username; }
		}
		
		public string Alias {
			get { return _alias; }
		}
		
		public MsnpDispatchServer DispatchServer {
			get { return _dispatch; }
		}
	}
}
