
using System;
using System.Collections;
using System.Collections.Generic;

namespace GLiveMsgr.Gui
{
	
	
	public class EmoticonCollection : 
		System.Collections.Generic.SortedList <int, Emoticon>
	{
		
		public EmoticonCollection ()
		{
				
		}
		
		public new void Add (int key, Emoticon emoticon)
		{
			base.Add (key, emoticon);
		}
		
		public new bool Remove (int key)
		{
			return base.Remove (key);
		}
		
		public new void RemoveAt (int index)
		{
			base.RemoveAt (index);
		}
	}
}
