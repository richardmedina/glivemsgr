
using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Net.Protocols.Msnp
{
	
	
	public class MsnpGroup
	{
		
		private string name;
		private int id;
		
		public MsnpGroup (string name, int id)
		{
			this.name = name;
			this.id = id;
		}
						
		public string Name {
			get {
				return name;
			}
			set {
				name = value;
			}
		}
		
		public int Id {
			get {
				return id;
			}
			set {
				id = value;
			}
		}
		
/*		
		public new int Count {
			get {
				return base.Count;
			}
		}
		
		public IEnumerator GetEnumerator ()
		{
			return base.GetEnumerator ();
		}
*/
	}
}
