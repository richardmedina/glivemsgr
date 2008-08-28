// MsnpGroup.cs created with MonoDevelop
// User: ricki at 6:04 PM 8/24/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;

namespace System.Net.Protocols.Msnp
{
	
	
	public class MsnpGroup : System.Collections.Generic.List <MsnpContact>
	{
		private int _id;
		private string _name;
		private string _description;
		
		public MsnpGroup ()
		{
			_id = 0;
			_name = string.Empty;
			_description = string.Empty;
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
