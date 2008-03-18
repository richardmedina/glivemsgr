
using System;

namespace GLiveMsgr.Gui
{
	
	
	public class ConversationToolbar : Toolbar
	{
		
		public ConversationToolbar()
		{
			base.Items.Add (new ToolbarButton ("images/x.png"));
			/*
			backgroundBrush = new SolidBrush (
				Theme.ConversationBackground);
			
			*/
		}
		/*
		protected override Brush BackgroundBrush {
			get { return backgroundBrush; }
			set { backgroundBrush = value; }
		}
		*/

	}
}
