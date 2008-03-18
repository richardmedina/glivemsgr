// ConversationButtonbar.cs created with MonoDevelop
// User: ricki at 01:13 02/25/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public class ConversationButtonbar : Buttonbar
	{
		
		private ButtonbarItem _fileButton;
		private ButtonbarItem _editButton;
		
		public ConversationButtonbar ()
		{
			_fileButton = new ButtonbarItem ("_File");
			_fileButton.Relief = ReliefStyle.None;
			
			_editButton = new ButtonbarItem ("_Edit");
			_editButton.Relief = ReliefStyle.None;
			
			Append (_fileButton);
			Append (_editButton);
		}
	}
}
