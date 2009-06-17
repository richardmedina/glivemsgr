// ConversationFileMenu.cs
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
	
	
	public class ConversationFileMenu : Gtk.Menu
	{
		private Gtk.MenuItem _mi_send;
		private Gtk.ImageMenuItem _mi_save;
		private Gtk.ImageMenuItem _mi_quit;
		
		public ConversationFileMenu ()
		{
			_mi_send = new MenuItem ("Send file...");
			_mi_save = new ImageMenuItem ("Save conversation...");
			_mi_save.Image = Gtk.Image.NewFromIconName (Stock.Save, IconSize.Menu);
			_mi_quit = new ImageMenuItem ("Quit", null);
			
			Append (_mi_send);
			Append (_mi_save);
			Append (new SeparatorMenuItem ());
			Append (_mi_quit);
			ShowAll ();
		}
	}
}