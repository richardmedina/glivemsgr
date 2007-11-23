
using System;
using System.Collections;
using System.Collections.Generic;

namespace GLiveMsgr.Gui
{
	
	
	public class ToolbarItemCollection : List <ToolbarItem>
	{	
		public ToolbarItemCollection ()
		{
			
		}
		
		public new void Add (ToolbarItem item)
		{
			base.Add (item);
		}		
	}
}
