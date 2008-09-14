// MsnpGroup.cs created with MonoDevelop
// User: ricki at 6:04 PM 8/24/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;

namespace System.Net.Protocols.Msnp
{
	
	
	public class Group : ObservedCollection <Contact>
	{
		private int _id;
		private string _name;
		private string _description;
		
		public Group () : this (0, string.Empty, string.Empty)
		{
		}
		
		public Group (string name) : this (0, name, string.Empty)
		{
		}
		
		public Group (int id, string name) : 
			this (id, name, string.Empty)
		{
		}
		
		public Group (int id, string name, string description)
		{
			_id = id;
			_name = name;
			_description = description;
		}
		
		protected override void OnAdded (Contact instance)
		{
			base.OnAdded (instance);
		}

		
		public int Id {
			get { return _id; }
		}
		
		public string Name {
			get { return _name; }
		}
		
		public string Description {
			get { return _description; }
		}
	}
}
