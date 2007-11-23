
using System;

namespace GLiveMsgr.Gui
{

	public delegate void EmoticonSelectedHandler (object sender,
		EmoticonSelectedArgs args);
	
	public class EmoticonSelectedArgs : System.EventArgs {
		
		private Emoticon emoticon;
		
		public EmoticonSelectedArgs (Emoticon emoticon)
		{
			this.emoticon = emoticon;
		}
		
		public Emoticon Emoticon {
			get {  return emoticon; }
		}
	}
	
}
