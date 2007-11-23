
using System;
using Gtk;
using RickiLib.Widgets;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GLiveMsgr.Gui
{
	
	
	public class ConversationMenu : Gtk.MenuBar
	{
		private CustomMenu [] menus;
		
		private readonly static string [] menu_labels = {
			"_File",
			"_Edit",
			"_Actions",
			"_Tools",
			"Help"
		};
		
		public ConversationMenu ()
		{
			menus = new CustomMenu [menu_labels.Length];
			
			for (int i = 0; i < menus.Length; i ++) {
				menus [i] = new CustomMenu (menu_labels [i]);
				base.Append (menus [i]);
			}
		}
	}
}
