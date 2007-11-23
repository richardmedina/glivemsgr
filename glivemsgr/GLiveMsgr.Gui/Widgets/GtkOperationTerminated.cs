
using System;

namespace GLiveMsgr.Gui
{
	
	public delegate void GtkOperationTerminatedHandler (object sender,
		GtkOperationTerminatedArgs args);
	
	public class GtkOperationTerminatedArgs : System.EventArgs
	{
		private bool retval;
		
		public GtkOperationTerminatedArgs (bool retval)
		{
			this.retval = retval;
		}
		
		public bool RetVal {
			get { return retval; }
		}
	}
}
