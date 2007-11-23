
using System;

namespace GLiveMsgr.Gui
{
	
	
	public class EntryEditingToolbar : Toolbar
	{
		private ToolbarButton buttonEmoticons;
		private ToolbarButton buttonWinks;
		private EmoticonBox emoticonBox;
		
		public EntryEditingToolbar()
		{
			buttonEmoticons = new ToolbarButton ("images/butsmile.png");
			buttonWinks = new ToolbarButton ("images/butwinks.png");
			
			buttonEmoticons.Clicked += buttonEmoticons_Clicked;
			emoticonBox = new EmoticonBox ();
			
			base.Items.Add (buttonEmoticons);
			base.Items.Add (buttonWinks);
		}
		
		private void buttonEmoticons_Clicked (object sender,
			EventArgs args)
		{
			int x, y;
			this.GdkWindow.GetOrigin (out x, out y);
						
			emoticonBox.Shown += delegate {
				emoticonBox.GdkWindow.Move (x -1, 
					 y - emoticonBox.Allocation.Height);
			};
			
			emoticonBox.ShowAll ();
			emoticonBox.Present ();
		}
		
		public EmoticonBox EmoticonBox {
			get { return emoticonBox; }
		}
	}
}
