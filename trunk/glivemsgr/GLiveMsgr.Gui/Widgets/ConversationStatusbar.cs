// ConversationStatusbar.cs created with MonoDevelop
// User: ricki at 05:59 02/29/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public class ConversationStatusbar : Gtk.Statusbar
	{
		
		public ConversationStatusbar()
		{
			ModifyBg (StateType.Normal,
			          Theme.GdkColorFromCairo (Theme.BgColor));
			ModifyFg (StateType.Normal,
			          Theme.GdkColorFromCairo (Theme.BaseColor));
			ModifyBase (StateType.Normal,
			            Theme.GdkColorFromCairo (Theme.BgColor));
		}
	}
}
