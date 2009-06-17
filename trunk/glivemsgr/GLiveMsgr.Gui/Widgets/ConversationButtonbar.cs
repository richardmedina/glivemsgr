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
		private ButtonbarItem _btn_about;
		
		public ConversationButtonbar ()
		{
			_fileButton = new ButtonbarItem ("_File");
			_fileButton.Relief = ReliefStyle.None;
			_fileButton.Menu = new ConversationFileMenu ();
			
			_editButton = new ButtonbarItem ("_Edit");
			_editButton.Relief = ReliefStyle.None;
			
			_btn_about = new ButtonbarItem ("_About");
			_btn_about.Menu = new ConversationHelpMenu ();
			
			Append (_fileButton);
			Append (_editButton);
			Append (_btn_about);
		}
	}
}
