// DisplayPictureMenu.cs
// 
// Copyright (C) 2009 Ricaardo Medina López
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
using RickiLib.Widgets;

namespace Glivemsgr
{
	
	
	public class DisplayPictureMenu : RickiLib.Widgets.CustomMenu
	{
		private Gtk.MenuItem _itemChangePic;
		
		public DisplayPictureMenu () : base ("Change Display Picture")
		{
		}
	}
}
