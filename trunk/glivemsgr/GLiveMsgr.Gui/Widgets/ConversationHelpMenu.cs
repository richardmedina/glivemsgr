// ConversationHelpMenu.cs
// 
// Copyright (C) 2009 Ricardo Medina López
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public class ConversationHelpMenu : Gtk.Menu
	{
		private Gtk.ImageMenuItem _mi_help;
		private Gtk.ImageMenuItem _mi_home;
		private Gtk.ImageMenuItem _mi_update;
		private Gtk.ImageMenuItem _mi_about;
		
		public ConversationHelpMenu()
		{
			_mi_help = new ImageMenuItem (Stock.Help, null);
			_mi_home = new ImageMenuItem (Stock.Home, null);
			_mi_update = new ImageMenuItem (Stock.Network, null);
			_mi_about = new ImageMenuItem (Stock.About, null);
			_mi_about.Activated += mi_aboutActivated;
			
			Append (_mi_help);
			Append (new SeparatorMenuItem ());
			Append (_mi_home);
			Append (_mi_update);
			Append (new SeparatorMenuItem ());
			Append (_mi_about);
			ShowAll ();
		}
		
		private void mi_aboutActivated (object sender, EventArgs args)
		{
			GLiveMsgrAboutDialog d = new GLiveMsgrAboutDialog ();
			d.Run ();
			d.Destroy ();
		}
	}
}
